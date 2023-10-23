using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resume1 : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public PauseMenu menu;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(ResumeGame);
        button2.onClick.AddListener(QuitGame);
        button3.onClick.AddListener(ReturnToMainMenu);
    }

    // Update is called once per frame
    void ResumeGame()
    {
        menu.ResumeGame();
    }

    void QuitGame()
    {
        menu.QuitGame();
    }

    void ReturnToMainMenu()
    {
        menu.ReturnToMainMenu();
    }
}
