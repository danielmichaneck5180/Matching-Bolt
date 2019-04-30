using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    private bool gameRunning;

    void Awake()
    {
        gameRunning = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) == true)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        gameRunning = false;
        GetComponent<TextHandler>().DisplayHighScore();
    }
}
