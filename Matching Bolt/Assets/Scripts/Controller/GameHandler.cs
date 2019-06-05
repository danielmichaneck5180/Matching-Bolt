using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private float difficultyMultiplier;
    private bool gameRunning;
    private bool gamePaused;

    void Awake()
    {
        gameRunning = true;
        Time.timeScale = 1;
        difficultyMultiplier = 1;
        Cursor.visible = false;
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().RefreshHighscore();
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
            SceneManager.LoadScene("Main Scene");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<TextHandler>().DisplayHighScore();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (gamePaused == true)
            {
                gamePaused = false;
            }
            else
            {
                gamePaused = true;
            }
        }
        if (gamePaused == true)
        {
            if (GameObject.FindGameObjectWithTag("Wiimote Controller").GetComponent<WiimoteScript>().WiimotesEnabled() == true)
            {
                if (GameObject.FindGameObjectWithTag("Wiimote Controller").GetComponent<WiimoteScript>().GetBButtonDown() == true)
                {
                    gamePaused = false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                gamePaused = false;
            }
        }
    }

    private void UpdateDifficulty()
    {
        difficultyMultiplier += Time.deltaTime / 45;
    }

    public float GetDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Input Name Scene");
    }

    public void EndGame()
    {
        gameRunning = false;
        GetComponent<TextHandler>().DisplayHighScore();
        Application.Quit();
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }

    public void SetGamePaused(bool b)
    {
        gamePaused = b;
    }
}
