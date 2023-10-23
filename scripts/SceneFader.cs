using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public Image sceneImage;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        sceneImage.color = Color.clear;

        while (fadeImage.color.a > 0)
        {
            fadeImage.color = new Color(0f, 0f, 0f, fadeImage.color.a - Time.deltaTime / fadeDuration);
            sceneImage.color = new Color(1f, 1f, 1f, sceneImage.color.a + Time.deltaTime / fadeDuration);
            yield return null;
        }

        fadeImage.color = Color.clear;
        sceneImage.color = Color.white; // Set the final color to white or your desired color
    }
}