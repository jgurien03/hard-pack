using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    public ButtonHover button;

    private void Start()
    {
        // Ensure the pause menu is initially hidden
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (button.GetAlpha > 0f && !isPaused)
        {
            button.SetAlpha(0f);
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
    }

    public void QuitGame()
    {
        // This function can be called when the player clicks a "Quit" button in the pause menu
        // You can add additional logic here, such as saving the game state
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        // This function can be called when the player clicks a "Return to Main Menu" button in the pause menu
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene("titleScene"); 
    }
}