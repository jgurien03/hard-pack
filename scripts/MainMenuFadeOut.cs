using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuFadeOut : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI buttonText;
    public Button playButton;
    public Button quitButton;
    public Image backgroundImage; // Reference to the background image
    public Image blackImage; // Reference to the black image to fade in
    public AudioSource backgroundAudio; // Reference to the background audio source
    public AudioClip buttonClickSound; // Button click sound clip
    public float fadeDuration = 1.0f;
    public float blackImageMoveSpeed = 50f; // Speed at which the black image moves (adjust as needed)

    private bool isFadingOut = false;

    public void OnFadeOutButtonClick()
    {
        if (!isFadingOut)
        {
            backgroundAudio.PlayOneShot(buttonClickSound);
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        isFadingOut = true;

        // Get the initial alpha value for all elements and the black image
        float initialAlpha = 1f;
        float blackImageAlpha = 0f;
        float initialAudioVolume = backgroundAudio.volume;

        // Save the initial position of the black image
        Vector3 initialBlackImagePosition = blackImage.rectTransform.position;

        // Fade out all elements and audio simultaneously
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;

            titleText.alpha = Mathf.Lerp(initialAlpha, 0f, normalizedTime);
            subtitleText.alpha = Mathf.Lerp(initialAlpha, 0f, normalizedTime);
            buttonText.alpha = Mathf.Lerp(initialAlpha, 0f, normalizedTime);

            // Fade out buttons' images by setting their color alpha
            playButton.image.color = new Color(1f, 1f, 1f, Mathf.Lerp(initialAlpha, 0f, normalizedTime));
            quitButton.image.color = new Color(1f, 1f, 1f, Mathf.Lerp(initialAlpha, 0f, normalizedTime));

            // Move the black image upwards (adjust direction as needed)
            blackImage.rectTransform.position += Vector3.up * blackImageMoveSpeed * Time.deltaTime;

            // Fade in the black image by increasing its alpha
            blackImageAlpha = Mathf.Lerp(0f, 1f, normalizedTime);
            blackImage.color = new Color(0f, 0f, 0f, blackImageAlpha);

            // Fade out the audio by decreasing its volume
            backgroundAudio.volume = Mathf.Lerp(initialAudioVolume, 0f, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that all elements and the audio are fully faded out at the end
        titleText.alpha = 0f;
        subtitleText.alpha = 0f;
        buttonText.alpha = 0f;
        playButton.interactable = false;
        quitButton.interactable = false;
        backgroundImage.color = new Color(1f, 1f, 1f, 0f);
        blackImageAlpha = 1f;
        blackImage.color = new Color(0f, 0f, 0f, blackImageAlpha);
        backgroundAudio.volume = 0f;

        // Reset the black image position to its initial position
        blackImage.rectTransform.position = initialBlackImagePosition;

        // Load the new scene after fading is complete
        SceneManager.LoadScene("game");

        isFadingOut = false;
    }
}