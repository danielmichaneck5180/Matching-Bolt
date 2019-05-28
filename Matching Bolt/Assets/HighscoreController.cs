using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour
{
    public Text rankText;
    public Text namesText;
    public Text pointsText;

    private void Awake()
    {
        GetComponent<ScoreKeeper>().GetHighScore(out List<string> names, out List<float> points, out List<System.DateTime> dates);

        for (int i = 0; i < names.Count; i++)
        {
            rankText.text += (i + 1).ToString();
            namesText.text += names[i];
            pointsText.text += points[i];
        }
    }
}
