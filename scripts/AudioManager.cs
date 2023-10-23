using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public GameObject[] volumeCubes; // An array of volume cubes (buttons)
    public ButtonControl button1;

    public GameManager gameManager;
    private float volumeIncrement = 0.1f; // Adjust the volume increment as needed

    private void Start()
    {
        for (int i = 0; i < volumeCubes.Length; i++)
        {
            int index = i; // Create a local copy of the index
            Button button = volumeCubes[index].GetComponent<Button>();
            button.onClick.AddListener(() => { AdjustVolume(index); });
            for (int j = 0; j < volumeCubes.Length; j++)
            {
                Image cubeImage = volumeCubes[j].GetComponent<Image>();
                cubeImage.fillCenter = (j * volumeIncrement < gameManager.currentVolume);
            }
        }
    }

    private void AdjustVolume(int cubeIndex)
    {
        if (button1.GetButton != true)
        {
            float newVolume = (cubeIndex + 1) * volumeIncrement;
            gameManager.AdjustVolume(newVolume);

            for (int i = 0; i < volumeCubes.Length; i++)
            {
                Image cubeImage = volumeCubes[i].GetComponent<Image>();
                cubeImage.fillCenter = (i <= cubeIndex);
            }
        }
    }
}