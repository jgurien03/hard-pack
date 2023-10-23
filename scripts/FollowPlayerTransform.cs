using UnityEngine;

public class FollowPlayerPosition : MonoBehaviour
{
    private Quaternion lastGoodRotation;  // Store the last good rotation here
    public Transform playerTransform;
    public SphereController sphere;

    public float smoothSpeed = 0.125f; // The higher the value, the faster the rotation will be

    private bool here = false;

    void LateUpdate()
    {
        // Check if W, A, S, or D is being pressed
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // Keep the camera at the last known good rotation
            if (sphere.getGrounded == false)
            {
                transform.rotation = lastGoodRotation;
                here = true;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerTransform.rotation, smoothSpeed);
                lastGoodRotation = playerTransform.rotation;
                here = false;
            }
        }
        else
        {
            // Smoothly update the camera to match the player's rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, playerTransform.rotation, smoothSpeed);

            // Save this as the last known good rotation
            lastGoodRotation = playerTransform.rotation;
            here = false;
        }
    }

    public bool getHere 
    {
        get {return here; }
    }
}
