using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    private bool nodeEnabled;

    private int positionX;
    private int positionZ;

    private GameObject green;
    private GameObject red;

    private void Awake()
    {
        SetEnabled(true);
        green = transform.Find("Green").gameObject;
        red = transform.Find("Red").gameObject;

        green.SetActive(false);
        red.SetActive(false);
    }
    
    void Update()
    {/*
        if (nodeEnabled == true)
        {
            green.SetActive(true);
            red.SetActive(false);
        }
        else
        {
            green.SetActive(false);
            red.SetActive(true);
        }*/
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
