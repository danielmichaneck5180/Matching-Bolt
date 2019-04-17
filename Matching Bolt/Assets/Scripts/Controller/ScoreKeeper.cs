using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoreText;

    private int score;
    
    void Awake()
    {
        score = 0;
    }
    
    void Update()
    {
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int points)
    {
        score += points;
    }
}
