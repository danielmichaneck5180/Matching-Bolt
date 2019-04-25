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
        vectorList = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().Pathfind(0, 0, 15, 8);
        Debug.Log("List size " + vectorList.Count);
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 1f)
        {
            if (i < vectorList.Count - 1)
            {
                i++;
                SetCurrentNode(i);
            }
        }
        else
        {
            Vector3 newPos = currentNode.transform.position - transform.position;
            newPos.Normalize();
            transform.Translate(newPos / 2, Space.Self);
        }
    }

    private void SetCurrentNode(int node)
    {
        currentNode = GameObject.FindGameObjectWithTag("Node Spawner").GetComponent<NodeHandler>().GetNode(vectorList[node]);
        //Debug.Log(currentNode.GetComponent<NodeScript>().GetXPosition() + " " + currentNode.GetComponent<NodeScript>().GetZPosition());
    }
}
