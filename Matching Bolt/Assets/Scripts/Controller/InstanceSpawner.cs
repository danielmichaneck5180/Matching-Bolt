using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSpawner : MonoBehaviour
{
    private GameObject instancePlane;
    private float spawnTimer;

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
        GameObject instance = Instantiate(instanceObject, instancePlane.transform);

        if (spawnPoint == 1)
        {
            instance.transform.position = new Vector3(0, instance.transform.position.y, 0);
        }
        else
        {
            instance.transform.position = new Vector3(-40, instance.transform.position.y, instance.transform.position.z + Random.Range(-20, 20));
        }

        return instance;
    }
}
