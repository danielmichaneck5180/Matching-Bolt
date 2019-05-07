using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    public GameObject node;
    public int gridWidth;
    public int gridHeight;
    public float gridCellSize;
    public int pathfindingAccuracy;

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
                if (Vector3.Distance(nodeList[i].transform.position, obstacleList[p].transform.position) < obstacleList[p].GetComponent<ObstacleScript>().GetObstacleSize())
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

            if (GetNode(startVector) == null || GetNode(endVector) == null || startVector == endVector)
            {
                if (GetNode(startVector) == null)
                {
                    //Debug.Log("ERROR: START");
                }
                else if (GetNode(endVector) == null)
                {
                    //Debug.Log("ERROR: END");
                }
                else if (startVector == endVector)
                {
                    //Debug.Log("ERROR: SAME");
                }
                path.Add(startVector);
                return path;
            }

            Pathfinder pathfinder = new Pathfinder(startVector, endVector, this);
            if (pathfinder.GetPath().Count < 40)
            {
                //Debug.Log("Path okay " + pathfinder.GetPath().Count);
            }
            else
            {
                Debug.LogError("Path faulty " + pathfinder.GetPath().Count);
            }
            for (int i = 0; i < pathfinder.GetPath().Count; i++)
            {
                if (GetNode(pathfinder.GetPath()[i]) == null)
                {
                    Debug.LogError("How did this happen???!!!");
                }
            }

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
            if (vector.x < 0 || vector.x > width - 1 || vector.y < 0 || vector.y > height - 1)
            {
                return null;
            }

            GameObject returnNode = grid[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)];

            if (returnNode.GetComponent<NodeScript>().GetEnabled() == false)
            {
                return null;
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
                RefinePath();
            }

            private void Pathfind()
            {
                List<Vector2> initList = new List<Vector2>();
                pathVectors = PathFindLoop(initList, startVector);
            }

            private List<Vector2> PathFindLoop(List<Vector2> path, Vector2 vector)
            {
                if (grid.GetNode(vector) == null)
                {
                    Debug.LogError("But how?");
                }

                path.Add(vector);

                if (vector == endVector)
                {
                    //Debug.Log("SUCCESS: FOUND PATH AFTER " + path.Count + " STEPS");
                    return path;
                }
                else
                {
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
                    if (CheckAvailableNode(new Vector2(vector.x - signX, vector.y - signY), path) == 0)
                    {
                        path = PathFindLoop(path, new Vector2(vector.x - signX, vector.y - signY));
                    }
                    else if (CheckAvailableNode(new Vector2(vector.x - signX, vector.y - signY), path) == 1)
                    {

                    }
                    else for (float i = 1; i < 9; i++)
                        {
                            int e = GetElement(signX, signY);

                            if (i / 2f == Mathf.RoundToInt(i / 2f))
                            {
                                e += Mathf.RoundToInt(i / 2);
                            }
                            else
                            {
                                e += Mathf.RoundToInt((((i - 1) / 2) * -1) - 1);
                            }

                            if (e < 0)
                            {
                                e += 8;
                            }
                            else if ( e > 7)
                            {
                                e -= 8;
                            }
                        
                            if (CheckAvailableNode(new Vector2(vector.x - GetDirection(e)[0], vector.y - GetDirection(e)[1]), path) == 0)
                            {
                                path = PathFindLoop(path, new Vector2(vector.x - GetDirection(e)[0], vector.y - GetDirection(e)[1]));
                            }
                            else if (CheckAvailableNode(new Vector2(vector.x - GetDirection(e)[0], vector.y - GetDirection(e)[1]), path) == 1)
                            {
                                i = 8;
                            }

                            //Debug.Log("E: " + e + " element: " + GetElement(signX, signY) + " i: " + i);
                            
                            if (path[path.Count - 1] == endVector)
                            {
                                i = 8;
                            }
                        }
                    
                    return path;
                }
            }

            private int CheckAvailableNode(Vector2 vector, List<Vector2> previousVectors)
            {
                if (grid.GetNode(vector) == null)
                {
                    return -1;
                }
                else if (previousVectors.Contains(vector) == true)
                {
                    return -1;
                }

                return 0;
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
                //Debug.Log("Element: " + element);
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

            private void RefinePath()
            {
                //Debug.Log("path size 1: " + pathVectors.Count);
                PathRefineLoopBackward(0);
                //Debug.Log("path size 2: " + pathVectors.Count);
                SmoothPath();
                //Debug.Log("path size 3: " + pathVectors.Count);
                for (int i = 0; i < 2; i++)
                {
                    PathRefineLoopForward(0);
                }
                //Debug.Log("path size 4: " + pathVectors.Count);
                SmoothPath();
                //Debug.Log("path size 5: " + pathVectors.Count);
            }

            private bool CheckPaths(List<Vector2> list1, List<Vector2> list2)
            {
                if (list1.Count == list2.Count)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        if (list1[i] != list2[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            private void PathRefineLoopBackward(int start)
            {
                if (start < pathVectors.Count - 1)
                {
                    for (int i = pathVectors.Count - 1; i >= start; i--)
                    {
                        if (i < pathVectors.Count)
                        {
                            if (CheckPossiblePath(pathVectors[start], pathVectors[i], 0) == true)
                            {
                                for (int p = i - 1; p > start; p--)
                                {
                                    pathVectors.Remove(pathVectors[p]);
                                }
                            }
                        }
                    }

                    PathRefineLoopBackward(start + 1);
                }
            }

            private void SmoothPath()
            {
                List<Vector2> newPath = new List<Vector2>();
                List<Vector2> addList;

                for (int i = 0; i < pathVectors.Count - 1; i++)
                {
                    addList = GetPossiblePath(pathVectors[i], pathVectors[i + 1], new List<Vector2>());

                    for (int p = 0; p < addList.Count; p++)
                    {
                        newPath.Add(addList[p]);
                    }
                }

                for (int i = 0; i < newPath.Count; i++)
                {
                    for (int p = 0; p < newPath.Count; p++)
                    {
                        if (i != p)
                        {
                            if (newPath[i] == newPath[p])
                            {
                                newPath.Remove(newPath[p]);
                            }
                        }
                    }
                }

                pathVectors = newPath;
            }

            private void PathRefineLoopForward(int start)
            {
                if (start < pathVectors.Count - 2)
                {
                    if (CheckPossiblePath(pathVectors[start], pathVectors[start + 2], 0) == true)
                    {
                        pathVectors.Remove(pathVectors[start + 1]);
                    }

                    PathRefineLoopForward(start + 1);
                }
            }

            private bool CheckPossiblePath(Vector2 vector1, Vector2 vector2, int steps)
            {
                if (steps > grid.width - 1)
                {
                    return false;
                }

                if (vector1 == vector2)
                {
                    return true;
                }
                else
                {
                    // Calculates optimal direction of movement
                    int signX = 0;

                    if (vector1.x - vector2.x != 0)
                    {
                        signX = Mathf.RoundToInt(Mathf.Sign(vector1.x - vector2.x));
                    }

                    int signY = 0;

                    if (vector1.y - vector2.y != 0)
                    {
                        signY = Mathf.RoundToInt(Mathf.Sign(vector1.y - vector2.y)); ;
                    }

                    // Checks if most optimal node is available
                    if (CheckAvailableNode(new Vector2(vector1.x - signX, vector1.y - signY), new List<Vector2>()) == 0)
                    {
                        return CheckPossiblePath(new Vector2(vector1.x - signX, vector1.y - signY), vector2, steps + 1);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            private List<Vector2> GetPossiblePath(Vector2 vector1, Vector2 vector2, List<Vector2> path)
            {
                path.Add(vector1);

                if (vector1 == vector2)
                {
                    return path;
                }
                else
                {
                    // Calculates optimal direction of movement
                    int signX = 0;

                    if (vector1.x - vector2.x != 0)
                    {
                        signX = Mathf.RoundToInt(Mathf.Sign(vector1.x - vector2.x));
                    }

                    int signY = 0;

                    if (vector1.y - vector2.y != 0)
                    {
                        signY = Mathf.RoundToInt(Mathf.Sign(vector1.y - vector2.y)); ;
                    }

                    return GetPossiblePath(new Vector2(vector1.x - signX, vector1.y - signY), vector2, path);
                }
            }

            public List<Vector2> GetPath()
            {
                return pathVectors;
            }
        };
    };
}
