using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    public GameObject node;
    public int gridWidth;
    public int gridHeight;
    public float gridCellSize;

    private List<GameObject> nodeList;
    private List<Vector2> vectorList;

    private void Awake()
    {
        nodeList = new List<GameObject>();
        vectorList = new List<Vector2>();

        float positionX = transform.position.x - (gridCellSize * (gridWidth / 2));
        float positionZ = transform.position.z - (gridCellSize * (gridHeight / 2));

        GameObject spawnedNode;

        for (int i = 0; i < gridWidth; i++)
        {
            for (int p = 0; p < gridHeight; p++)
            {
                spawnedNode = Instantiate(node, transform);
                spawnedNode.transform.Translate(new Vector3(positionX + (i * gridCellSize), 0, positionZ + (p * gridCellSize)));
                spawnedNode.GetComponent<NodeScript>().SetNode(i, p);
                nodeList.Add(spawnedNode);
            }
        }

        CreateVectorList();
    }

    private void CreateVectorList()
    {
        for(int i = 0; i < nodeList.Count; i++)
        {
            vectorList.Add(new Vector2(nodeList[i].GetComponent<NodeScript>().GetXPosition(), nodeList[i].GetComponent<NodeScript>().GetZPosition()));
        }
    }

    private Vector2 GetNodeVector(int x, int z)
    {
        Vector2 returnVector = new Vector2();

        if (nodeList[(gridHeight * x) + z].GetComponent<NodeScript>().GetEnabled() == true)
        {
            returnVector = new Vector2(nodeList[(gridHeight * x) + z].GetComponent<NodeScript>().GetXPosition(), nodeList[(gridHeight * x) + z].GetComponent<NodeScript>().GetZPosition());
        }
        else
        {
            returnVector = new Vector2(-1, -1);
        }

        return returnVector;
    }

    private List<Vector2> GetAvailabeNodes(int x, int z)
    {
        List<Vector2> returnList = new List<Vector2>();
        Vector2 newVector = new Vector2();

        if (x > 0)
        {
            newVector = GetNodeVector(x - 1, z);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }

            if (z > 0)
            {
                newVector = GetNodeVector(x - 1, z - 1);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }

            if (z < gridWidth - 2)
            {
                newVector = GetNodeVector(x - 1, z + 1);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }
        }

        if (x < gridWidth - 2)
        {
            newVector = GetNodeVector(x + 1, z);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }

            if (z > 0)
            {
                newVector = GetNodeVector(x + 1, z - 1);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }

            if (z < nodeList.Count / gridWidth - 1)
            {
                newVector = GetNodeVector(x + 1, z + 1);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }
        }

        if (z > 0)
        {
            newVector = GetNodeVector(x, z - 1);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }
        }

        if (z < gridHeight - 2)
        {
            newVector = GetNodeVector(x, z + 1);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }
        }

        Debug.Log("returnList.Count: " + returnList.Count.ToString());
        return returnList;
    }

    private List<Vector2> PathfindLoop(List<Vector2> vectors, Vector2 thisVector, Vector2 targetVector, int steps, int targetSteps, out int returnSteps, out bool targetFound)
    {
        vectors.Add(thisVector);
        int newSteps = steps++;
        targetFound = false;

        if (newSteps < targetSteps)
        {
            if (thisVector == targetVector)
            {
                Debug.Log("Found it!");
                returnSteps = newSteps;
                targetFound = true;
                return vectors;
            }

            if (newSteps < targetSteps - 1)
            {
                int newReturnSteps = 1000;
                bool found = false;

                List<Vector2> pathList = GetAvailabeNodes(Mathf.RoundToInt(thisVector.x), Mathf.RoundToInt(thisVector.y));

                for (int i = 0; i < pathList.Count; i++)
                {
                    if (vectors.Contains(pathList[i]) == false)
                    {
                        vectors = PathfindLoop(vectors, pathList[i], targetVector, newSteps, targetSteps, out newReturnSteps, out found);

                        if (found == true)
                        {
                            if (newReturnSteps < targetSteps)
                            {
                                targetSteps = newReturnSteps;
                                Debug.Log("Stopped " + pathList[i].x.ToString() + " " + pathList[i].y.ToString());
                                i = pathList.Count;
                            }
                            else
                            {
                                vectors.Remove(pathList[i]);
                            }
                        }
                    }
                }

                returnSteps = newSteps;
                return vectors;
            }
        }
        
        returnSteps = newSteps;
        return vectors;
    }

    public void DisableNode(int x, int z)
    {
        nodeList[(gridHeight * x) + z].GetComponent<NodeScript>().SetEnabled(false);
        Debug.Log(x.ToString() + " " + z.ToString() + " " + nodeList[(gridHeight * x) + z].GetComponent<NodeScript>().GetXPosition().ToString() + " " + nodeList[(gridHeight * x) + z].GetComponent<NodeScript>().GetZPosition().ToString());
    }

    public void PathfindToObject(int x, int z, GameObject seekObject)
    {
        Pathfind(x, z, seekObject.GetComponent<NodeScript>().GetXPosition(), seekObject.GetComponent<NodeScript>().GetZPosition());
    }

    public List<Vector2> Pathfind(int x, int z, int seekX, int seekZ)
    {
        Debug.Log("Pathfind string " + x.ToString() + " " + z.ToString() + " " + seekX.ToString() + " " + seekZ.ToString());
        List<Vector2> path = PathfindLoop(new List<Vector2>(), new Vector2(x, z), new Vector2(seekX, seekZ), 0, 1000, out int steps, out bool found);
        return path;
    }

    public GameObject GetNode(Vector2 nodeVector)
    {
        Debug.Log("GetNode: " + Mathf.RoundToInt((gridHeight * nodeVector.x) + nodeVector.y).ToString() + " nodeList.Count: " + nodeList.Count);

        return nodeList[Mathf.RoundToInt((gridHeight * nodeVector.x) + nodeVector.y)];
    }
}
