using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public SphereController sphereController;
    public TextMeshProUGUI speedText;

    public ToggleSpeed toggleSettings;

    

    void Update()
    {
        if (sphereController != null && speedText != null && toggleSettings != null && toggleSettings.GetSpeed)
        {
            // Get the character's forward velocity component
            Vector3 forwardVelocity = Vector3.Project(sphereController.rb.velocity, sphereController.transform.forward);

            // Calculate the magnitude of the forward velocity
            float currentSpeed = forwardVelocity.magnitude;

            // Calculate normalized speed
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / sphereController.maxSpeed);

            // Convert the speed to a formatted string and update the TextMeshPro text
            string speedString = string.Format("Speed: {0:0.00}", currentSpeed * 2.23694f);
            speedText.text = speedString;
        }
    }
}