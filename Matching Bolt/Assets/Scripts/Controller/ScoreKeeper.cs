using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ScoreKeeper : MonoBehaviour
{
    public int maximumHighScoreRows;

    private int score;
    private HighScore highScore;
    private Score currentScore;
    string path;

    private void Awake()
    {
        score = 0;
        highScore = new HighScore();
        currentScore = new Score("Daniel", 0);
        path = "Assets/Text/Highscore.txt";
        LoadHighScoreText();
    }
    
    void Update()
    {
        GetComponent<TextHandler>().SetScoreText(score);
        currentScore.SetPoints(score);
    }

    private void LoadHighScoreText()
    {
        StreamReader reader = new StreamReader(path);

        
    }

    public void SaveHighscoreText()
    {
        GetHighScore(out List<string> names, out List<float> points, out List<System.DateTime> dates);

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write("");
        writer.Close();
        writer = new StreamWriter(path, true);

        for (int i = 0; i < names.Count; i++)
        {
            writer.WriteLine(names[i]);
            writer.WriteLine(points[i]);
        }

        writer.Close();
        AssetDatabase.ImportAsset(path);
    }

    public int GetScore()
    {
        return score;
    }

    public void AddPoints(int points)
    {
        score += points;
    }

    public bool GetHighScore(out List<string> names, out List<float> points, out List<System.DateTime> dates)
    {
        names = new List<string>();
        points = new List<float>();
        dates = new List<System.DateTime>();

        // TEMPORARY
        highScore.RemoveScore(currentScore);
        highScore.AddScore(currentScore);

        int index = highScore.GetScoreListSize();

        if (index > maximumHighScoreRows)
        {
            index = maximumHighScoreRows;
        }
        else if (index == 0)
        {
            return false;
        }

        List<Score> tempList = highScore.GetScoreList();

        for (int i = 0; i < index; i++)
        {
            names.Add(tempList[i].GetName());
            points.Add(tempList[i].GetPoints());
            dates.Add(tempList[i].GetDate());
        }

        return true;
    }

    private class HighScore
    {
        private List<Score> scoreList;

        public HighScore()
        {
            scoreList = new List<Score>();
        }

        private void SortList()
        {
            bool endLoop = false;
            int p = 0;

            while (endLoop == false)
            {
                if (p >= 100)
                {
                    endLoop = true;
                }
                else
                {
                    p++;
                }

                bool sorted = true;

                for (int i = 0; i < scoreList.Count; i++)
                {
                    if (i + 1 < scoreList.Count)
                    {
                        if (scoreList[i + 1].GetPoints() > scoreList[i].GetPoints())
                        {
                            sorted = false;

                            Score tempScore = scoreList[i];
                            scoreList[i] = scoreList[i + 1];
                            scoreList[i + 1] = tempScore;
                        }
                    }
                }

                if (sorted == true)
                {
                    endLoop = true;
                }

                //Debug.Log("Iterations: " + p.ToString());
            }
        }

        public void AddScore(Score newScore)
        {
            scoreList.Add(newScore);
            SortList();
        }

        // TEMPORARY
        public void RemoveScore(Score oldScore)
        {
            scoreList.Remove(oldScore);
        }

        public int GetScoreListSize()
        {
            return scoreList.Count;
        }

        private Score GetScore(int index)
        {
            return scoreList[index];
        }

        public List<Score> GetScoreList()
        {
            return scoreList;
        }
    };

    private class Score
    {
        private string name;
        private float points;
        private System.DateTime date;

        public Score(string scoreName, float scorePoints)
        {
            name = scoreName;
            points = scorePoints;
            date = System.DateTime.Now;
        }

        public void SetPoints(float newPoints)
        {
            points = newPoints;
        }

        public string GetName()
        {
            return name;
        }

        public float GetPoints()
        {
            return points;
        }

        public System.DateTime GetDate()
        {
            return date;
        }
    };
}
