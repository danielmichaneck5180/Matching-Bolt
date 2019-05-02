using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreTextNames;
    public Text highScoreTextPoints;
    public Text highScoreTextDates;

    // Start is called before the first frame update
    void Start()
    {
        ResetText();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // Sets highScore texts
        highScoreTextNames.text = "";
        highScoreTextNames.gameObject.SetActive(false);
        highScoreTextPoints.text = "";
        highScoreTextPoints.gameObject.SetActive(false);
        highScoreTextDates.text = "";
        highScoreTextDates.gameObject.SetActive(false);
}
}
