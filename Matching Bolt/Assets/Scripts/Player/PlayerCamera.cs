using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float cameraSensitivity;
    public int invertedHorizontal;
    public int invertedVertical;
    public bool wiimoteEnabled;

    private Vector3 startPosition;
    private GameObject camera;
    private float sideDistance;

    void Awake()
    {
        camera = transform.Find("Main Camera").gameObject;
        startPosition = camera.transform.position;
        sideDistance = 10f;
    }
    
    void Update()
    {
        if (wiimoteEnabled == true)
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
        else
        {
            int dir = 0;
            if (Input.GetKey(KeyCode.A) == true)
            {
                dir -= 1;
            }
            if (Input.GetKey(KeyCode.D) == true)
            {
                dir += 1;
            }
            if (dir == 0)
            {
                float dist = camera.transform.position.x - startPosition.x;
                if (dist > 1f)
                {
                    if (dist - 1 < 0)
                    {
                        dir = Mathf.RoundToInt(-dist);
                    }
                    else
                    {
                        dir = -1;
                    }
                }
                else if (dist < -1f)
                {
                    if (dist + 1 > 0)
                    {
                        dir = Mathf.RoundToInt(dist);
                    }
                    else
                    {
                        dir = 1;
                    }
                }
            }

            camera.transform.position = new Vector3(camera.transform.position.x + (dir * sideDistance * Time.deltaTime * 2), camera.transform.position.y, camera.transform.position.z);

            if (Mathf.Abs(camera.transform.position.x) > Mathf.Abs(startPosition.x + (dir * sideDistance)))
            {
                camera.transform.position = new Vector3(startPosition.x + (dir * sideDistance), camera.transform.position.y, camera.transform.position.z);
            }
        }
    }
}
