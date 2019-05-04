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
    private GameObject indicator;

    // TEMPORARY
    private List<Vector2> vectorList;
    private GameObject currentNode;
    private int i = 0;
    private float showTimer;
    private int interest;
    private int x;
    private int z;

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
        //x = 0;
        //z = 0;
        SetPath(15, 0);
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[0]);

        //TEMPORARY
        showTimer = 0.05f;
    }

    void Update()
    {
        if (isMatched == false)
        {
            /*
            while (currentNode == null)
            {
                SetRandomPath();
                SetCurrentNode(0);
                i = 0;
            }
            */
            if (Vector3.Distance(transform.position, currentNode.transform.position) < 1f)
            {
                if (i < vectorList.Count - 1)
                {
                    i++;
                    SetCurrentNode(i);
                    if (currentNode == null)
                    {
                        Debug.Log("ERROR: " + i + " " + vectorList.Count);
                    }
                    x = currentNode.GetComponent<NodeScript>().GetXPosition();
                    z = currentNode.GetComponent<NodeScript>().GetZPosition();
                }
                else
                {
                    Debug.Log("Found node: " + vectorList.Count);
                    i = 0;
                    SetRandomPath();
                }
            }
            else
            {
                Vector3 newPos = currentNode.transform.position - transform.position;
                newPos.Normalize();
                transform.Translate(newPos / 16, Space.World);
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
        }
        else
        {
            transform.Translate(new Vector3(5, 0, 0), Space.Self);
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
        vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(x, z, Mathf.RoundToInt(Random.Range(1f, 14f)), Mathf.RoundToInt(Random.Range(0f, 7f)));
        while (vectorList.Count > 39)
        {
            vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(Mathf.RoundToInt(Random.Range(1f, 14f)), Mathf.RoundToInt(Random.Range(0f, 7f)), Mathf.RoundToInt(Random.Range(1f, 14f)), Mathf.RoundToInt(Random.Range(0f, 7f)));
        }

        for (int i = 0; i < vectorList.Count; i++)
        {
            //Debug.Log("Added vector: " + vectorList[i].x + " " + vectorList[i].y);
        }

        //for (int i = 0; i < vectorList.Count; i++)
        //{
        //    if (GameObject.FindGameObjectWithTag("Controller").GetComponent<NodeHandler>().GetNode(vectorList[i]) == null)
        //    {
        //        Debug.Log(i);
        //    }
        //}
    }

    private void SetPath(int xCor, int zCor)
    {
        vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(0, 0, xCor, zCor);
        while (vectorList.Count > 39)
        {
            vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(0, 0, xCor, zCor);
        }
    }

    private void RotateToCamera()
    {
        //Debug.Log("Rotation: " + sprite.transform.rotation.eulerAngles.x + " " + player.transform.Find("Main Camera").transform.rotation.eulerAngles.x);
        //sprite.transform.LookAt(player.transform.Find("Main Camera").transform.position);
        //sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.position - sprite.transform.position, 10000f, 1000f));
        //sprite.transform.rotation = Quaternion.Euler(transform.position - player.transform.Find("Main Camera").transform.position);
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 1000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x, 180, sprite.transform.rotation.eulerAngles.z));
        //Debug.Log(sprite.transform.rotation.eulerAngles.x);
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
            indicator.SetActive(true);
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
}
