using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSpeed : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI toggleButtonText;
    private bool isSpeedVisible = true;
    public Button button;

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(ToggleSpeedVisibility);
        }
    }

    public void ToggleSpeedVisibility()
    {
        // Toggle FPS visibility and update the button text
        isSpeedVisible = !isSpeedVisible;
        UpdateToggleButtonText();

        // Clear the FPS text when it's turned off
        if (!isSpeedVisible)
        {
            speedText.text = "";
        }
    }

    private void UpdateToggleButtonText()
    {
        // Update the button text to reflect the current state
        toggleButtonText.text = isSpeedVisible ? "on" : "off";
    }

    public bool GetSpeed
    {
        get { return isSpeedVisible; }
    }
}