using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupScript : MonoBehaviour
{
    private void Awake()
    {
        if (transform.position.x > 0)
        {
            transform.Translate(new Vector3(-15, 8, -3));
        }
        else
        {
            transform.Translate(new Vector3(15, 8, -3));
        }
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetGamePaused() == false)
        {
            Destroy(gameObject);
        }

        RotateToCamera();
    }

    private void RotateToCamera()
    {
        transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.rotation.eulerAngles, GameObject.FindGameObjectWithTag("Player").transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 10000f));
    }
}
