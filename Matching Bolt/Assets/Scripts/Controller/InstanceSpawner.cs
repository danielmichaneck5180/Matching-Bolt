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
                instance.transform.Translate(0, 0, 0, Space.Self);
                //instance.transform.position = new Vector3(0, instance.transform.position.y, 0);
            }
            else
            {
                instance.transform.Translate(20, 0, Random.Range(-15, 15), Space.Self);
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
}
