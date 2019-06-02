﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSpawner : MonoBehaviour
{
    private GameObject instancePlane;
    private float spawnTimer;

    public int maxInstances;
    public GameObject personObject;
    public GameObject despairObject;

    public GameObject matchSpawnNode;
    public GameObject matchEndNode;

    public GameObject leftNode1;
    public GameObject leftNode2;
    public GameObject leftNode3;
    public GameObject leftNode4;
    public GameObject leftNode5;
    public GameObject leftNode6;
    public GameObject leftNode7;

    public GameObject rightNode1;
    public GameObject rightNode2;
    public GameObject rightNode3;
    public GameObject rightNode4;
    public GameObject rightNode5;
    public GameObject rightNode6;
    public GameObject rightNode7;

    private List<GameObject> spawnPoints;

    // Start is called before the first frame update
    void Awake()
    {
        spawnTimer = 2f;

        instancePlane = transform.parent.Find("Game Plane").Find("Instance Plane").gameObject;
        spawnPoints = new List<GameObject>();
        spawnPoints.Add(leftNode1);
        spawnPoints.Add(leftNode2);
        spawnPoints.Add(leftNode3);
        spawnPoints.Add(leftNode4);
        spawnPoints.Add(leftNode5);
        spawnPoints.Add(leftNode6);
        spawnPoints.Add(leftNode7);
        spawnPoints.Add(rightNode1);
        spawnPoints.Add(rightNode2);
        spawnPoints.Add(rightNode3);
        spawnPoints.Add(rightNode4);
        spawnPoints.Add(rightNode5);
        spawnPoints.Add(rightNode6);
        spawnPoints.Add(rightNode7);

        leftNode1.transform.Find("Cube").gameObject.SetActive(false);
        leftNode2.transform.Find("Cube").gameObject.SetActive(false);
        leftNode3.transform.Find("Cube").gameObject.SetActive(false);
        leftNode4.transform.Find("Cube").gameObject.SetActive(false);
        leftNode5.transform.Find("Cube").gameObject.SetActive(false);
        leftNode6.transform.Find("Cube").gameObject.SetActive(false);
        leftNode7.transform.Find("Cube").gameObject.SetActive(false);
        rightNode1.transform.Find("Cube").gameObject.SetActive(false);
        rightNode2.transform.Find("Cube").gameObject.SetActive(false);
        rightNode3.transform.Find("Cube").gameObject.SetActive(false);
        rightNode4.transform.Find("Cube").gameObject.SetActive(false);
        rightNode5.transform.Find("Cube").gameObject.SetActive(false);
        rightNode6.transform.Find("Cube").gameObject.SetActive(false);
        rightNode7.transform.Find("Cube").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetGamePaused() == false)
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                if (GetComponent<MatchHandler>().GetPersonCount() < maxInstances)
                {
                    if (GetComponent<MatchHandler>().GetPersonCount() < 6)
                    {
                        spawnTimer = Random.Range(0.75f, 1.5f);
                    }
                    else
                    {
                        spawnTimer = Random.Range(1f, 2f);
                    }

                    SpawnPersonRandom();
                }
            }
        }
    }

    public GameObject SpawnMatchSeeker()
    {
        GameObject instance = Instantiate(personObject, instancePlane.transform);
        instance.transform.position = matchSpawnNode.transform.position;
        instance.GetComponent<PersonScript>().SetPosition(0, 3);
        return instance;
    }

    public GameObject SpawnPersonRandom()
    {
        GameObject instance = Instantiate(personObject, instancePlane.transform);

        int pos = Mathf.FloorToInt(Random.Range(0, spawnPoints.Count - 0.01f));
        instance.transform.position = spawnPoints[pos].transform.position;

        switch (pos)
        {
            case 0:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 0);
                break;

            case 1:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 1);
                break;

            case 2:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 2);
                break;

            case 3:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 5);
                break;

            case 4:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 6);
                break;

            case 5:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 7);
                break;

            case 6:
                instance.GetComponent<PersonScript>().SetRandomPosition(0, 8);
                break;

            case 7:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 0);
                break;

            case 8:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 1);
                break;

            case 9:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 2);
                break;

            case 10:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 5);
                break;

            case 11:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 6);
                break;

            case 12:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 7);
                break;

            case 13:
                instance.GetComponent<PersonScript>().SetRandomPosition(15, 8);
                break;
        }

        GetComponent<MatchHandler>().AddPerson(instance);

        return instance;
    }

    public GameObject SpawnPersonFromDespair(Vector3 spawnVector, int x, int z, int i)
    {
        GameObject instance = Instantiate(personObject, instancePlane.transform);
        instance.transform.position = spawnVector;
        instance.GetComponent<PersonScript>().SetRandomPosition(x, z);
        instance.GetComponent<PersonScript>().SetInterest(i);
        instance.GetComponent<PersonScript>().SetKnownDespairStatus(true);
        GetComponent<MatchHandler>().AddPerson(instance);
        return instance;
    }

    public GameObject SpawnDespair(Vector3 spawnVector, int x, int z, int i)
    {
        GameObject instance = Instantiate(despairObject, instancePlane.transform);
        instance.transform.position = spawnVector;
        instance.GetComponent<DespairScript>().SetPosition(x, z);
        instance.GetComponent<DespairScript>().SetInterest(i);
        GetComponent<MatchHandler>().AddPerson(instance);
        return instance;
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
