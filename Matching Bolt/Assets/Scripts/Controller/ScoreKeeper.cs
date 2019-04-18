using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoreText;

    private int score;
    private HighScore highScore;
    
    void Awake()
    {
        score = 0;
        highScore = new HighScore();
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

    private class HighScore
    {
        private List<Score> scoreList;

        private void SortList()
        {
            scoreList.Sort();
        }

        public void AddScore(Score newScore)
        {
            scoreList.Add(newScore);
            SortList();
        }

        public int GetScoreListSize()
        {
            return scoreList.Count;
        }

        public Score GetScore(int index)
        {
            return scoreList[index];
        }
    };

    private class Score
    {
        private readonly string name;
        private readonly float points;
        private readonly System.DateTime date;

        Score(string scoreName, float scorePoints)
        {
            name = scoreName;
            points = scorePoints;
            date = System.DateTime.Now;
        }
    };
}
