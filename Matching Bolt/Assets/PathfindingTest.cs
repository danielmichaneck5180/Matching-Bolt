using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    private int x = 0;
    private int z = 0;
    private List<Vector2> vectorList;
    private int i = 0;
    private GameObject currentNode;

    // Start is called before the first frame update
    void Start()
    {
        vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(1, 1, 6, 6);
        Debug.Log("List size " + vectorList.Count);
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 10f)
        {
            if (i < vectorList.Count - 1)
            {
                i++;
                SetCurrentNode(i);
            }
        }
        else
        {
            transform.Translate((currentNode.transform.position - transform.position) / 100, Space.Self);
            Debug.Log(currentNode.transform.position.x);
        }
    }

    private void SetCurrentNode(int node)
    {
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[node]);
    }
}
