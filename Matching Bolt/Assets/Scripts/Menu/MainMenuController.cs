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

    private GameObject wiimote;
    private Vector3 startPosition;

    private Ray cursorRay;
    private RaycastHit cursorRayHit;

    private GameObject startButton;
    private GameObject optionsButton;
    private GameObject howtoplayButton;

    private void Awake()
    {
        Cursor.visible = false;
        startPosition = transform.position;
    }

    private void Start()
    {
        startButton = GameObject.FindGameObjectWithTag("Start Button").transform.Find("Start Indicator").gameObject;
        optionsButton = GameObject.FindGameObjectWithTag("Options Button").transform.Find("Options Indicator").gameObject;
        howtoplayButton = GameObject.FindGameObjectWithTag("How To Play Button").transform.Find("How To Play Indicator").gameObject;

        wiimote = GameObject.FindGameObjectWithTag("Wiimote Controller");
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
            //camera.transform.position = startPosition;
            float[] infra = new float[3];
            for (int i = 0; i < 3; i++)
            {
                infra[i] = wiimote.GetComponent<WiimoteScript>().GetCrossWiimotePosition()[i];
            }
            //infra[0] -= 0.5f; infra[1] -= 0.5f;// infra[2] -= 0.5f;
            infra[0] *= 1920f;
            infra[1] *= 1080f;
            Vector3 newVector = new Vector3(infra[0], infra[1], 1f);
            /*
            cursor.transform.position = startPosition + (newVector * 10f);
            cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, 1f);
            cursorRay = camera.ScreenPointToRay(newVector * 100f);
            */
            cursor.transform.position = camera.ScreenToWorldPoint(newVector);
            cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, 1f);
            cursorRay = camera.ScreenPointToRay(newVector);
            //Debug.Log("MENU :: INFRA 0: " + infra[0] + " INFRA 1: " + infra[1]);
        }
        else
        {
            cursor.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
            cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, 1f);
            cursorRay = camera.ScreenPointToRay(Input.mousePosition);
            //Debug.Log("MENU :: INFRA 0: " + Input.mousePosition.x + " INFRA 1: " + Input.mousePosition.y);
        }

        bool boolHit = false;
        if (Physics.Raycast(cursorRay, out cursorRayHit, Mathf.Infinity, LayerMask.GetMask("Button")))
        {
            boolHit = true;
        }

        if (boolHit == true)
        {
            switch (cursorRayHit.transform.tag)
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

    private void StartClicked()
    {
        if (Input.GetMouseButtonDown(0) == true || wiimote.GetComponent<WiimoteScript>().GetBButtonDown() == true)
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(cursorRay);
    }
}
