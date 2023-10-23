using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleFPSSettings : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI toggleButtonText;
    private bool isFPSVisible = true;
    public Button button;

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(ToggleFPSVisibility);
        }
    }

    public void ToggleFPSVisibility()
    {
        // Toggle FPS visibility and update the button text
        isFPSVisible = !isFPSVisible;
        UpdateToggleButtonText();

        // Clear the FPS text when it's turned off
        if (!isFPSVisible)
        {
            fpsText.text = "";
        }
    }

    private void UpdateToggleButtonText()
    {
        // Update the button text to reflect the current state
        toggleButtonText.text = isFPSVisible ? "on" : "off";
    }

    public bool GetFPS
    {
        get { return isFPSVisible; }
    }
}