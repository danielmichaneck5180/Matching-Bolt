using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    private bool isMatchSeeker;
    private bool isMatched;

    private void Awake()
    {
        isMatchSeeker = false;
    }

    void Update()
    {
        if (isMatchSeeker == true)
        {
            
        }
        else
        {
            transform.Translate(-10 * Time.deltaTime, 0, 0);
        }
    }

    public void FoundMatch()
    {
        isMatched = true;
        isMatchSeeker = false;
    }

    public bool Match()
    {
        if (CanBeMatched() == true)
        {
            isMatched = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CanBeMatched()
    {
        if (isMatched == true || isMatchSeeker == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void HitPerson()
    {
        Debug.Log("Hit person!");
    }

    public void BecomeMatchSeeker()
    {
        isMatchSeeker = true;
    }
}
