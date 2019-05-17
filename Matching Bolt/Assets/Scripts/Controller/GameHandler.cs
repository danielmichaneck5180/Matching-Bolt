using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private float difficultyMultiplier;
    private bool gameRunning;

    void Awake()
    {
        gameRunning = true;
        Time.timeScale = 1;
        difficultyMultiplier = 1;
    }

    private void Update()
    {
        UpdateDifficulty();

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    private void UpdateDifficulty()
    {
        difficultyMultiplier += Time.deltaTime / 60;
    }

    public float GetDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }

    public void EndGame()
    {
        gameRunning = false;
        GetComponent<TextHandler>().DisplayHighScore();
        Application.Quit();
    }
}
