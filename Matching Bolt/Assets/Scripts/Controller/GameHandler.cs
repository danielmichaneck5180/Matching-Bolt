using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    private bool gameRunning;

    void Awake()
    {
        gameRunning = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            EndGame();
        }
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            SceneManager.LoadScene("PERSPECTIVE Daniel's Scene");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<TextHandler>().DisplayHighScore();
        }
    }

    public void EndGame()
    {
        gameRunning = false;
        GetComponent<TextHandler>().DisplayHighScore();
        Application.Quit();
    }
}
