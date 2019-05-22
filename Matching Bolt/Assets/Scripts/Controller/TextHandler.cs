using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text scoreText;
    public Text healthText;
    public Text highScoreTextNames;
    public Text highScoreTextPoints;
    public Text highScoreTextDates;

    void Start()
    {
        ResetText();
    }

    void Update()
    {
        SetHealthText();
    }

    private void SetHealthText()
    {
        int newText = GameObject.FindGameObjectWithTag("Controller").GetComponent<HealthScript>().GetHealth();
        if (newText < 0)
        {
            healthText.text = "0";
        }
        else
        {
            healthText.text = newText.ToString();
        }
    }

    public void SetScoreText(float score)
    {
        scoreText.text = score.ToString();
    }

    public void DisplayHighScore()
    {
        List<string> namesList = new List<string>();
        List<float> pointsList = new List<float>();
        List<System.DateTime> datesList = new List<System.DateTime>();

        GetComponent<ScoreKeeper>().GetHighScore(out namesList, out pointsList, out datesList);

        if (namesList.Count != pointsList.Count || namesList.Count != datesList.Count || pointsList.Count != datesList.Count)
        {
            Debug.Log("CRITICAL ERROR: DIFFERENT LENGTH OF HIGHSCORE LISTS " + " namesList.Count: " + namesList.Count.ToString() + " pointsList.Count: " + pointsList.Count.ToString() + " datesList.Count: " + datesList.Count.ToString());
        }

        highScoreTextNames.text = "";
        highScoreTextPoints.text = "";
        highScoreTextDates.text = "";

        for (int i = 0; i < namesList.Count; i++)
        {
            highScoreTextNames.text += namesList[i] + "\n";
            highScoreTextPoints.text += pointsList[i].ToString() + "\n";
            highScoreTextDates.text += datesList[i].ToString() + "\n";
        }

        highScoreTextNames.gameObject.SetActive(true);
        highScoreTextPoints.gameObject.SetActive(true);
        highScoreTextDates.gameObject.SetActive(true);
    }

    public void ResetText()
    {
        // Sets scoreText
        scoreText.text = 0.ToString();
        scoreText.gameObject.SetActive(true);
        // Sets healthText
        healthText.text = GameObject.FindGameObjectWithTag("Controller").GetComponent<HealthScript>().GetHealth().ToString();
        healthText.gameObject.SetActive(true);
        // Sets highScore texts
        highScoreTextNames.text = "";
        highScoreTextNames.gameObject.SetActive(false);
        highScoreTextPoints.text = "";
        highScoreTextPoints.gameObject.SetActive(false);
        highScoreTextDates.text = "";
        highScoreTextDates.gameObject.SetActive(false);
}
}
