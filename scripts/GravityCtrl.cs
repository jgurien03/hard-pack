using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCtrl : MonoBehaviour
{
    private Rigidbody rb;
    private bool isInsideGravityZone = false;
    public Vector3 gravityDirection = Vector3.down; // Default gravity direction
    private Quaternion targetRotation; // New rotation to align with surface normal
    public GravityOrbit Gravity;
    public float GravityStrength = 9.81f; // Adjust this as needed
    private bool isStickingToWall = false;
    public SphereController orientation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on this object or its children.");
        }
    }

    private void FixedUpdate()
    {
        if (isInsideGravityZone)
        {
            ApplyCustomGravity();
        }
    }

    private void ApplyCustomGravity()
    {
        // Apply the custom gravity to the rigidbody
        rb.AddForce(gravityDirection * GravityStrength, ForceMode.Acceleration);
    }

    private void Update()
    {
        if (isInsideGravityZone)
        {
            UpdateGravityDirection();
        }
    }

    private void UpdateGravityDirection()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Vector3 hitNormal = hit.normal;

            if (Vector3.Dot(hitNormal, Vector3.up) < 0.85f)
            {
                // We're on a normal slope
                gravityDirection = -hitNormal;
                isStickingToWall = false;
            }
            else
            {
                // We're on a wall
                if (orientation.getCounter == 0f)
                {
                    gravityDirection = Vector3.back;
                } 
                else if (orientation.getCounter == 1f)
                {
                    gravityDirection = Vector3.forward;
                }
                else if (orientation.getCounter == 2f)
                {
                    gravityDirection = Vector3.right;
                }
                else if (orientation.getCounter == 3f)
                {
                    gravityDirection = Vector3.left;
                }
                else if (orientation.getCounter == 4f)
                {
                    gravityDirection = Vector3.up;
                }
                else if (orientation.getCounter == 5f)
                {
                    gravityDirection = Vector3.down;
                }
                
                isStickingToWall = true;
            }
        }
        else
        {
            // Default to regular gravity if no collision detected
            gravityDirection = Vector3.down;
            isStickingToWall = false;
        }
    }

    public bool IsStickingToWall
    {
        get { return isStickingToWall; }
    }

    public bool IsInsideGravityZone
    {
        get { return isInsideGravityZone; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            isInsideGravityZone = true;
            rb.useGravity = false;

            Debug.Log("Entered GravityZone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            isInsideGravityZone = false;
            rb.useGravity = true;
            gravityDirection = Vector3.down; // Reset gravity direction when exiting the zone
            isStickingToWall = false;
        }
    }
}