using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ScoreKeeper : MonoBehaviour
{
    private int maximumHighScoreRows;

    private int score;
    private HighScore highScore;
    private Score currentScore;
    private string path;
    private TextAsset scoreFile;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Score Keeper").Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        scoreFile = Resources.Load("Highscore") as TextAsset;
        //path = AssetDatabase.GetAssetPath(scoreFile);
        Debug.Log(Application.persistentDataPath);
        path = Application.persistentDataPath + "/Highscore.txt";
        maximumHighScoreRows = 10;
        RefreshHighscore();
    }
    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Controller") != null)
        {
            if (GameObject.FindGameObjectWithTag("Controller").GetComponent<TextHandler>() != null)
            {
                GameObject.FindGameObjectWithTag("Controller").GetComponent<TextHandler>().SetScoreText(score);
                currentScore.SetPoints(score);
            }
        }
    }

    private void LoadHighScoreText()
    {
        StreamReader reader = new StreamReader(path);
        List<string> highScoreString = new List<string>();
        List<string> highScorePoints = new List<string>();

        while(reader.EndOfStream == false)
        {
            highScoreString.Add(reader.ReadLine());
            highScorePoints.Add(reader.ReadLine());
        }

        reader.Close();
        for (int i = 0; i < highScoreString.Count; i++)
        {
            highScore.AddScore(new Score(highScoreString[i], float.Parse(highScorePoints[i])));
        }
    }

    public void RefreshHighscore()
    {
        score = 0;
        highScore = new HighScore();
        currentScore = new Score("No name", 0);
        LoadHighScoreText();
    }

    public void SaveHighscoreText()
    {
        highScore.RemoveScore(currentScore);
        highScore.AddScore(currentScore);

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
        //AssetDatabase.ImportAsset(path);
    }

    public int GetScore()
    {
        return score;
    }

    public void AddPoints(int points)
    {
        score += points;

        if (score < 0)
        {
            score = 0;
        }
    }

    public void SetPlayerName(string n)
    {
        currentScore.SetName(n);
    }

    public int GetMaximumHighscoreRows()
    {
        return maximumHighScoreRows;
    }

    public bool GetHighScore(out List<string> names, out List<float> points, out List<System.DateTime> dates)
    {
        names = new List<string>();
        points = new List<float>();
        dates = new List<System.DateTime>();

        int index = highScore.GetScoreListSize();

        if (index < 1)
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

    public void GetHighScoreToDisplay(out List<string> names, out List<float> points)
    {
        names = new List<string>();
        points = new List<float>();

        GetHighScore(out names, out points, out List<System.DateTime> dates);

        if (names.Count > maximumHighScoreRows)
        {
            Debug.Log("Max: " + maximumHighScoreRows + " Count: " + names.Count);
            while (names.Count > maximumHighScoreRows)
            {
                names.RemoveAt(maximumHighScoreRows);
                points.RemoveAt(maximumHighScoreRows);
            }
        }
    }

    public void GetHighScoreToDisplayCurrent(out string name, out float points, out int place)
    {
        name = currentScore.GetName();
        points = currentScore.GetPoints();
        place = highScore.GetScoreListSize();

        for (int i = 0; i < highScore.GetScoreListSize(); i++)
        {
            if (highScore.GetScoreList()[i] == currentScore)
            {
                place = i + 1;
                i = highScore.GetScoreListSize();
            }
        }
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

        public void SetName(string newName)
        {
            name = newName;
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
