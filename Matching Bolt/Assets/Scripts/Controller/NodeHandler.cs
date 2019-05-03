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
        return grid.FindPath(new Vector2(x, z), new Vector2(seekX, seekZ));
    }

    public GameObject GetNode(Vector2 nodeVector)
    {
        if (grid.GetNode(nodeVector) == null)
        {
            Debug.Log("OI: " + nodeVector.x + " " + nodeVector.y);
        }
        else if (grid.GetNode(nodeVector).GetComponent<NodeScript>().GetEnabled() == false)
        {
            Debug.Log("WOW: " + nodeVector.x + " " + nodeVector.y);
        }
        return grid.GetNode(nodeVector);
    }

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
            private int[] directions;
            private Grid grid;

            public Pathfinder(Vector2 v1, Vector2 v2, Grid g)
            {
                startVector = v1;
                endVector = v2;
                pathVectors = new List<Vector2>();
                directions = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
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
                    else for (float i = 1; i < 7; i++)
                        {
                            int e = GetElement(signX, signY);

                            if (i / 2 == Mathf.RoundToInt(i / 2))
                            {
                                e += Mathf.RoundToInt(i / 2);
                            }
                            else
                            {
                                e += Mathf.RoundToInt((((i - 1) / 2) * -1) - 1);
                            }

                            if (e < 0)
                            {
                                e += 7;
                            }
                            else if ( e > 7)
                            {
                                e -= 7;
                            }
                        
                            if (grid.GetNode(new Vector2(vector.x - GetDirection(e)[0], vector.y - GetDirection(e)[1])) != null)
                            {
                                path = Loop(path, new Vector2(vector.x - signX, vector.y - signY));
                            }

                            if (path.Count < 40)
                            {
                                i = 8;
                            }

                            Debug.Log("E: " + e + " element: " + GetElement(signX, signY));
                        }

                    return path;
                }
            }

            private int GetElement(int xDir, int yDir)
            {
                if (xDir > 0)
                {
                    if (yDir > 0)
                    {
                        return 1;
                    }
                    else if (yDir < 0)
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else if (xDir < 0)
                {
                    if (yDir > 0)
                    {
                        return 7;
                    }
                    else if (yDir < 0)
                    {
                        return 5;
                    }
                    else
                    {
                        return 6;
                    }
                }
                else if (yDir > 0)
                {
                    return 0;
                }
                else
                {
                    return 4;
                }
            }

            private int[] GetDirection(int element) 
            {
                switch(element)
                {
                    case 0:
                        return new int[] { 0, 1 };

                    case 1:
                        return new int[] { 1, 1 };

                    case 2:
                        return new int[] { 1, 0 };

                    case 3:
                        return new int[] { 1, -1 };

                    case 4:
                        return new int[] { 0, -1 };

                    case 5:
                        return new int[] { -1, -1 };

                    case 6:
                        return new int[] { -1, 0 };

                    default:
                        return new int[] { -1, 1 };

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
