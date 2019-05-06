using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSpawner : MonoBehaviour
{
    private GameObject instancePlane;
    private float spawnTimer;

    public int maxInstances;
    public GameObject instanceObject;

    public GameObject Node1;
    public GameObject Node2;
    public GameObject Node3;
    public GameObject Node4;
    public GameObject Node5;
    public List<GameObject> spawnPoints;

    // Start is called before the first frame update
    void Awake()
    {
        instancePlane = transform.parent.Find("Game Plane").Find("Instance Plane").gameObject;
        spawnPoints.Add(Node2);
        spawnPoints.Add(Node3);
        spawnPoints.Add(Node4);
        spawnPoints.Add(Node5);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            spawnTimer = 1f;

            SpawnPerson(true);
        }
    }

    public GameObject SpawnPerson(bool random)
    {
        if (GetComponent<MatchHandler>().GetPersonCount() < maxInstances)
        {
            GameObject instance = Instantiate(instanceObject, instancePlane.transform);

            if (random == true)
            {
                int pos = Mathf.FloorToInt(Random.Range(0, spawnPoints.Count - 0.01f));
                instance.transform.Translate(transform.position - spawnPoints[pos].transform.position, Space.Self);
                instance.GetComponent<PersonScript>().SetPosition(5, 5);
                Debug.Log(instance.transform.position.x + " " + instance.transform.position.y + " " + instance.transform.position.z);
            }
            else
            {
                Debug.Log("OI");
                instance.transform.Translate(transform.position - Node1.transform.position, Space.Self);
                instance.GetComponent<PersonScript>().SetPosition(15, 5);
                Debug.Log(instance.transform.position.x + " " + instance.transform.position.y + " " + instance.transform.position.z);
            }

            GetComponent<MatchHandler>().AddPerson(instance);

            if (GetComponent<MatchHandler>().GetCurrentMatchSeeker() == null)
            {
                GetComponent<MatchHandler>().SetCurrentMatchSeekier(gameObject);
            }

            return instance;
        }

        return null;
    }

    private Vector3 RandomInstancePosition(out int spawnX, out int spawnZ)
    {
        var nh = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>();
        Vector2 pos = new Vector2(Mathf.RoundToInt(Random.Range(0, nh.gridWidth - 1)), Mathf.RoundToInt(Random.Range(0, nh.gridHeight - 1)));
        spawnX = Mathf.RoundToInt(pos.x);
        spawnZ = Mathf.RoundToInt(pos.y);
        return InstancePosition(pos);
    }

    private Vector3 InstancePosition(Vector2 pos)
    {
        var nh = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>();

        float returnX = nh.GetNode(pos).transform.position.x;
        float returnY = nh.GetNode(pos).transform.position.y;
        float returnZ = nh.GetNode(pos).transform.position.z;

        return new Vector3(returnX, returnY, returnZ);
    }
}
