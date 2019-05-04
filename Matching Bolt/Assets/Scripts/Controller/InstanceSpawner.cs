using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSpawner : MonoBehaviour
{
    private GameObject instancePlane;
    private float spawnTimer;

    public int maxInstances;
    public GameObject instanceObject;

    // Start is called before the first frame update
    void Awake()
    {
        instancePlane = transform.parent.Find("Game Plane").Find("Instance Plane").gameObject;
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

            SpawnPerson(0);
        }
    }

    public GameObject SpawnPerson(int spawnPoint)
    {
        if (GetComponent<MatchHandler>().GetPersonCount() < maxInstances)
        {
            GameObject instance = Instantiate(instanceObject, instancePlane.transform);

            if (spawnPoint == 1)
            {
                instance.transform.Translate(InstancePosition(new Vector2(GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().gridWidth - 2, 0)), Space.Self);
                instance.GetComponent<PersonScript>().SetPosition(0, 0);
                //instance.transform.Translate(-15, 0, -15, Space.Self);
                //instance.transform.position = new Vector3(0, instance.transform.position.y, 0);
            }
            else
            {
                instance.transform.Translate(RandomInstancePosition(out int spawnX, out int spawnZ), Space.Self);
                instance.GetComponent<PersonScript>().SetPosition(spawnX, spawnZ);
                //instance.transform.Translate(20, 0, Random.Range(-15, 15), Space.Self);
                //instance.transform.position = new Vector3(-20, instance.transform.position.y, instance.transform.position.z + Random.Range(-15, 15));
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
        Vector2 pos = new Vector2(Mathf.RoundToInt(Random.Range(nh.gridWidth - 2, nh.gridWidth - 2)), Mathf.RoundToInt(Random.Range(0, nh.gridHeight - 2)));
        spawnX = Mathf.RoundToInt(pos.x);
        spawnZ = Mathf.RoundToInt(pos.y);
        //Debug.Log(spawnX);
        //Debug.Log(spawnZ);
        return InstancePosition(pos);
    }

    private Vector3 InstancePosition(Vector2 pos)
    {
        var nh = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>();

        float returnX = nh.GetNode(pos).transform.position.x;
        float returnZ = nh.GetNode(pos).transform.position.z;

        return new Vector3(returnX, 0, returnZ);
    }
}
