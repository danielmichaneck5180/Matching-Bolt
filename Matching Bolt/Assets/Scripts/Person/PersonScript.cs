﻿using System.Collections;
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
    private GameObject indicator;
    
    private List<Vector2> vectorList;
    private GameObject currentNode;
    private int i = 0;
    private float showTimer;
    private int interest;
    private int x;
    private int z;

    enum PersonState { Normal, StraightPath, MatchWaiting, MatchMover, MatchEnd };
    private PersonState state;
    private GameObject matchObject;

    private void Awake()
    {
        var conspr = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>();
        isMatchSeeker = false;
        originPosition = transform.position;
        TESTbounce = 1;
        sprite = transform.Find("Rotation").gameObject;
        indicator = sprite.transform.Find("Sprite").transform.Find("Indicator").gameObject;
        interest = Mathf.RoundToInt(Random.Range(0, conspr.GetMaxInterests() - 1));
        indicator.GetComponent<SpriteRenderer>().sprite = conspr.GetInterest(interest);
        sprite.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = conspr.GetRandomPerson();

        vectorList = new List<Vector2>();

        //vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(0, 0, 15, 8);
        x = 0;
        z = 0;

        //SetPath(15, 8);

        //TEMPORARY
        showTimer = 0.05f;

        state = PersonState.Normal;
    }

    private void Start()
    {
        if (isMatchSeeker == false)
        {
            SetRandomPath();
        }
    }

    void Update()
    {
        switch(state)
        {
            case PersonState.Normal:
                if (Vector3.Distance(transform.position, currentNode.transform.position) < 1f)
                {
                    if (i < vectorList.Count - 1)
                    {
                        i++;
                        SetCurrentNode(i);
                        x = currentNode.GetComponent<NodeScript>().GetXPosition();
                        z = currentNode.GetComponent<NodeScript>().GetZPosition();
                    }
                    else
                    {
                        SetRandomPath();
                        SetCurrentNode(i);
                    }
                }
                else
                {
                    Vector3 newPos = currentNode.transform.position - transform.position;
                    newPos.Normalize();
                    transform.Translate(newPos / 16, Space.World);
                }
                break;

            case PersonState.StraightPath:
                if (vectorList.Count > 0)
                {
                    if (vectorList[vectorList.Count - 1] != new Vector2(15, 3))
                    {
                        SetPath(15, 3);
                    }
                }
                else
                {
                    SetPath(15, 3);
                }

                if (Vector3.Distance(transform.position, currentNode.transform.position) < 1f)
                {
                    if (i < vectorList.Count - 1)
                    {
                        i++;
                        SetCurrentNode(i);
                        x = currentNode.GetComponent<NodeScript>().GetXPosition();
                        z = currentNode.GetComponent<NodeScript>().GetZPosition();
                    }
                    else
                    {
                        state = PersonState.MatchEnd;
                    }
                }
                else
                {
                    Vector3 newPos = currentNode.transform.position - transform.position;
                    newPos.Normalize();
                    transform.Translate(newPos / 16, Space.World);
                }
                break;

            case PersonState.MatchMover:
                if (matchObject != null)
                {
                    if (vectorList.Count <= 0)
                    {
                        SetPath(matchObject.GetComponent<PersonScript>().GetPosition()[0], matchObject.GetComponent<PersonScript>().GetPosition()[1]);
                    }
                    else if (vectorList[vectorList.Count - 1 ] != new Vector2(matchObject.GetComponent<PersonScript>().GetPosition()[0], matchObject.GetComponent<PersonScript>().GetPosition()[1]))
                    {
                        SetPath(matchObject.GetComponent<PersonScript>().GetPosition()[0], matchObject.GetComponent<PersonScript>().GetPosition()[1]);
                    }

                    if (Vector3.Distance(transform.position, currentNode.transform.position) < 1f)
                    {
                        if (i < vectorList.Count - 1)
                        {
                            i++;
                            SetCurrentNode(i);
                            x = currentNode.GetComponent<NodeScript>().GetXPosition();
                            z = currentNode.GetComponent<NodeScript>().GetZPosition();
                        }
                        else
                        {
                            state = PersonState.StraightPath;
                            matchObject.GetComponent<PersonScript>().SetState(1);
                        }
                    }
                    else
                    {
                        Vector3 newPos = currentNode.transform.position - transform.position;
                        newPos.Normalize();
                        transform.Translate(newPos / 16, Space.World);
                    }
                }
                break;

            case PersonState.MatchWaiting:
                break;

            case PersonState.MatchEnd:
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("MatchEndPoint").transform.position) < 1f)
                {
                    if (GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().GetCurrentMatchSeeker() == gameObject)
                    {
                        GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().SetCurrentMatchSeekier(null);
                    }
                    GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
                    Destroy(gameObject);
                }
                else
                {
                    Vector3 newPos = GameObject.FindGameObjectWithTag("MatchEndPoint").transform.position - transform.position;
                    newPos.Normalize();
                    transform.Translate(newPos / 16, Space.World);
                }
                break;
        }

        if (CanBeMatched() == true)
        {
            if (showTimer > 0)
            {
                showTimer -= Time.deltaTime;
            }
            else
            {
                indicator.SetActive(false);
            }
        }

        // Checks if the instance is outside of destroyBoundary and if true destroys it
        if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.x) >= destroyBoundary)
        {
            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
            Destroy(gameObject);
        }

        RotateToCamera();
    }

    private void SetCurrentNode(int node)
    {
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[node]);
    }

    private void SetRandomPath()
    {
        if (isMatchSeeker == true)
        {
            Debug.Log("Wat");
        }
        int goX = Mathf.RoundToInt(Random.Range(1f, 14f));
        int goZ = Mathf.RoundToInt(Random.Range(0f, 7f));

        for (int i = 0; i < 10; i++)
        {
            if (goZ == 3)
            {
                goZ = Mathf.RoundToInt(Random.Range(0f, 7f));
            }
        }

        if (goZ == 3)
        {
            goZ = 4;
        }

        vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(x, z, goX, goZ);
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[0]);
        i = 0;
    }

    private void SetPath(int xCor, int zCor)
    {
        vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(x, z, xCor, zCor);
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[0]);
        i = 0;
    }

    private void RotateToCamera()
    {
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 1000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x, 180, sprite.transform.rotation.eulerAngles.z));
    }

    public void FoundMatch(GameObject match)
    {
        if (isMatchSeeker == true)
        {
            state = PersonState.MatchWaiting;
            matchObject = match;
        }
        else
        {
            state = PersonState.MatchMover;
            matchObject = match;
        }
        isMatchSeeker = false;
        isMatched = true;
        GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
    }

    public bool Match(GameObject match)
    {
        if (CanBeMatched() == true)
        {
            FoundMatch(match);
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
            indicator.SetActive(true);
            state = PersonState.StraightPath;
            Debug.Log("X: " + x + " Z: " + z + " List size: " + vectorList.Count);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowInterest()
    {
        indicator.SetActive(true);
        showTimer = 0.05f;
    }

    public int GetInterest()
    {
        return interest;
    }

    public void SetPosition(int setX, int setZ)
    {
        x = setX;
        z = setZ;
    }

    public int[] GetPosition()
    {
        return new int[] { x, z };
    }

    public void SetMatchObject(GameObject match)
    {
        matchObject = match;
    }

    public void SetState(int newState)
    {
        switch (newState)
        {
            case 0:
                state = PersonState.Normal;
                break;

            case 1:
                state = PersonState.StraightPath;
                break;

            case 2:
                state = PersonState.MatchWaiting;
                break;

            case 3:
                state = PersonState.MatchMover;
                break;

            case 4:
                state = PersonState.MatchEnd;
                break;
        }
    }
}
