using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class SaveButton : MonoBehaviour
{
    // Reference to the Save button
    public Button saveButton;

    // Add any other UI elements that you want to reset here
    public Image[] uiElementsToReset;
    public TextMeshProUGUI[] tmpTextElements;

    private List<ButtonControl> buttonControls = new List<ButtonControl>();
    private List<Graphic> graphicsToFade = new List<Graphic>();

    public bool isSaved = false;
    public RawImage canvasRawImage;
    public Texture newTexture;

    private void Start()
    {
        // Attach a click event listener to the Save button
        saveButton.onClick.AddListener(OnSaveButtonClick);

        // Find all ButtonControl scripts in the children of this object
        ButtonControl[] childButtonControls = GetComponentsInChildren<ButtonControl>(true);
        buttonControls.AddRange(childButtonControls);

        // Find all Graphic components (including Images and TMP elements) in the children of this object
        Graphic[] childGraphics = GetComponentsInChildren<Graphic>(true);
        graphicsToFade.AddRange(childGraphics);
    }

    // Called when the Save button is clicked
    public void OnSaveButtonClick()
    {
        // Save the game data or perform any necessary operations here

        // Reset UI elements and hovering properties for all ButtonControl scripts
        foreach (ButtonControl buttonControl in buttonControls)
        {
            buttonControl.button1.gameObject.SetActive(true);
            buttonControl.button2.gameObject.SetActive(false);
            buttonControl.button1.enabled = true;
            buttonControl.check.SetHoverEnabled(true);
            // Reset the UI elements' colors
            //buttonControl.testImage.GetComponent<Image>().color = Color.white;
        }

        // Fade out all Graphic components (including TMP elements) in the children of this object
        foreach (Graphic graphic in graphicsToFade)
        {
            StartCoroutine(FadeOutGraphic(graphic, .5f)); // You can adjust the duration as needed
        }

        foreach (Image uiElement in uiElementsToReset)
        {
            // Reset the UI element to its default state
            // You may need to customize this part based on your specific UI setup
            // For example, you can reset colors, text, or other properties here.
        }
        if (canvasRawImage != null && newTexture != null)
        {
            canvasRawImage.texture = newTexture;
        }
        isSaved = true;
    }

    // Coroutine to fade out a Graphic component (including TMP elements)
    private IEnumerator FadeOutGraphic(Graphic graphic, float duration)
    {
        float startAlpha = graphic.color.a;
        float startTime = Time.unscaledTime;

        while (Time.unscaledTime - startTime < duration)
        {
            float t = (Time.unscaledTime - startTime) / duration;
            Color newColor = graphic.color;
            newColor.a = Mathf.Lerp(startAlpha, 0f, t);
            graphic.color = newColor;
            yield return null;
        }

        Color finalColor = graphic.color;
        finalColor.a = 0f; // Ensure it's fully transparent
        graphic.color = finalColor;
    }

    public bool GetSaved
    {
        get { return isSaved; }
    }

    public void SetSaved(bool value)
    {
        isSaved = value;
    }
}
