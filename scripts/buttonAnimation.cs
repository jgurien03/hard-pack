using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Color originalColor;
    private Vector3 originalScale;

    public Color hoverColor = Color.green;
    public float hoverScaleMultiplier = 1.2f;

    private void Start()
    {
        // Get a reference to the Button component
        button = GetComponent<Button>();

        // Store the original color and scale of the button
        originalColor = button.image.color;
        originalScale = transform.localScale;
    }

    // Called when the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the button color to the hoverColor
        button.image.color = hoverColor;

        // Scale up the button
        transform.localScale = originalScale * hoverScaleMultiplier;
    }

    // Called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the original button color
        button.image.color = originalColor;

        // Restore the original button scale
        transform.localScale = originalScale;
    }
}
