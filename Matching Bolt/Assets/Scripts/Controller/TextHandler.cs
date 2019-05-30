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

    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;

    void Start()
    {
        ResetText();
        ResetHealth();
    }

    void Update()
    {
        SetHealth();
    }

    private void SetHealth()
    {
        int health = GetComponent<HealthScript>().GetHealth();
        int fullHearts = Mathf.FloorToInt(health / 2);
        health -= fullHearts * 2;
        int halfHearts = health;

        if (fullHearts >= 1)
        {
            Heart1.SetActive(true);
            Heart1.GetComponent<Image>().sprite = GetComponent<SpriteReferences>().GetHeart(2);

            if (fullHearts >= 2)
            {
                Heart2.SetActive(true);
                Heart2.GetComponent<Image>().sprite = GetComponent<SpriteReferences>().GetHeart(2);

                if (fullHearts >= 3)
                {
                    Heart3.SetActive(true);
                    Heart3.GetComponent<Image>().sprite = GetComponent<SpriteReferences>().GetHeart(2);
                }
                else
                {
                    if (halfHearts > 0)
                    {
                        Heart3.SetActive(true);
                        Heart3.GetComponent<Image>().sprite = GetComponent<SpriteReferences>().GetHeart(0);
                        Heart3.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(67f, 100f);
                    }
                    else
                    {
                        Heart3.SetActive(false);
                    }
                }
            }
            else
            {
                if (halfHearts > 0)
                {
                    Heart2.SetActive(true);
                    Heart2.GetComponent<Image>().sprite = GetComponent<SpriteReferences>().GetHeart(0);
                    Heart2.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(67f, 100f);
                }
                else
                {
                    Heart2.SetActive(false);
                }
            }
        }
        else
        {
            if (halfHearts > 0)
            {
                Heart1.SetActive(true);
                Heart1.GetComponent<Image>().sprite = GetComponent<SpriteReferences>().GetHeart(0);
                Heart1.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(67f, 100f);
            }
            else
            {
                Heart1.SetActive(false);
            }
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

        GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().GetHighScore(out namesList, out pointsList, out datesList);

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
        //Sets health

    }

    private void ResetHealth()
    {
        Heart1.SetActive(false);
        Heart2.SetActive(false);
        Heart3.SetActive(false);
    }
}
