using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button textMeshProButton;
    public Image childImage;

    public float fadeDuration = 0.2f; // Duration of the fade-in and fade-out animations
    private float currentAlpha = 0f; // Current alpha value of the child Image

    private bool hoverEnabled = true; // Flag to enable/disable the hover effect
    public PauseMenu resume;

    private void Awake()
    {
        // Initialize references to the Button and child Image components
        textMeshProButton = GetComponent<Button>();
        childImage = transform.GetChild(0).GetComponent<Image>();

        // Set the initial alpha of the child Image to 0 (fully transparent)
        SetChildImageAlpha(0f);
    }

    private void Update()
    {
        // Check if the mouse pointer is over the button both horizontally and vertically
        if (hoverEnabled && IsPointerOverButton())
        {
            currentAlpha = Mathf.Lerp(currentAlpha, 1f, Time.unscaledDeltaTime / fadeDuration);
        }
        else
        {
            currentAlpha = Mathf.Lerp(currentAlpha, 0f, Time.unscaledDeltaTime / fadeDuration);
        }

        // Set the alpha value of the child Image

        SetChildImageAlpha(currentAlpha);
    }

    // Called when the mouse pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        // No need to do anything specific here since the check is done in Update
    }

    // Called when the mouse pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        // No need to do anything specific here since the check is done in Update
    }

    // Function to set the alpha value of the child Image
    private void SetChildImageAlpha(float alpha)
    {
        Color imageColor = childImage.color;
        imageColor.a = alpha;
        childImage.color = imageColor;
    }

    // Check if the mouse pointer is over the button both horizontally and vertically
    private bool IsPointerOverButton()
    {
        RectTransform buttonRect = textMeshProButton.GetComponent<RectTransform>();
        Vector2 mousePosition = Input.mousePosition;

        // Check if the mouse position is within the button's rect
        return buttonRect.rect.Contains(buttonRect.InverseTransformPoint(mousePosition));
    }

    // Enable or disable the hover effect
    public void SetHoverEnabled(bool enabled)
    {
        hoverEnabled = enabled;
    }

    public float GetAlpha
    {
        get { return currentAlpha; }
    }

    public void SetAlpha(float value)
    {
        currentAlpha = value;
    }
}