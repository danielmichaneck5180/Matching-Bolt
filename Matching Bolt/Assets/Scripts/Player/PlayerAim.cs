using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Ray aimRay;
    private RaycastHit aimRayHit;

    private GameObject line;
    private Vector3[] linePositions;

    private Camera playerCamera;
    private Vector3 mousePosition;
    private Vector3 centerPoint;
    private Vector3 aimPoint;
    private float distanceToPlane;

    void Awake()
    {
        line = transform.Find("Line").gameObject;
        linePositions = new Vector3[2];

        // Initialize ray
        aimRay = new Ray
        {
            origin = transform.position
        };

        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();

        UpdateDistanceToPlane();
    }
    
    void Update()
    {
        aimRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        bool boolHit = false;
        if (Physics.Raycast(aimRay, out aimRayHit))
        {
            boolHit = true;
        }

        if (boolHit == true)
        {
            linePositions[0] = transform.position;
            linePositions[1] = aimRayHit.point;
            line.GetComponent<LineRenderer>().SetPositions(linePositions);
        }
    }

    private void UpdateDistanceToPlane()
    {
        // Sets distance to plane
        distanceToPlane = 0;
        Ray planeRay = new Ray(transform.position, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z));
        RaycastHit[] planeRays;
        planeRays = Physics.RaycastAll(planeRay);
        for (int i = 0; i < planeRays.Length; i++)
        {
            if (planeRays[i].transform.tag == "Main Plane")
            {
                distanceToPlane = planeRays[i].distance;
            }
        }
        if (distanceToPlane == 0)
        {
            Debug.Log("CRITICAL ERROR: NO MAIN PLANE FOUND");
        }
    }

    public Vector3 GetAimPoint()
    {
        return aimRayHit.point;
    }
}
