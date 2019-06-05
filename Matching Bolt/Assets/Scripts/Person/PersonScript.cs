using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public float destroyBoundary;
    public GameObject player;
    public GameObject heartAnimation;
    private RuntimeAnimatorController animatorController;
    private Animator animator;

    private bool isMatchSeeker;
    private bool isMatched;

    private Vector3 originPosition;

    private GameObject sprite;
    private GameObject indicator;
    
    private List<Vector2> vectorList;
    private GameObject currentNode;
    private int i = 0;
    private float showTimer;

    private int interest;
    private bool knownDespairStatus;

    private int x;
    private int z;

    enum PersonState { Normal, StraightPath, MatchWaiting, MatchMover, MatchEnd };
    private PersonState state;
    private GameObject matchObject;

    private float previousX;
    private float directionRotation;

    private float minY;
    private float maxY;

    private void Awake()
    {
        previousX = transform.position.x;
        directionRotation = 0;
        var conspr = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>();
        isMatchSeeker = false;
        originPosition = transform.position;
        sprite = transform.Find("Rotation").gameObject;
        indicator = sprite.transform.Find("Sprite").transform.Find("Indicator").gameObject;
        SetIndicatorVisible(false);
        int i = GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().GetQueuedInterest();
        if (i > -1)
        {
            SetInterest(i);
        }
        else
        {
            SetInterest(Mathf.FloorToInt(Random.Range(0f, conspr.GetMaxInterests() - 0.001f)));
        }
        animatorController = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetRandomPerson();
        animator = sprite.transform.Find("Sprite").GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
        //sprite.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = conspr.GetRandomPerson();

        vectorList = new List<Vector2>();

        //vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(0, 0, 15, 8);
        x = 0;
        z = 0;

        //SetPath(15, 8);

        //TEMPORARY
        showTimer = 0.05f;

        state = PersonState.Normal;

        knownDespairStatus = false;
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
        if (GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetGamePaused() == false)
        {
            switch (state)
            {
                case PersonState.Normal:
                    animator.Play("Walking");
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
                        transform.Translate((newPos / 16) * Time.deltaTime * 60, Space.World);
                    }
                    break;

                case PersonState.StraightPath:
                    animator.Play("Walking");
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
                        int multiplier = Mathf.FloorToInt(GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetDifficultyMultiplier());
                        if (multiplier < 4f)
                        {
                            multiplier = 1;
                        }
                        else
                        {
                            multiplier -= 3;
                        }
                        if (isMatched == false)
                        {
                            transform.Translate(((newPos / 12) + ((newPos / 240) * (multiplier - 1))) * Time.deltaTime * 60, Space.World);
                        }
                        else
                        {
                            transform.Translate((newPos / 6) * Time.deltaTime * 60, Space.World);
                        }
                    }
                    break;

                case PersonState.MatchMover:
                    animator.Play("Walking");
                    if (matchObject != null)
                    {
                        if (vectorList.Count <= 0)
                        {
                            SetPath(matchObject.GetComponent<PersonScript>().GetPosition()[0], matchObject.GetComponent<PersonScript>().GetPosition()[1]);
                        }
                        else if (vectorList[vectorList.Count - 1] != new Vector2(matchObject.GetComponent<PersonScript>().GetPosition()[0], matchObject.GetComponent<PersonScript>().GetPosition()[1]))
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
                            transform.Translate((newPos / 6) * Time.deltaTime * 60, Space.World);
                        }
                    }
                    break;

                case PersonState.MatchWaiting:
                    animator.Play("Idle");
                    break;

                case PersonState.MatchEnd:
                    if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("MatchEndPoint").transform.position) < 1f)
                    {
                        if (GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().GetCurrentMatchSeeker() == gameObject)
                        {
                            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().SetCurrentMatchSeekier(null);
                            TurnToDespair();
                        }
                        else
                        {
                            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Vector3 newPos = GameObject.FindGameObjectWithTag("MatchEndPoint").transform.position - transform.position;
                        newPos.Normalize();
                        if (isMatched == false)
                        {
                            int multiplier = Mathf.FloorToInt(GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetDifficultyMultiplier());
                            if (multiplier < 4f)
                            {
                                multiplier = 1;
                            }
                            else
                            {
                                multiplier -= 3;
                            }
                            transform.Translate(((newPos / 12) + ((newPos / 240) * (multiplier - 1))) * Time.deltaTime * 60, Space.World);
                        }
                        else
                        {
                            transform.Translate((newPos / 6) * Time.deltaTime * 60, Space.World);
                        }
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
                    SetIndicatorVisible(false);
                }
            }

            // Checks if the instance is outside of destroyBoundary and if true destroys it
            if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.x) >= destroyBoundary)
            {
                GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
                Destroy(gameObject);
            }
        }

        RotateToCamera();
        Turn();
    }

    private void SetCurrentNode(int node)
    {
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[node]);
    }

    private void SetRandomPath()
    {
        int goX = Mathf.RoundToInt(Random.Range(1f, 14f));
        int goZ = Mathf.RoundToInt(Random.Range(minY, maxY));

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
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 10000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x + 180f, 0f, sprite.transform.rotation.eulerAngles.z + directionRotation));
        Transform indicator = sprite.transform.Find("Sprite").transform.Find("Indicator").transform;
        indicator.rotation = Quaternion.Euler(Vector3.RotateTowards(indicator.rotation.eulerAngles, indicator.transform.forward, 10000f, 10000f));
        indicator.rotation = Quaternion.Euler(new Vector3(90f, indicator.rotation.eulerAngles.y, indicator.rotation.eulerAngles.z));
    }

    private void ShowInterest()
    {
        SetIndicatorVisible(true);
        showTimer = 0.05f;
    }

    public void CheckDespair()
    {
        if (knownDespairStatus == false)
        {
            //knownDespairStatus = true;
            if (GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().GetDespairStatus() == true)
            {
                TurnToDespair();
            }
        }
    }

    public void TurnToDespair()
    {
        if (knownDespairStatus == false)
        {
            GameObject.FindGameObjectWithTag("Controller").GetComponent<InstanceSpawner>().SpawnDespair(transform.position, x, z, interest);
            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
            Destroy(gameObject);
        }
    }

    private void Turn()
    {
        if (transform.position.x > previousX)
        {
            directionRotation = 0;
        }
        else if (transform.position.x < previousX)
        {
            directionRotation = 180;
        }

        previousX = transform.position.x;
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
        Instantiate(heartAnimation, sprite.transform);
        SetIndicatorVisible(false);
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
            SetIndicatorVisible(true);
            state = PersonState.StraightPath;
            GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
            //Debug.Log("X: " + x + " Z: " + z + " List size: " + vectorList.Count);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AimedAt()
    {
        if (GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetGamePaused() == false)
        {
            if (isMatchSeeker == false && isMatched == false)
            {
                //CheckDespair();
                ShowInterest();
            }
        }
    }

    public int GetInterest()
    {
        return interest;
    }

    public void SetInterest(int newInterest)
    {
        interest = newInterest;
        indicator.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetInterest(interest);
    }

    public void SetKnownDespairStatus(bool b)
    {
        knownDespairStatus = b;
    }

    public void SetRandomPosition(int setX, int setZ)
    {
        if (setZ == 3)
        {
            setZ = 2;
        }

        if (setZ > 3)
        {
            minY = 5;
            maxY = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetGridHeight() - 1;
        }
        else
        {
            minY = 0;
            maxY = 2;
        }

        x = setX;
        z = setZ;
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

    private void SetIndicatorVisible(bool visible)
    {
        switch (visible)
        {
            case false:
                indicator.SetActive(false);
                break;

            default:
                if (isMatched == false)
                {
                    indicator.SetActive(true);
                }
                else
                {
                    indicator.SetActive(false);
                }
                break;
        }
    }
}
