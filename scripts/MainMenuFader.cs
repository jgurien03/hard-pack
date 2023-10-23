using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuFader : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI buttonText;
    public Button playButton;
    public Button quitButton;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(FadeInAllElements());
    }

    private IEnumerator FadeInAllElements()
    {
        // Define the initial alpha value for all elements
        float initialAlpha = 0f;

        // Set the initial alpha for all elements
        titleText.alpha = initialAlpha;
        subtitleText.alpha = initialAlpha;
        buttonText.alpha = initialAlpha;
        playButton.image.color = new Color(1f, 1f, 1f, initialAlpha);
        playButton.interactable = false;
        quitButton.image.color = new Color(1f, 1f, 1f, initialAlpha);
        quitButton.interactable = false;

        // Fade in all elements simultaneously
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;

            titleText.alpha = Mathf.Lerp(initialAlpha, 1f, normalizedTime);
            subtitleText.alpha = Mathf.Lerp(initialAlpha, 1f, normalizedTime);
            buttonText.alpha = Mathf.Lerp(initialAlpha, 1f, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that all elements are fully visible at the end
        titleText.alpha = 1f;
        subtitleText.alpha = 1f;
        buttonText.alpha = 1f;
        playButton.interactable = true;
        quitButton.interactable = true;
    }
}