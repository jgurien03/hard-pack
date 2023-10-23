using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SphereController : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, jumpForce = 10f;
    private float speedInput, turnInput;
    public bool isGrounded;
    private Quaternion initialRotation;
    private float previousSpeed;
    public GravityCtrl gravityController; // Reference to the GravityCtrl script
    private RaycastHit hit;
    float raycastDistance = 1.1f;
    public float counter = 0f;
    public float threshold = 0.1f; // Adjust the threshold value as needed
    public CinemachineBrain cinemachineBrain;
    public Transform worldUpOverrideTransform;
    public float increasedFOV = 50.0f;
    public float originalFOV = 40.0f;
    public CinemachineVirtualCamera virtualCamera;
    private float driftTime;
    private float currentDriftCharge = 0.0f;
    public float driftBoostForce = 1000.0f;
    private bool isDrifting = false;
    public float driftHopForce = 200f;
    private bool right;
    private bool left;
    private float boostTime = 0f;
    public float driftSteerForce = 10f;
    public Material materialToModify;
    public Material blurMaterial;
    public FollowPlayerPosition player1;
    private float totalRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb.transform.parent = null;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineBrain != null)
        {
            if (gravityController.IsStickingToWall || gravityController.IsInsideGravityZone)
            {
                cinemachineBrain.m_WorldUpOverride = worldUpOverrideTransform;
                virtualCamera.m_Lens.FieldOfView = increasedFOV;
            }
            else
            {
                cinemachineBrain.m_WorldUpOverride = null; // or null, to revert to the default behavior
                virtualCamera.m_Lens.FieldOfView = originalFOV;
            }
        }
        isGrounded = (gravityController.IsStickingToWall && (Physics.Raycast(transform.position, -transform.up, out hit, 1.1f) || Physics.Raycast(transform.position, transform.right, out hit, 1.1f) || 
                Physics.Raycast(transform.position, -transform.right, out hit, 1.1f) || Physics.Raycast(transform.position, -transform.up, out hit, 1.1f) || 
                Physics.Raycast(transform.position, -transform.forward, out hit, 1.1f) || Physics.Raycast(transform.position, transform.forward, out hit, 1.1f)))
            || Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);
        move();
        steer();
        drift();
        boost();
        if (!isGrounded)
        {
            float distanceToGround = 0f;
            RaycastHit groundHit;
            if (Physics.Raycast(transform.position, -transform.up, out groundHit))
            {
                distanceToGround = groundHit.distance;
            }
            float normalizedDistanceToGround = Mathf.Clamp01(distanceToGround / 2f);
            float intensity = 1.0f - normalizedDistanceToGround;
            if (materialToModify != null)
            {
                materialToModify.SetFloat("_Intensity", intensity); // Assuming "_Intensity" is the property name in your material shader
            }
            if (blurMaterial != null)
            {
                blurMaterial.SetFloat("_BlurSize", intensity); // Assuming "_Intensity" is the property name in your material shader
            }
        } else {
            materialToModify.SetFloat("_Intensity", 1.0f);
            blurMaterial.SetFloat("_BlurSize", 0f);

        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(speedInput) > 0)
            {
                rb.AddForce(transform.forward * speedInput);
                previousSpeed = speedInput;
            }
            else
            {
                // Slow down when not accelerating
                rb.velocity *= 0.99f;
                previousSpeed = 0f;
            }
        }
        else
        {
            // Apply custom gravity when in the air
            rb.AddForce(gravityController.gravityDirection * gravityForce);
            rb.AddForce(transform.forward * speedInput * 0.5f);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f) && !gravityController.IsStickingToWall)
        {
            Vector3 groundNormal = hit.normal;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .1f);
            isGrounded = true;
        }
        else
        {
            if (!gravityController.IsStickingToWall && !gravityController.IsInsideGravityZone)
            {
                isGrounded = false;
                // Reset rotation around the X-axis when in the air
                if (!player1.getHere){
                    Vector3 newRotation = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * 5f);
                }
            } else if (gravityController.IsStickingToWall)
            {
                isGrounded = true;
                RotateTowardsWall(transform.forward);
                RotateTowardsWall(-transform.forward);
                RotateTowardsWall(-transform.right);
                RotateTowardsWall(transform.right);
                RotateTowardsWall(transform.up);
                RotateTowardsWall(-transform.up);
            }
        }
    }

    void RotateTowardsWall(Vector3 direction)
    {
        RaycastHit wallHit;

        if (Physics.Raycast(transform.position, direction, out wallHit, raycastDistance))
        {
            Vector3 wallNormal = wallHit.normal;
            if (Vector3.Distance(wallNormal, Vector3.forward) < threshold)
            {
                counter = 0f;
            }
            else if (Vector3.Distance(wallNormal, Vector3.back) < threshold)
            {
                counter = 1f;
            }
            else if (Vector3.Distance(wallNormal, Vector3.left) < threshold)
            {
                counter = 2f;
            }
            else if (Vector3.Distance(wallNormal, Vector3.right) < threshold)
            {
                counter = 3f;
            }
            else if (Vector3.Distance(wallNormal, Vector3.down) < threshold)
            {
                counter = 4f;
            }
            else if (Vector3.Distance(wallNormal, Vector3.up) < threshold)
            {
                counter = 5f;
            }
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, wallNormal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

    private void boost()
    {
        boostTime -= Time.deltaTime;
        if(boostTime > 0)
        {
            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        } else
        {
            maxSpeed = 50f;
        }
    }

    private void drift()
    {
        if (Input.GetKeyDown(KeyCode.V) && isGrounded) 
        {
            rb.AddForce(-gravityController.gravityDirection * driftHopForce, ForceMode.Impulse);
            gravityController.GravityStrength = 9.81f;
            if(turnInput > 0)
            {
                right = true;
                left = false;
            }else if (turnInput < 0)
            {
                left = true;
                right = false;
            }
        }
        if (Input.GetKey(KeyCode.V) && isGrounded && (turnInput > 0 || turnInput < 0) && speedInput > 4 )
        {
            driftTime += Time.deltaTime;
            float sidewaysForce = turnInput * driftSteerForce;
            rb.AddForce(transform.right * sidewaysForce, ForceMode.Acceleration);
        }

        if (!Input.GetKey(KeyCode.V) || speedInput < 4)
        {
            left = false;
            right = false;
            if (driftTime > 1.5 && driftTime < 4)
            {
                boostTime = .75f;
            }
            if (driftTime >= 4 && driftTime < 7)
            {
                boostTime = 1.5f;
            }
            if (driftTime >= 7)
            {
                boostTime = 2.5f;
            }
            driftTime = 0f;
        }
    }

    private void move()
    {
        if (isGrounded)
        {
            speedInput = 0f;
            totalRotation = 0f;
            if (Input.GetAxis("Vertical") > 0)
            {
                speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(-gravityController.gravityDirection * 2000f, ForceMode.Impulse);
                //gravityController.GravityStrength = 9.81f;
                isGrounded = false; // Prevent consecutive jumps until grounded again
            }
        }
        else // if not grounded
        {
            Quaternion targetRotation = transform.rotation; // Start with the current rotation

            if (Input.GetKey(KeyCode.W))
            {
                targetRotation *= Quaternion.Euler(Vector3.right * 1800f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                targetRotation *= Quaternion.Euler(Vector3.left * 1800f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                targetRotation *= Quaternion.Euler(Vector3.down * 1800f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                targetRotation *= Quaternion.Euler(Vector3.up * 1800f * Time.deltaTime);
            }
            // Now smoothly interpolate between the current and target rotations
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);

            speedInput = Input.GetAxis("Vertical") * forwardAccel * 200f;
        }
        transform.position = rb.transform.position;
    }
    
    private void steer()
    {
        turnInput = Input.GetAxis("Horizontal");
        if (left && !right)
        {
            turnInput = Input.GetAxis("Horizontal") < 0 ? -1f : -.25f;
            transform.Rotate(new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f), Space.Self);
        } else if (!left &&  right)
        {
            turnInput = Input.GetAxis("Horizontal") > 0 ? 1f : .25f;
            transform.Rotate(new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f), Space.Self);
        } else 
        {
            transform.Rotate(new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f), Space.Self);
        }
    }

    public float getCounter
    {
        get { return counter; }
    }

    public bool getGrounded 
    {
        get {return isGrounded; }
    }
}