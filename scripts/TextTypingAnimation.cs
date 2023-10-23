using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextTypingAnimation : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    public string textToType = "Hello, world!<br>This is a typing animation.<br><br>Welcome to the game!";
    public float delayBeforeTyping = 5f; // 5 seconds delay before typing starts
    public AudioClip typingSound; // Sound effect for typing (optional)
    public float delayAfterTyping = 2f; // 2 seconds delay after typing animation
    public float fadeOutDuration = 1.5f; // Time it takes for the text to fade out
    public string nextSceneName = "NextScene"; // The name of the next scene to load

    // New variables for handling newlines and font size
    public float newlineDelay = 1.5f; // 1.5 seconds delay before newline text types
    public float smallerFontSize = 36f; // The font size to use after the newline

    private TMP_Text textMeshPro;
    private int currentCharacterIndex = 0;
    private AudioSource audioSource;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component (optional)

        StartCoroutine(StartTypingWithDelay());
    }

    private IEnumerator StartTypingWithDelay()
    {
        // Wait for the initial delay before typing starts
        yield return new WaitForSeconds(delayBeforeTyping);

        // Play the typing sound effect (optional)
        if (typingSound != null && audioSource != null)
        {
            audioSource.clip = typingSound;
            audioSource.Play();
        }

        // Start the typing animation
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        if (textMeshPro == null)
        {
            yield break;
        }

        // Replace <br> tags with actual newlines
        textToType = textToType.Replace("<br>", "\n");

        if (string.IsNullOrEmpty(textToType))
        {
            yield break;
        }

        textMeshPro.text = ""; // Clear existing text

        while (currentCharacterIndex < textToType.Length)
        {
            if (textToType[currentCharacterIndex] == '\n')
            {
                yield return new WaitForSeconds(newlineDelay);
                UpdateFontSize(); // Call the function to change font size after newline
            }

            textMeshPro.text += textToType[currentCharacterIndex];
            currentCharacterIndex++;

            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for the delay after typing animation
        yield return new WaitForSeconds(delayAfterTyping);

        // Fade out the text
        float elapsedTime = 0f;
        Color originalColor = textMeshPro.color;
        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    // Function to change font size after the newline
    private void UpdateFontSize()
    {
        // Find the newline character's index
        int newlineIndex = textMeshPro.text.IndexOf('\n');

        // Check if the newline character exists and if the smaller font size is greater than 0
        if (newlineIndex != -1 && smallerFontSize > 0f)
        {
            // Set the font size after the newline
            textMeshPro.fontSize = smallerFontSize;

            // Force an update of the text to apply the font size change
            textMeshPro.ForceMeshUpdate(true);
        }
    }
}