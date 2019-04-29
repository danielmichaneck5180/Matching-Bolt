using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float cameraSensitivity;
    public int invertedHorizontal;
    public int invertedVertical;

    private Vector3 startPosition;
    private GameObject playerCamera;

    void Awake()
    {
        playerCamera = transform.Find("Main Camera").gameObject;
        startPosition = playerCamera.transform.position;
    }
    
    void Update()
    {
        playerCamera.transform.position = startPosition;
        float[] ir = GetComponent<WiimoteScript>().GetWiimotePosition();
        ir[0] -= 0.5f; ir[1] -= 0.5f; ir[2] -= 0.5f;
        Vector3 newPosition = new Vector3(ir[0] * invertedHorizontal, ir[1] * invertedVertical * -1, ir[2] / 2000);
        playerCamera.transform.Translate(newPosition * cameraSensitivity, Space.Self);
    }
}
