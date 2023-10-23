using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ResolutionType : MonoBehaviour
{
    public TextMeshProUGUI settingsText; // Reference to your TextMeshPro text element
    public Button toggleButton; // Reference to your toggle button
    private bool isLegacy = true; // Initial setting is "legacy"
    private UniversalAdditionalCameraData _camData;
    public Camera YourCamera;
    public ButtonControl button1;
    void Start()
    {
        toggleButton.interactable = true;
        toggleButton.onClick.AddListener(ToggleSetting);
        _camData = YourCamera.GetComponent<UniversalAdditionalCameraData>();
    }

    // Update is called once per frame
    public void ToggleSetting()
    {
        isLegacy = !isLegacy; // Toggle the setting

        // Update the TextMeshPro text to display the current setting
        settingsText.text = isLegacy ? "legacy" : "modern";

        // Call a method to enable/disable your render feature based on the setting
        HandleRenderFeature();
    }

    private void HandleRenderFeature()
    {
        if (_camData == null)
        {
            Debug.LogError("Camera data is not assigned.");
            return;
        }

        if (isLegacy)
        {
            // Switch to a renderer without the render feature.
            _camData.SetRenderer(0);
        }
        else
        {
            // Switch to a renderer with the render feature.
            _camData.SetRenderer(1);
        }
    }
}
