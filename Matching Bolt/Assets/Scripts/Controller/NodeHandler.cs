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
    private Grid grid;

    private int iterations;

    private void Awake()
    {
        iterations = 0;
        /*
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
        */
        grid = new Grid(gridWidth, gridHeight, gridCellSize, node, transform);
    }

    private Vector2 GetNodeVector(int x, int z)
    {
        Vector2 returnVector = new Vector2(-1, -1);

        GameObject tempNode = nodeList[(gridHeight * x) + z].gameObject;

        if (tempNode.GetComponent<NodeScript>().GetEnabled() == true)
        {
            returnVector = new Vector2(tempNode.GetComponent<NodeScript>().GetXPosition(), tempNode.GetComponent<NodeScript>().GetZPosition());
        }
        
        return returnVector;
    }

    private List<Vector2> GetAvailabeNodes(int x, int z)
    {
        List<Vector2> returnList = new List<Vector2>();
        Vector2 newVector = new Vector2();

        //Debug.Log("Start x: " + x + " z: " + z);

        if (x > 0)
        {
            newVector = GetNodeVector(x - 1, z);

            //Debug.Log("x: " + newVector.x + " z: " + newVector.y);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }

            if (z > 0)
            {
                newVector = GetNodeVector(x - 1, z - 1);

                //Debug.Log("x: " + newVector.x + " z: " + newVector.y);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }

            if (z < gridHeight - 2)
            {
                newVector = GetNodeVector(x - 1, z + 1);

                //Debug.Log("x: " + newVector.x + " z: " + newVector.y);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }
        }

        if (x < gridWidth - 2)
        {
            newVector = GetNodeVector(x + 1, z);

            //Debug.Log("x: " + newVector.x + " z: " + newVector.y);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }

            if (z > 0)
            {
                newVector = GetNodeVector(x + 1, z - 1);

                //Debug.Log("x: " + newVector.x + " z: " + newVector.y);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }

            if (z < gridHeight - 2)
            {
                newVector = GetNodeVector(x + 1, z + 1);

                //.Log("x: " + newVector.x + " z: " + newVector.y);

                if (newVector != new Vector2(-1, -1))
                {
                    returnList.Add(newVector);
                }
            }
        }

        if (z > 0)
        {
            newVector = GetNodeVector(x, z - 1);

            //Debug.Log("x: " + newVector.x + " z: " + newVector.y);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }
        }

        if (z < gridHeight - 2)
        {
            newVector = GetNodeVector(x, z + 1);

            //.Log("x: " + newVector.x + " z: " + newVector.y);

            if (newVector != new Vector2(-1, -1))
            {
                returnList.Add(newVector);
            }
        }

        //Debug.Log("returnList.Count: " + returnList.Count.ToString());
        return returnList;
    }

    private Path PathPathFindLoop(Path previousPath, Vector2 thisVector, Vector2 targetVector, bool first)
    {
        iterations++;
        // Sets up path
        Path thisPath = previousPath;
        thisPath.AddVector(thisVector);

        //Debug.Log("Steps: " + thisPath.GetSteps());

        // Check if it has found its target
        if (thisVector == targetVector)
        {
            thisPath.SetFoundTarget(true);
            return thisPath;
        }
        else
        {
            // Sets up list of nearby nodes
            //List<Vector2> nodeList = GetAvailabeNodes(Mathf.RoundToInt(thisVector.x), Mathf.RoundToInt(thisVector.y));
            List<Vector2> nodeList = grid.GetSurroundingVectors(Mathf.RoundToInt(thisVector.x), Mathf.RoundToInt(thisVector.y));

            // Loops through all nearby nodes
            List<Path> pathList = new List<Path>();
            
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (thisPath.CheckForVector(nodeList[i]) == false)
                {
                    pathList.Add(PathPathFindLoop(thisPath, nodeList[i], targetVector, false));
                    
                }
            }

            /*
            for (int p = 0; p < pathList.Count; p++)
            {
                if (pathList[p].GetFoundTarget() == true)
                {
                    Debug.Log(p);
                }
            }
            */

            //Debug.Log(iterations);
            
            // Checks paths to find best route
            int targetSteps = 1000;

            for (int i = 0; i < pathList.Count; i++)
            {
                if (pathList[i].GetFoundTarget() == true)
                {
                    if (pathList[i].GetSteps() < targetSteps)
                    {
                        thisPath = pathList[i];
                        targetSteps = pathList[i].GetSteps();
                        Debug.Log("1: " + i + " Steps: " + targetSteps);
                    }
                }
            }

            if (first == true)
            {
                for (int i = 0; i < pathList.Count; i++)
                {
                    Debug.Log("I: " + i + " Steps: " + pathList[i].GetSteps());
                }
            }

            return thisPath;
        }
    }

    private List<Vector2> PathfindLoop(List<Vector2> vectors, Vector2 thisVector, Vector2 targetVector, int steps, int targetSteps, out int returnSteps, out bool targetFound)
    {
        vectors.Add(thisVector);
        int newSteps = steps + 1;
        Debug.Log("Steps: " + steps.ToString());
        targetFound = false;
        List<Vector2> returnVectors = vectors;

        if (newSteps < targetSteps)
        {
            if (thisVector == targetVector)
            {
                Debug.Log("Found it!");
                Debug.Log("New return steps: " + newSteps.ToString());
                returnSteps = newSteps;
                targetFound = true;
                return vectors;
            }

            if (newSteps < targetSteps - 1)
            {
                int newReturnSteps = 1000;
                bool found = false;
                List<Vector2> vector2s = new List<Vector2>();

                List<Vector2> pathList = GetAvailabeNodes(Mathf.RoundToInt(thisVector.x), Mathf.RoundToInt(thisVector.y));

                for (int i = 0; i < pathList.Count; i++)
                {
                    newReturnSteps = 1000;
                    found = false;
                    vector2s = new List<Vector2>();

                    if (vectors.Contains(pathList[i]) == false)
                    {
                        vector2s = PathfindLoop(vectors, pathList[i], targetVector, newSteps, targetSteps, out newReturnSteps, out found);

                        if (found == true)
                        {
                            if (newReturnSteps < targetSteps)
                            {
                                returnVectors = vector2s;
                                targetSteps = newReturnSteps;
                                targetFound = true;
                                //Debug.Log("Stopped " + pathList[i].x.ToString() + " " + pathList[i].y.ToString());
                                i = pathList.Count;
                            }
                        }
                    }
                }

                returnSteps = newSteps;
                return returnVectors;
            }
        }
        
        returnSteps = newSteps;
        return returnVectors;
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
        /*
        Debug.Log("Pathfind string " + x.ToString() + " " + z.ToString() + " " + seekX.ToString() + " " + seekZ.ToString());
        int steps = 10000;
        List<Vector2> path = PathfindLoop(new List<Vector2>(), new Vector2(x, z), new Vector2(seekX, seekZ), 0, 1000, out steps, out bool found);
        */
        /*
        List<Vector2> returnVectors = PathPathFindLoop(new Path(new List<Vector2>(), false), new Vector2(x, z), new Vector2(seekX, seekZ), true).GetVectors();
        for (int i = 0; i < returnVectors.Count; i++)
        {
            if (returnVectors[i].x == 6 && returnVectors[i].y == 6)
            {
                Debug.Log("Vector position X: " + returnVectors[i].x + " Y: " + returnVectors[i].y);
            }
        }
        Debug.Log("Iterations: " + iterations);
        return returnVectors;
        */
        return grid.FindPath(new Vector2(x, z), new Vector2(seekX, seekZ));
    }

    public GameObject GetNode(Vector2 nodeVector)
    {
        //Debug.Log("GetNode: " + Mathf.RoundToInt((gridHeight * nodeVector.x) + nodeVector.y).ToString() + " nodeList.Count: " + nodeList.Count);

        //return nodeList[Mathf.RoundToInt((gridHeight * nodeVector.x) + nodeVector.y)];

        return grid.GetNode(nodeVector);
    }

    private class Path
    {
        private List<Vector2> vectorList;
        private bool foundTarget;

        public Path(List<Vector2> vs, bool f)
        {
            vectorList = vs;
            foundTarget = f;
        }

        public List<Vector2> GetVectors()
        {
            return vectorList;
        }

        public bool GetFoundTarget()
        {
            return foundTarget;
        }

        public int GetSteps()
        {
            return vectorList.Count;
        }

        public void AddVector(Vector2 newVector)
        {
            vectorList.Add(newVector);
        }

        public void SetFoundTarget(bool newFoundTarget)
        {
            foundTarget = newFoundTarget;
        }

        public bool CheckForVector(Vector2 checkVector)
        {
            bool returnBool = false;

            for (int i = 0; i < vectorList.Count; i++)
            {
                if (vectorList[i] == checkVector)
                {
                    returnBool = true;
                }
            }

            return returnBool;
        }
    };

    private class Grid
    {
        private readonly GameObject[,] grid;
        private readonly int width;
        private readonly int height;

        public Grid(int w, int h, float size, GameObject nodeReference, Transform transformReference)
        {
            width = w;
            height = h;
            grid = CreateGrid(size, nodeReference, transformReference);
        }
        
        private GameObject[,] CreateGrid(float size, GameObject nodeReference, Transform transformReference)
        {
            GameObject[,] returnGrid = new GameObject[width, height];
            GameObject tempNode;

            for (int i = 0; i < width; i++)
            {
                for (int p = 0; p < height; p++)
                {
                    tempNode = Instantiate(nodeReference, transformReference);
                    tempNode.transform.Translate(new Vector3((width * size / 2) - (i * size), 0, (height * size / 2) - (p * size)), Space.Self);
                    tempNode.GetComponent<NodeScript>().SetNode(i, p);
                    returnGrid[i, p] = tempNode;
                }
            }

            return returnGrid;
        }

        public List<Vector2> FindPath(Vector2 startVector, Vector2 endVector)
        {
            List<Vector2> path = new List<Vector2>();

            if (startVector == endVector)
            {
                path.Add(startVector);
                return path;
            }

            Pathfinder pathfinder = new Pathfinder(startVector, endVector, this);

            return pathfinder.GetPath();
        }

        public List<Vector2> GetSurroundingVectors(int x, int z)
        {
            List<Vector2> returnVectors = new List<Vector2>();

            if (x > 0)
            {
                returnVectors.Add(new Vector2(x - 1, z));

                if (z > 0)
                {
                    returnVectors.Add(new Vector2(x - 1, z - 1));
                }

                if (z < height - 1)
                {
                    returnVectors.Add(new Vector2(x - 1, z + 1));
                }
            }

            if (x < width -1)
            {
                returnVectors.Add(new Vector2(x + 1, z));

                if (z > 0)
                {
                    returnVectors.Add(new Vector2(x + 1, z - 1));
                }

                if (z < height - 1)
                {
                    returnVectors.Add(new Vector2(x + 1, z + 1));
                }
            }

            if (z > 0)
            {
                returnVectors.Add(new Vector2(x, z - 1));
            }

            if (z < height - 1)
            {
                returnVectors.Add(new Vector2(x, z + 1));
            }

            return returnVectors;
        }

        public GameObject GetNode(Vector2 vector)
        {
            return grid[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)];
        }

        private class Pathfinder
        {
            private readonly Vector2 startVector;
            private readonly Vector2 endVector;
            private List<Vector2> pathVectors;
            private Grid grid;

            public Pathfinder(Vector2 v1, Vector2 v2, Grid g)
            {
                startVector = v1;
                endVector = v2;
                pathVectors = new List<Vector2>();
                grid = g;
                Pathfind();
            }

            private void Pathfind()
            {
                List<Vector2> initList = new List<Vector2>();
                pathVectors = Loop(initList, startVector);
            }

            private List<Vector2> Loop(List<Vector2> path, Vector2 vector)
            {
                if (path.Count > 40)
                {
                    Debug.Log("ERROR: COULD NOT FIND PATH");
                    return path;
                }

                path.Add(vector);

                if (vector == endVector)
                {
                    return path;
                }
                else
                {
                    int signX = Mathf.RoundToInt(Mathf.Sign(vector.x - endVector.x));

                    if (vector.x - endVector.x == 0)
                    {
                        signX = 0;
                    }

                    int signY = Mathf.RoundToInt(Mathf.Sign(vector.y - endVector.y));

                    if (vector.y - endVector.y == 0)
                    {
                        signY = 0;
                    }

                    path = Loop(path, new Vector2(vector.x - signX, vector.y - signY));
                }

                return path;
            }

            public List<Vector2> GetPath()
            {
                return pathVectors;
            }
        };
    };
}
