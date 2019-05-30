using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public GameObject popup;

    private bool shownLover;
    private bool shownInterest;
    private bool shownDespair;

    private void Awake()
    {
        shownLover = false;
        shownInterest = false;
        shownDespair = false;
    }

    private void Update()
    {
        if (shownLover == false)
        {
            if (GetComponent<MatchHandler>().GetCurrentMatchSeeker() != null && GetComponent<MatchHandler>().GetPersonCount() > 0)
            {
                GameObject pop = Instantiate(popup, GetComponent<MatchHandler>().GetCurrentMatchSeeker().transform);
                pop.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteReferences>().GetPopup("Lover");
                Debug.Log("Lover");
                shownLover = true;
                GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().SetGamePaused(true);
            }
        }
        else if (shownInterest == false)
        {
            if (GetComponent<MatchHandler>().GetPersonCount() > 1)
            {
                GameObject pop = Instantiate(popup, GetComponent<MatchHandler>().GetFirstPerson().transform);
                pop.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteReferences>().GetPopup("Interest");
                Debug.Log("Interest");
                shownInterest = true;
                GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().SetGamePaused(true);
            }
        }
        else if (shownDespair == false)
        {
            if (GameObject.FindGameObjectsWithTag("Despair").Length > 0)
            {
                GameObject pop = Instantiate(popup, GameObject.FindGameObjectsWithTag("Despair")[0].transform);
                pop.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteReferences>().GetPopup("Despair");
                Debug.Log("Despair");
                shownDespair = true;
                GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().SetGamePaused(true);
            }
        }
    }
}
