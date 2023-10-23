using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewportToggle : MonoBehaviour
{
    private bool isFullscreen = true; // Tracks fullscreen/windowed status
    public TextMeshProUGUI statusText;
    public Button button;

    private void Start()
    {
        // Update the initial status text
        UpdateStatusText();

        if (button != null)
        {
            button.onClick.AddListener(ToggleFullscreen);
        }
    }

    private void ToggleFullscreen()
    {
        // Toggle between fullscreen and windowed modes
        isFullscreen = !isFullscreen;

        // Set the application's fullscreen/windowed mode
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        // Update the status text
        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        // Update the TextMeshPro text to reflect the current mode
        if (statusText != null)
        {
            statusText.text = isFullscreen ? "fullscreen" : "windowed";
        }
    }
}