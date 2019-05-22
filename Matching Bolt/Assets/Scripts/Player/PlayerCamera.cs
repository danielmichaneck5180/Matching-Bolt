using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float cameraSensitivity;
    public int invertedHorizontal;
    public int invertedVertical;

    private Vector3 startPosition;
    private GameObject camera;

    void Awake()
    {
        camera = transform.Find("Main Camera").gameObject;
        startPosition = camera.transform.position;
    }
    
    void Update()
    {
        camera.transform.position = startPosition;
        float[] infra = new float[3];
        for (int i = 0; i < 3; i++)
        {
            infra[i] = GetComponent<WiimoteScript>().GetCameraWiimotePosition()[i];
        }
        infra[0] -= 0.5f; infra[1] -= 0.5f;// infra[2] -= 0.5f;
        Vector3 newVector = new Vector3(infra[0] * invertedHorizontal, infra[1] * invertedVertical * -1, infra[2] / 1500);
        camera.transform.Translate(newVector * cameraSensitivity, Space.Self);
        
    }
}
