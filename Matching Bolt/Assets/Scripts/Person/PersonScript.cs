using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public float destroyBoundary;
    public GameObject player;

    private bool isMatchSeeker;
    private bool isMatched;

    private Vector3 originPosition;

    private int TESTbounce;

    private GameObject sprite;

    private void Awake()
    {
        isMatchSeeker = false;
        originPosition = transform.position;
        TESTbounce = 1;
        sprite = transform.Find("Sprite").gameObject;
    }

    void Update()
    {
        if (isMatchSeeker == true)
        {
            transform.Find("Indicator").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Indicator").gameObject.SetActive(false);
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

        RotateToCamera();
    }

    private void RotateToCamera()
    {
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.rotation.eulerAngles, 10000f, 1000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x, 180, sprite.transform.rotation.eulerAngles.z));
        Debug.Log(sprite.transform.rotation.eulerAngles.x);
    }

    public void FoundMatch()
    {
        isMatched = true;
        isMatchSeeker = false;
        GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
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
