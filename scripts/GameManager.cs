using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float currentVolume = 0.5f; // Default volume level
    public AudioClipChanger audioClipChanger; // Reference to the AudioClipChanger script

    private void Awake()
    {
        // Load the last saved volume from PlayerPrefs
        currentVolume = PlayerPrefs.GetFloat("SavedVolume", currentVolume);
        // Set the audio source volume to the last saved volume
        audioSource.volume = currentVolume;
    }

    public void AdjustVolume(float newVolume)
    {
        currentVolume = newVolume;
        audioSource.volume = currentVolume;
        PlayerPrefs.SetFloat("SavedVolume", currentVolume);
        PlayerPrefs.Save(); // Save the PlayerPrefs data
    }
}