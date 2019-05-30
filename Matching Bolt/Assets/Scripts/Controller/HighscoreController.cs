using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreController : MonoBehaviour
{
    public Text rankText;
    public Text namesText;
    public Text pointsText;
    public Text rankTextTop;
    public Text nameTextTop;
    public Text pointsTextTop;

    private void Awake()
    {
        var scoreKeeper = GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>();

        scoreKeeper.GetHighScoreToDisplay(out List<string> names, out List<float> points);

        rankText.text = "";
        namesText.text = "";
        pointsText.text = "";

        for (int i = 0; i < names.Count; i++)
        {
            rankText.text += (i + 1).ToString();
            namesText.text += names[i];
            pointsText.text += points[i];

            if (i < names.Count - 1)
            {
                rankText.text += "\n";
                namesText.text += "\n";
                pointsText.text += "\n";
            }
        }

        scoreKeeper.GetHighScoreToDisplayCurrent(out string n, out float p, out int r);

        rankTextTop.text = r.ToString();
        nameTextTop.text = n;
        pointsTextTop.text = p.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
