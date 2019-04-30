using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    List<GameObject> personList;

    private GameObject currentMatchSeeker;

    private void Awake()
    {
        personList = new List<GameObject>();
    }

    private void Start()
    {
        if (currentMatchSeeker == null)
        {
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnPerson(1);
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
        }
    }

    private void Update()
    {
        //Debug.Log(personList.Count);
    }

    public void MatchMade(GameObject newMatch)
    {
        currentMatchSeeker.GetComponent<PersonScript>().FoundMatch();
        newMatch.GetComponent<PersonScript>().Match();

        GetComponent<ScoreKeeper>().AddPoints(1);

        bool stopLoop = false;

        if (personList.Count <= 0)
        {
            currentMatchSeeker = null;
        }

        for (int i = 0; i < personList.Count; i++)
        {
            if (personList[i].GetComponent<PersonScript>().CanBeMatched() == true && stopLoop == false)
            {
                currentMatchSeeker = personList[i];
                currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
                stopLoop = true;
                Debug.Log(i);
            }
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
