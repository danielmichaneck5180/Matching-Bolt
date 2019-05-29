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
            //camera.transform.position = startPosition;
            float[] infra = new float[3];
            for (int i = 0; i < 3; i++)
            {
                infra[i] = GetComponent<WiimoteScript>().GetCameraWiimotePosition()[i];
            }
            infra[0] -= 0.5f; infra[1] -= 0.5f;// infra[2] -= 0.5f;
            Vector3 newVector = new Vector3(infra[0] * invertedHorizontal, 0, infra[1] * invertedVertical * -1);
            //camera.transform.Translate(newVector * cameraSensitivity, Space.Self);
            camera.transform.position = startPosition + (newVector * cameraSensitivity);
            camera.transform.Find("Crossbow").transform.position = new Vector3(0, 0, -25);
        }
        else
        {
            int xDir = CalcDir(KeyCode.A, KeyCode.D, camera.transform.position.x, startPosition.x);
            int yDir = CalcDir(KeyCode.S, KeyCode.W, camera.transform.position.z, startPosition.z);

            camera.transform.position = new Vector3(camera.transform.position.x + (xDir * sideDistance * Time.deltaTime * 2), camera.transform.position.y, camera.transform.position.z + (yDir * sideDistance * Time.deltaTime * 2));

            if (Mathf.Abs(camera.transform.position.x) > Mathf.Abs(startPosition.x + (xDir * sideDistance)))
            {
                camera.transform.position = new Vector3(startPosition.x + (xDir * sideDistance), camera.transform.position.y, camera.transform.position.z);
            }

            if (Mathf.Abs(camera.transform.position.z) > Mathf.Abs(startPosition.z + (yDir * sideDistance)))
            {
                camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, startPosition.z + (yDir * sideDistance));
            }
        }
    }

    private int CalcDir(KeyCode A, KeyCode B, float a, float b)
    {
        int dir = 0;
        if (Input.GetKey(A) == true)
        {
            dir -= 1;
        }
        if (Input.GetKey(B) == true)
        {
            dir += 1;
        }
        if (dir == 0)
        {
            float dist = a - b;
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
        return dir;
    }
}
