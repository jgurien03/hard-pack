using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Button showButton;           // Button to make the hidden button appear
    public Button hiddenButton;         // The button to be hidden initially
    public float lerpDuration = 1.0f;   // Duration of the lerp animation

    private bool isLerping = false;
    private float lerpStartTime;
    private Color initialColor; // Store the initial color of the hidden button
    public TextMeshProUGUI[] textMeshPros; // Array to store child TextMeshPro objects
    public RawImage canvasRawImage;
    public Texture newTexture;
    public ButtonHoverManager hover;

    public SaveButton checkSave;

    public Button checkButton;

    void Start()
    {
        // Hide the initially hidden button
        hiddenButton.gameObject.SetActive(false);

        // Store the initial color of the hidden button
        initialColor = hiddenButton.GetComponent<Image>().color;

        // Populate the textMeshPros array with child TextMeshPro objects
        textMeshPros = hiddenButton.GetComponentsInChildren<TextMeshProUGUI>();

        // Attach click event handler to showButton
        showButton.onClick.AddListener(StartLerpHiddenButton);
    }

    void Update()
    {
        if (isLerping)
        {
            float timeSinceStart = Time.unscaledTime - lerpStartTime;
            float lerpProgress = Mathf.Clamp01(timeSinceStart / lerpDuration);

            // Calculate the new alpha value using Lerp
            float newAlpha = Mathf.Lerp(0f, 1f, lerpProgress);

            // Update the color with the new alpha value for the button
            Color lerpedColor = hiddenButton.GetComponent<Image>().color;
            lerpedColor.a = newAlpha;
            hiddenButton.GetComponent<Image>().color = lerpedColor;

            // Update the alpha value for each child TextMeshPro object
            foreach (var textMeshPro in textMeshPros)
            {
                Color textMeshColor = textMeshPro.color;
                textMeshColor.a = newAlpha;
                textMeshPro.color = textMeshColor;
            }

            // Update the alpha value for each child TextMeshPro button (if they exist)
            var textMeshProButtons = hiddenButton.GetComponentsInChildren<Button>(); // Adjust this to your actual TextMeshPro button component type
            foreach (var textMeshProButton in textMeshProButtons)
            {
                if (textMeshProButton.GetComponent<Image>()){
                    Color buttonColor = textMeshProButton.GetComponent<Image>().color;
                    buttonColor.a = newAlpha;
                    textMeshProButton.GetComponent<Image>().color = buttonColor;
                }
            }

            if (lerpProgress >= 1.0f)
            {
                isLerping = false;
            }
        }
        if (checkSave.GetSaved)
        {
            hover.SetHoverEnabledForAll(true);
            var allButtons = FindObjectsOfType<Button>();
            foreach (var button in allButtons)
            {
                button.interactable = true;
            }
            checkButton.interactable = false;

            // Enable interaction for the children of the hidden button
            var childButtons = hiddenButton.GetComponentsInChildren<Button>();
            foreach (var childButton in childButtons)
            {
                childButton.interactable = false;
            }
            checkSave.SetSaved(false);
            if (!checkSave.GetSaved)
            {
                hiddenButton.gameObject.SetActive(false);
            }
        }

    }

    void StartLerpHiddenButton()
    {
        RectTransform rectTransform = showButton.GetComponent<RectTransform>();
        Vector2 mousePos = Input.mousePosition;
        bool isMouseWithinButton1Bounds = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos);
        if (isMouseWithinButton1Bounds)
        {
            // Make the hidden button and its children appear
            hiddenButton.gameObject.SetActive(true);
            hover.SetHoverEnabledForAll(false);

            ButtonHover[] childButtonHovers = hiddenButton.GetComponentsInChildren<ButtonHover>();
            foreach (ButtonHover childButtonHover in childButtonHovers)
            {
                hover.SetHoverForOne(childButtonHover, true);
            }

            // Set the alpha value to 0 before starting the lerp
            Color lerpedColor = hiddenButton.GetComponent<Image>().color;
            lerpedColor.a = 0f;
            hiddenButton.GetComponent<Image>().color = lerpedColor;

            // Reset the alpha value for each child TextMeshPro object
            foreach (var textMeshPro in textMeshPros)
            {
                Color textMeshColor = textMeshPro.color;
                textMeshColor.a = 0f;
                textMeshPro.color = textMeshColor;
            }

            // Start the lerp animation
            isLerping = true;
            lerpStartTime = Time.unscaledTime;

            // Disable interaction for all buttons in the scene
            var allButtons = FindObjectsOfType<Button>();
            foreach (var button in allButtons)
            {
                button.interactable = false;
            }

            // Enable interaction for the children of the hidden button
            var childButtons = hiddenButton.GetComponentsInChildren<Button>();
            foreach (var childButton in childButtons)
            {
                childButton.interactable = true;
            }

            if (canvasRawImage != null && newTexture != null)
            {
                // Assign the new texture to the RawImage component on the canvas
                canvasRawImage.texture = newTexture;
            }
        }
    }

    // Helper function to check if a transform is a child of the hidden button
    bool IsChildOfHiddenButton(Transform transform)
    {
        Transform currentTransform = transform;
        while (currentTransform != null)
        {
            if (currentTransform == hiddenButton.transform)
            {
                return true;
            }
            currentTransform = currentTransform.parent;
        }
        return false;
    }
}