using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public bool wiimoteEnabled;
    public GameObject cursor;
    public Camera camera;

    private Ray cursorRay;
    private RaycastHit cursorRayHit;

    private GameObject startButton;
    private GameObject optionsButton;
    private GameObject howtoplayButton;

    private void Start()
    {
        startButton = GameObject.FindGameObjectWithTag("Start Button").transform.Find("Start Indicator").gameObject;
        optionsButton = GameObject.FindGameObjectWithTag("Options Button").transform.Find("Options Indicator").gameObject;
        howtoplayButton = GameObject.FindGameObjectWithTag("How To Play Button").transform.Find("How To Play Indicator").gameObject;
    }

    private void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        startButton.SetActive(false);
        optionsButton.SetActive(false);
        howtoplayButton.SetActive(false);

        if (wiimoteEnabled == true)
        {

        }
        else
        {
            cursor.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
            cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z + 10);
            cursorRay = camera.ScreenPointToRay(Input.mousePosition);

            bool boolHit = false;
            if (Physics.Raycast(cursorRay, out cursorRayHit, 1000f, LayerMask.GetMask("Button")))
            {
                boolHit = true;
            }

            if (boolHit == true)
            {
                switch(cursorRayHit.transform.tag)
                {
                    case "Start Button":
                        startButton.SetActive(true);
                        StartClicked();
                        break;

                    case "Options Button":
                        optionsButton.SetActive(true);
                        OptionsClicked();
                        break;

                    case "How To Play Button":
                        howtoplayButton.SetActive(true);
                        HowToPlayClicked();
                        break;
                }
            }
        }
    }

    private void StartClicked()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            SceneManager.LoadScene("Main Scene");
        }
    }

    private void OptionsClicked()
    {

    }

    private void HowToPlayClicked()
    {

    }
}
