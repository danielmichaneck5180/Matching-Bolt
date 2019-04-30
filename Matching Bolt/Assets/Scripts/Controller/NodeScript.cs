using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    private bool nodeEnabled;

    private int positionX;
    private int positionZ;

    private void Awake()
    {
        nodeEnabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNode(int x, int z)
    {
        positionX = x;
        positionZ = z;
    }

    public void SetEnabled(bool enable)
    {
        nodeEnabled = enable;
    }

    public bool GetEnabled()
    {
        return nodeEnabled;
    }

    public int GetXPosition()
    {
        return positionX;
    }

    public int GetZPosition()
    {
        return positionZ;
    }
}
