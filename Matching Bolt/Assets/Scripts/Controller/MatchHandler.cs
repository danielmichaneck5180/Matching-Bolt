using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private List<GameObject> personList;
    private List<bool> despairList;

    private GameObject currentMatchSeeker;

    //TEMPORARY
    private int matchInterest;

    private void Awake()
    {
        personList = new List<GameObject>();
        ResetDespairList();
        matchInterest = 0;
    }

    private void Start()
    {
        if (currentMatchSeeker == null)
        {
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnMatchSeeker();
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
            matchInterest = currentMatchSeeker.GetComponent<PersonScript>().GetInterest();
        }
    }

    private void Update()
    {
        if (currentMatchSeeker == null)
        {
            Debug.Log("Spawned matchseeker");
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnMatchSeeker();
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
            List<int> list = new List<int>();
            for (int i = 0; i < personList.Count; i++)
            {
                if (personList[i].GetComponent<PersonScript>() != null)
                {
                    if (list.Contains(personList[i].GetComponent<PersonScript>().GetInterest()) == false)
                    {
                        list.Add(personList[i].GetComponent<PersonScript>().GetInterest());
                    }
                }
            }
            int rMatch = Mathf.FloorToInt(Random.Range(0f, list.Count - 0.01f));
            matchInterest = list[rMatch];
            currentMatchSeeker.GetComponent<PersonScript>().SetInterest(matchInterest);
        }
    }

    private void ResetDespairList()
    {
        despairList = new List<bool>();
        despairList.Add(false);
        despairList.Add(false);

        if (Random.Range(0f, 3f) < 1)
        {
            despairList.Add(true);
            despairList.Add(false);
            despairList.Add(false);
        }
        else
        {
            if (Random.Range(0f, 2f) < 1)
            {
                despairList.Add(false);
                despairList.Add(true);
                despairList.Add(false);
            }
            else
            {
                despairList.Add(false);
                despairList.Add(false);
                despairList.Add(true);
            }
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

    public bool GetDespairStatus()
    {
        bool returnBool = false;
        if (despairList.Count < 1)
        {
            ResetDespairList();
        }
        if (despairList[0] == true)
        {
            returnBool = true;
        }
        despairList.RemoveAt(0);
        return returnBool;
    }
}
