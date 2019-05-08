using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    List<GameObject> personList;

    private GameObject currentMatchSeeker;

    //TEMPORARY
    private int matchInterest;
    private float matchTimer;

    private void Awake()
    {
        personList = new List<GameObject>();
        matchInterest = 0;
        matchTimer = 15f;
    }

    private void Start()
    {
        if (currentMatchSeeker == null)
        {
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnPerson(false);
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
            matchInterest = currentMatchSeeker.GetComponent<PersonScript>().GetInterest();
        }
    }

    private void Update()
    {
        if (currentMatchSeeker == null)
        {
            Debug.Log("Spawned matchseeker");
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnPerson(false);
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
            matchInterest = currentMatchSeeker.GetComponent<PersonScript>().GetInterest();
        }
    }

    public void MatchMade(GameObject newMatch)
    {
        if (newMatch.GetComponent<PersonScript>().GetInterest() == matchInterest)
        {
            if (newMatch.GetComponent<PersonScript>().Match(currentMatchSeeker) == true)
            {
                currentMatchSeeker.GetComponent<PersonScript>().FoundMatch(newMatch);
                currentMatchSeeker = null;
                GetComponent<ScoreKeeper>().AddPoints(2);
            }
        }
        else if (GetComponent<ScoreKeeper>().GetScore() > 0)
        {
            GetComponent<ScoreKeeper>().AddPoints(-1);
        }
    }

    public bool RemovePerson(GameObject person)
    {
        if (personList.Contains(person) == true)
        {
            personList.Remove(person);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddPerson(GameObject person)
    {
        if (personList.Contains(person) == false)
        {
            personList.Add(person);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetPersonCount()
    {
        return personList.Count;
    }

    public GameObject GetCurrentMatchSeeker()
    {
        return currentMatchSeeker;
    }

    public void SetCurrentMatchSeekier(GameObject newMatchSeeker)
    {
        currentMatchSeeker = newMatchSeeker;
    }
}
