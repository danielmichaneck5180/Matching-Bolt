using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private GameObject currentMatchSeeker;

    private void Start()
    {
        if (currentMatchSeeker == null)
        {
            currentMatchSeeker = GetComponent<InstanceSpawner>().SpawnPerson(1);
            currentMatchSeeker.GetComponent<PersonScript>().BecomeMatchSeeker();
        }
    }

    public void MatchMade(GameObject newMatch)
    {
        currentMatchSeeker.GetComponent<PersonScript>().FoundMatch();
        newMatch.GetComponent<PersonScript>().Match();
    }
}
