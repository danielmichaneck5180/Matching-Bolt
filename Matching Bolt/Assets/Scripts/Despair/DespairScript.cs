using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespairScript : MonoBehaviour
{
    public GameObject projectile;

    private enum DespairState { Start, Normal, Throw, End }
    private DespairState state;

    private GameObject player;
    private GameObject sprite;
    private RuntimeAnimatorController animatorController;
    private Animator animator;
    private int despair;
    private List<Vector2> vectorList;
    private GameObject currentNode;
    private int i;
    private int x;
    private int z;
    private float transformTimer;
    private float throwTimer;
    private float stopThrowTimer;
    private float endTimer;

    private float previousX;
    private float directionRotation;

    private void Awake()
    {
        previousX = transform.position.x;
        directionRotation = 0;
        state = DespairState.Start;
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = transform.Find("Rotation").gameObject;
        despair = Mathf.FloorToInt(Random.Range(0f, GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetMaxDespairs() - 0.01f));
        animatorController = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetDespair(despair);
        animator = sprite.transform.Find("Sprite").GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
        //sprite.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetRandomDespair();
        x = 0;
        z = 0;
        transformTimer = 0.5f;
        throwTimer = 3f;
        stopThrowTimer = 1f;
        endTimer = 1f;
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().AddPerson(gameObject);

        SetRandomPath();
        SetCurrentNode(i);
    }

    private void Update()
    {
        switch (state)
        {
            case DespairState.Start:
                sprite.transform.Find("Sprite").GetComponent<Animator>().Play("Idle", 0);
                if (transformTimer > 0)
                {
                    transformTimer -= Time.deltaTime;
                }
                else
                {
                    state = DespairState.Normal;
                }
                break;

            case DespairState.Normal:
                sprite.transform.Find("Sprite").GetComponent<Animator>().Play("Walking", 0);
                if (currentNode == null)
                {
                    Debug.Log("OI");
                }

                if (throwTimer > 0)
                {
                    throwTimer -= Time.deltaTime;
                }
                else
                {
                    throwTimer = 3f;
                    stopThrowTimer = 1f;
                    Shoot();
                    state = DespairState.Throw;
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
                        SetRandomPath();
                        SetCurrentNode(i);
                    }
                }
                else
                {
                    Vector3 newPos = currentNode.transform.position - transform.position;
                    newPos.Normalize();
                    transform.Translate((newPos / 8 ) * Time.deltaTime * 60, Space.World);
                }
                break;

            case DespairState.Throw:
                sprite.transform.Find("Sprite").GetComponent<Animator>().Play("Throw", 0);
                if (stopThrowTimer > 0)
                {
                    stopThrowTimer -= Time.deltaTime;
                }
                else
                {
                    state = DespairState.Normal;
                }
                break;

            case DespairState.End:
                sprite.transform.Find("Sprite").GetComponent<Animator>().Play("Idle", 0);
                if (endTimer > 0)
                {
                    endTimer -= Time.deltaTime;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Controller").GetComponent<InstanceSpawner>().SpawnPersonFromDespair(transform.position, x, z);
                    GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchHandler>().RemovePerson(gameObject);
                    Destroy(gameObject);
                }
                break;
        }

        RotateToCamera();
        Turn();
    }

    public void SetPosition(int setX, int setZ)
    {
        x = setX;
        z = setZ;
    }

    private void SetCurrentNode(int node)
    {
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[node]);
    }

    private void SetRandomPath()
    {
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
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x + directionRotation, 180, sprite.transform.rotation.eulerAngles.z + directionRotation));
    }

    private void Shoot()
    {
        GameObject p = Instantiate(projectile, transform);
        p.GetComponent<DespairProjectileScript>().SetupProjectile(player.transform.position);
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

    public void HitDespair()
    {
        state = DespairState.End;
    }
}
