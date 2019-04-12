using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Camera playerCamera;
    private Vector3 mousePosition;
    private Vector3 centerPoint;
    private Vector3 aimPoint;
    private Vector3 distanceToPlane;

    private float widthRatio;
    private float heightRatio;

    public GameObject playerProjectile;
    public float distanceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();

        widthRatio = playerCamera.pixelWidth / 1920f;
        heightRatio = playerCamera.pixelHeight / 1080f;

        mousePosition = Input.mousePosition;

        centerPoint = new Vector3(playerCamera.pixelWidth / 2, playerCamera.pixelHeight / 2, 0);
        aimPoint = centerPoint;

        distanceToPlane = transform.position - transform.parent.Find("Game Plane").Find("Plane Image").position;
        Debug.Log(distanceToPlane);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        
        // Calculate aimPoint.x
        if (mousePosition.x < centerPoint.x)
        {
            aimPoint.x = -Mathf.Abs(centerPoint.x - mousePosition.x) / (100 * widthRatio);
        }
        else if (mousePosition.x > centerPoint.x)
        {
            aimPoint.x = Mathf.Abs(mousePosition.x - centerPoint.x) / (100 * widthRatio);
        }
        else
        {
            aimPoint.x = 0;
        }

        // Calculate aimPoint.z
        if (mousePosition.y < centerPoint.y)
        {
            aimPoint.z = -Mathf.Abs(centerPoint.y - mousePosition.y) / (100 * heightRatio);
        }
        else if (mousePosition.y > centerPoint.y)
        {
            aimPoint.z = Mathf.Abs(mousePosition.y - centerPoint.y) / (100 * heightRatio);
        }
        else
        {
            aimPoint.z = 0;
        }

        aimPoint.x *= 1f * distanceMultiplier;
        aimPoint.z *= 1f * distanceMultiplier;
        aimPoint.y = -6f * distanceMultiplier;

        if (Input.GetMouseButtonDown(0) == true)
        {
            GameObject projectile = GameObject.Instantiate(playerProjectile, transform);
            projectile.GetComponent<Rigidbody>().velocity = aimPoint;
        }
    }

}
