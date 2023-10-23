using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioClipChanger : MonoBehaviour
{
    public AudioSource mainAudioSource; // Drag and drop your AudioSource here in the Inspector
    public List<ButtonAudioPair> buttonAudioPairs = new List<ButtonAudioPair>();

    private void Start()
    {
        // Make sure the mainAudioSource is set properly in the Inspector
        if (mainAudioSource == null)
        {
            Debug.LogError("Please assign the mainAudioSource in the Inspector.");
            enabled = false;
            return;
        }

        foreach (var pair in buttonAudioPairs)
        {
            // Attach click events to the buttons
            pair.button.onClick.AddListener(() => ChangeAudioClip(pair.audioClip));
        }
    }

    private void ChangeAudioClip(AudioClip newAudioClip)
    {
        // Change the audio clip of the mainAudioSource
        mainAudioSource.clip = newAudioClip;
        mainAudioSource.Play();
    }
}