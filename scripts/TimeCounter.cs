using System.Collections;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    private float startTime;
    public bool raceStarted = false;

    private void Start()
    {
        // Initialize the start time when the race begins
        startTime = Time.time;
    }

    private void Update()
    {
        if (raceStarted)
        {
            // Calculate the time elapsed since the race began
            float currentTime = Time.time - startTime;

            // Calculate minutes, seconds, and milliseconds
            int minutes = (int)(currentTime / 60);
            int seconds = (int)(currentTime % 60);
            int milliseconds = (int)((currentTime * 1000) % 1000);

            // Update the TextMeshPro text with the formatted time
            textMeshPro.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
    }

    public void StartRace()
    {
        raceStarted = true;
        startTime = Time.time;
    }

    public void EndRace()
    {
        raceStarted = false;
    }
}