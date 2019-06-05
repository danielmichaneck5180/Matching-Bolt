using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private List<GameObject> personList;
    private List<bool> despairList;

    private GameObject currentMatchSeeker;

    private int matchInterest;
    private int queuedInterest;
    private List<int> queuedInterestList;

    private void Awake()
    {
        personList = new List<GameObject>();
        ResetDespairList();
        matchInterest = 0;
        queuedInterest = -1;
        queuedInterestList = new List<int>();
    }

    private void Start()
    {
        UpdateMatchSeeker();
    }

    private void Update()
    {
        UpdateMatchSeeker();
    }

    private void UpdateMatchSeeker()
    {
        if (currentMatchSeeker == null)
        {
            Debug.Log("Spawned matchseeker");
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnMatchSeeker();
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
            if (personList.Count < 1)
            {

                matchInterest = currentMatchSeeker.GetComponent<PersonScript>().GetInterest();
                queuedInterest = matchInterest;
                Debug.Log("Interest queued");
            }
            else
            {
                queuedInterest = -1;
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
                int rMatch = Mathf.FloorToInt(Random.Range(0f, list.Count - 0.001f));
                matchInterest = list[rMatch];
                currentMatchSeeker.GetComponent<PersonScript>().SetInterest(matchInterest);
            }
        }
    }

    private void ResetDespairList()
    {
        float dif = Mathf.FloorToInt(GetComponent<GameHandler>().GetDifficultyMultiplier());
        int setup;
        despairList = new List<bool>();

        switch (dif)
        {
            case 0:
                setup = 0;
                break;

            case 1:
                setup = 0;
                break;

            case 2:
                setup = 1;
                break;

            case 3:
                setup = 2;
                break;

            case 4:
                setup = 3;
                break;

            case 5:
                setup = 3;
                break;

            default:
                setup = 4;
                break;
        }

        switch (setup)
        {
            case 0:
                despairList.Add(false);
                break;

            case 1:
                despairList.Add(false);
                despairList.Add(false);
                despairList.Add(false);
                if (Random.Range(0f, 2f) < 1f)
                {

                    despairList.Add(true);
                    despairList.Add(false);
                }
                else
                {
                    despairList.Add(false);
                    despairList.Add(true);
                }
                break;

            case 2:
                despairList.Add(false);
                despairList.Add(false);
                if (Random.Range(0f, 2f) < 1f)
                {

                    despairList.Add(true);
                    despairList.Add(false);
                }
                else
                {
                    despairList.Add(false);
                    despairList.Add(true);
                }
                break;

            case 3:
                despairList.Add(false);
                if (Random.Range(0f, 2f) < 1f)
                {

                    despairList.Add(true);
                    despairList.Add(false);
                }
                else
                {
                    despairList.Add(false);
                    despairList.Add(true);
                }
                break;

            default:
                if (Random.Range(0f, 2f) < 1f)
                {

                    despairList.Add(true);
                    despairList.Add(false);
                }
                else
                {
                    despairList.Add(false);
                    despairList.Add(true);
                }
                break;
        }
    }

    public void MatchMade(GameObject newMatch)
    {
        if (newMatch.GetComponent<PersonScript>().GetInterest() == matchInterest)
        {
            if (newMatch.GetComponent<PersonScript>().Match(currentMatchSeeker) == true)
            {
                GetComponent<AudioManager>().PlaySound("Match1");
                currentMatchSeeker.GetComponent<PersonScript>().FoundMatch(newMatch);
                currentMatchSeeker = null;
                GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().AddPoints(20);
            }
        }
        /*
        else if (GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().GetScore() > 0)
        {
            GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().AddPoints(-1);
        }
        */
        else
        {
            newMatch.GetComponent<PersonScript>().TurnToDespair();
            GameObject.FindGameObjectWithTag("Score Keeper").GetComponent<ScoreKeeper>().AddPoints(-10);
            //GetComponent<InstanceSpawner>().SpawnDespair(newMatch.transform.position, newMatch.GetComponent<PersonScript>().GetPosition()[0], newMatch.GetComponent<PersonScript>().GetPosition()[1], newMatch.GetComponent<PersonScript>().GetInterest());
        }
    }

    private void SetQueuedInterest()
    {
        int rand = Mathf.FloorToInt(Random.Range(0f, 2.999f));
        switch (rand)
        {
            case 0:
                queuedInterestList.Add(matchInterest);
                queuedInterestList.Add(-1);
                queuedInterestList.Add(-1);
                break;

            case 1:
                queuedInterestList.Add(-1);
                queuedInterestList.Add(matchInterest);
                queuedInterestList.Add(-1);
                break;

            case 2:
                queuedInterestList.Add(-1);
                queuedInterestList.Add(-1);
                queuedInterestList.Add(matchInterest);
                break;

            default:
                queuedInterestList.Add(matchInterest);
                queuedInterestList.Add(-1);
                queuedInterestList.Add(-1);
                break;
        }
    }

    public int GetQueuedInterest()
    {
        if (queuedInterest > -1)
        {
            if (queuedInterestList.Count < 1)
            {
                SetQueuedInterest();
            }
            else if (queuedInterestList.Contains(matchInterest) == false)
            {
                SetQueuedInterest();
            }

            int returnInterest = queuedInterestList[0];

            if (returnInterest > -1)
            {
                queuedInterest = -1;
            }
            else
            {
                queuedInterestList.RemoveAt(0);
            }
            return returnInterest;
        }
        else
        {
            return -1;
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

    public GameObject GetFirstPerson()
    {
        return personList[0];
    }
}
