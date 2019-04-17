using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    private bool isMatchSeeker;
    private bool isMatched;

    public float destroyBoundary;
    private Vector3 originPosition;

    private int TESTbounce;

    private void Awake()
    {
        isMatchSeeker = false;
        originPosition = transform.position;
        TESTbounce = 1;
    }

    void Update()
    {
        if (isMatchSeeker == true)
        {
            
        }
        else
        {
            transform.Translate(-10 * Time.deltaTime * TESTbounce, 0, 0);
        }

        // Checks if the instance is outside of destroyBoundary and if true destroys it
        if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.x) >= destroyBoundary)
        {
            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
            Destroy(gameObject);
        }

        // Bounces
        if (isMatched == false)
        {
            if (transform.position.x >= 20)
            {
                TESTbounce = -1;
            }
            if (transform.position.x <= -20)
            {
                TESTbounce = 1;
            }
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
            FoundMatch();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanBeMatched()
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
        if (CanBeMatched() == true)
        {
            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().MatchMade(gameObject);
        }
    }

    public bool BecomeMatchSeeker()
    {
        if (CanBeMatched() == true)
        {
            isMatchSeeker = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
