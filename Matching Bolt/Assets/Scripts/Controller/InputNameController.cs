using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputNameController : MonoBehaviour
{
    public Text nameText;

    List<KeyCode> keyList;

    private void Awake()
    {
        nameText.text = "";
    }

    private void Update()
    {
        if (nameText.text.Length < 15)
        {
            if (Input.inputString.Length > 0)
            {
                if (nameText.text == "")
                {
                    nameText.text = Input.inputString;
                }
                else if (Input.GetKey(KeyCode.Return) == false)
                {
                    nameText.text += Input.inputString;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) == true && nameText.text.Length > 1)
        {
            int currentLength = nameText.text.Length;
            nameText.text = nameText.text.Substring(0, (currentLength - 2));
        }

        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().SetPlayerName(nameText.text);
            GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().SaveHighscoreText();
            SceneManager.LoadScene("Highscore Scene");
        }
    }
}
