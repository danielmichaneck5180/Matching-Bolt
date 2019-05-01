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

    private void Start()
    {
        GameObject[] nodeList = GameObject.FindGameObjectsWithTag("Node");
        GameObject[] obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");

        for (int i = 0; i < nodeList.Length; i++)
        {
            for (int p = 0; p < obstacleList.Length; p++)
            {
                //Debug.Log(Vector3.Distance(nodeList[i].transform.position, obstacleList[p].transform.position));
                if (Vector3.Distance(nodeList[i].transform.position, obstacleList[p].transform.position) < 9)
                {
                    //Debug.Log("OI");
                    nodeList[i].GetComponent<NodeScript>().SetEnabled(false);
                }
            }
        }
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
            GameObject returnNode = grid[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)];
            if (returnNode.GetComponent<NodeScript>().GetEnabled() == false)
            {
                returnNode = null;
            }
            return returnNode;
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
                if (path.Count > 39)
                {
                    Debug.Log("ERROR: COULD NOT FIND PATH AFTER " + path.Count + " STEPS. Target vector: " + endVector.x + " " + endVector.y + " Current vector: " + vector.x + " " + vector.y);
                    return path;
                }

                path.Add(vector);

                if (vector == endVector)
                {
                    //Debug.Log("SUCCESS: FOUND PATH AFTER " + path.Count + " STEPS");
                    return path;
                }
                else
                {
                    // Gets reference to surrounding vectors
                    //List<Vector2> nodeVectors = grid.GetSurroundingVectors(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
                    // Creates vector list of potential nodes to move to next
                    List<Vector2> potentialVectors = new List<Vector2>();

                    // Calculates optimal direction of movement
                    int signX = 0;

                    if (vector.x - endVector.x != 0)
                    {
                        signX = Mathf.RoundToInt(Mathf.Sign(vector.x - endVector.x));
                    }

                    int signY = 0;

                    if (vector.y - endVector.y != 0)
                    {
                        signY = Mathf.RoundToInt(Mathf.Sign(vector.y - endVector.y)); ;
                    }

                    // Checks if most optimal node is available
                    if (grid.GetNode(new Vector2(vector.x - signX, vector.y - signY)) != null)
                    {
                        path = Loop(path, new Vector2(vector.x - signX, vector.y - signY));
                    }
                    // Checks if less optimal nodes are available
                    else
                    {
                        if (signX < 0)
                        {
                            if (signY < 0)
                            {
                                // Middle right
                                if (grid.GetNode(new Vector2(vector.x + 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x + 1, vector.y));
                                }
                                // Top center
                                else if (grid.GetNode(new Vector2(vector.x, vector.y + 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y + 1));
                                }
                                // Bottom center
                                else if (grid.GetNode(new Vector2(vector.x, vector.y - 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y - 1));
                                }
                                // Middle left
                                else if (grid.GetNode(new Vector2(vector.x - 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x - 1, vector.y));
                                }
                            }
                            else
                            {
                                // Bottom center
                                if (grid.GetNode(new Vector2(vector.x, vector.y - 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y - 1));
                                }
                                // Middle right
                                else if (grid.GetNode(new Vector2(vector.x + 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x + 1, vector.y));
                                }
                                // Middle left
                                else if (grid.GetNode(new Vector2(vector.x - 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x - 1, vector.y));
                                }
                                // Top center
                                else if (grid.GetNode(new Vector2(vector.x, vector.y + 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y + 1));
                                }
                            }
                        }
                        else
                        {
                            if (signY < 0)
                            {
                                // Top center
                                if (grid.GetNode(new Vector2(vector.x, vector.y + 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y + 1));
                                }
                                // Middle left
                                else if (grid.GetNode(new Vector2(vector.x - 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x - 1, vector.y));
                                }
                                // Middle right
                                else if (grid.GetNode(new Vector2(vector.x + 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x + 1, vector.y));
                                }
                                // Bottom center
                                else if (grid.GetNode(new Vector2(vector.x, vector.y - 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y - 1));
                                }
                            }
                            else
                            {
                                // Middle left
                                if (grid.GetNode(new Vector2(vector.x - 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x - 1, vector.y));
                                }
                                // Bottom center
                                else if (grid.GetNode(new Vector2(vector.x, vector.y - 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y - 1));
                                }
                                // Top center
                                else if (grid.GetNode(new Vector2(vector.x, vector.y + 1)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x, vector.y + 1));
                                }
                                // Middle right
                                else if (grid.GetNode(new Vector2(vector.x + 1, vector.y)) != null)
                                {
                                    path = Loop(path, new Vector2(vector.x + 1, vector.y));
                                }
                            }
                        }
                    }

                    return path;

                    /*
                    else
                    {
                        potentialVectors = GetVectors(Mathf.RoundToInt(vector.x + signX), Mathf.RoundToInt(vector.y + signY));

                        bool searching = true;

                        for (int i = 0; i < potentialVectors.Count; i++)
                        {
                            if (searching == true)
                            {
                                if (grid.GetNode(potentialVectors[i]) != null)
                                {
                                    path = Loop(path, potentialVectors[i]);

                                    if (path.Count < 40)
                                    {
                                        searching = false;
                                    }
                                }
                            }
                        }

                        if (searching == true)
                        {
                            Debug.Log("ERROR: COULD NOT FIND PATH BECAUSE OF NO AVAILABLE NODES");
                        }
                        */
                }
            }

            public List<Vector2> GetPath()
            {
                return pathVectors;
            }

            private List<Vector2> GetVectors(int x, int z)
            {
                // x and z represent the preferred order of vectors, the direction it is moving in
                List<Vector2> returnList = new List<Vector2>();
                int[] order = new int[8];

                if (x > 0)
                {
                    // Top right
                    if (z > 0)
                    {
                        order = new int[] { 0, 3, 1, 5, 2, 6, 4, 7 };
                    }
                    // Bottom right
                    else if (z < 0)
                    {
                        order = new int[] { 5, 6, 3, 7, 0, 4, 1, 2 };
                    }
                    // Middle right
                    else
                    {
                        order = new int[] { 3, 5, 0, 6, 1, 7, 2, 4 };
                    }
                }
                else if (x < 0)
                {
                    // Top left
                    if (z > 0)
                    {
                        order = new int[] { 2, 1, 4, 0, 7, 3, 6, 5 };
                    }
                    // Bottom left
                    else if (z < 0)
                    {
                        order = new int[] { 7, 4, 5, 2, 5, 1, 3, 0 };
                    }
                    // Middle left
                    else
                    {
                        order = new int[] { 4, 2, 7, 1, 6, 0, 5, 3 };
                    }
                }
                else
                {
                    // Top center
                    if (z > 0)
                    {
                        order = new int[] { 1, 0, 2, 3, 4, 5, 7, 6 };
                    }
                    // Bottom center
                    else //if (z < 0)
                    {
                        order = new int[] { 6, 7, 5, 4, 3, 2, 0, 1 };
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    returnList.Add(GetVector(x, z, order[i]));
                }

                if (x < 1)
                {
                    Debug.Log("Failed x: " + GetVector(x, z, 2).x);
                    returnList.Remove(GetVector(x, z, 2));
                    returnList.Remove(GetVector(x, z, 4));
                    returnList.Remove(GetVector(x, z, 7));

                    if (x < 0)
                    {
                        returnList.Remove(GetVector(x, z, 1));
                        returnList.Remove(GetVector(x, z, 6));

                        if (x < -1)
                        {
                            returnList.Clear();
                        }
                    }
                }

                if (x + 2 > grid.width)
                {
                    returnList.Remove(GetVector(x, z, 0));
                    returnList.Remove(GetVector(x, z, 3));
                    returnList.Remove(GetVector(x, z, 5));

                    if (x + 1 > grid.width)
                    {
                        returnList.Remove(GetVector(x, z, 1));
                        returnList.Remove(GetVector(x, z, 6));

                        if (x > grid.width)
                        {
                            returnList.Clear();
                        }
                    }
                }

                if (z < 1)
                {
                    returnList.Remove(GetVector(x, z, 5));
                    returnList.Remove(GetVector(x, z, 6));
                    returnList.Remove(GetVector(x, z, 7));

                    if (z < 0)
                    {
                        returnList.Remove(GetVector(x, z, 3));
                        returnList.Remove(GetVector(x, z, 4));

                        if (z < -1)
                        {
                            returnList.Clear();
                        }
                    }
                }

                if (z + 2 > grid.height)
                {
                    returnList.Remove(GetVector(x, z, 0));
                    returnList.Remove(GetVector(x, z, 1));
                    returnList.Remove(GetVector(x, z, 2));

                    if (z + 1 > grid.height)
                    {
                        returnList.Remove(GetVector(x, z, 3));
                        returnList.Remove(GetVector(x, z, 4));

                        if (z > grid.height)
                        {
                            returnList.Clear();
                        }
                    }
                }

                return returnList;
            }

            private Vector2 GetVector(int x, int z, int i)
            {
                switch (i)
                {
                    // Top right
                    case 0:
                        return new Vector2(x + 1, z + 1);

                    // Top center
                    case 1:
                        return new Vector2(x, z + 1);

                    // Top left
                    case 2:
                        return new Vector2(x - 1, z + 1);

                    // Middle right
                    case 3:
                        return new Vector2(x + 1, z);

                    // Middle left
                    case 4:
                        return new Vector2(x - 1, z);

                    // Bottom right
                    case 5:
                        return new Vector2(x + 1, z - 1);

                    // Bottom center
                    case 6:
                        return new Vector2(x, z - 1);

                    // Bottom left
                    default:
                        return new Vector2(x - 1, z - 1);
                }
            }
        };
    };
}
