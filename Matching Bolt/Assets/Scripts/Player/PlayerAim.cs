using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Ray aimRay;
    private RaycastHit aimRayHit;

    private GameObject line;
    private LineRenderer lineRenderer;
    private Vector3[] linePositions;

    private GameObject crossbow;
    private GameObject aim;

    private Camera playerCamera;
    private Vector3 mousePosition;
    private Vector3 centerPoint;
    private Vector3 aimPoint;
    private float distanceToPlane;
    private float cameraMultiplier;
    private List<float> irX;
    private List<float> irZ;
    private readonly float smoothening = 20f;
    private readonly int maxCount = 4;

    void Awake()
    {
        cameraMultiplier = 100f;
        irX = new List<float>();
        irZ = new List<float>();
        for (int i = 0; i < 60; i++)
        {
            irX.Add(0f);
            irZ.Add(0f);
        }

        line = transform.Find("Line").gameObject;
        linePositions = new Vector3[2];

        lineRenderer = line.GetComponent<LineRenderer>();
        //lineRenderer.sortingOrder = 1;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.material.color = Color.red;

        // Initialize ray
        aimRay = new Ray
        {
            origin = transform.position
        };

        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        crossbow = playerCamera.transform.Find("Crossbow").gameObject;
        aim = transform.Find("Aim").gameObject;

        UpdateDistanceToPlane();
    }
    
    void Update()
    {
        //aimRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        float[] ir = GetComponent<WiimoteScript>().GetCrossWiimotePosition();
        ir[0] -= 0.5f;
        ir[1] -= 0.5f;

        // Adds values to list of floats and calculates medium position
        float mediumX = CalcMediumValue(irX);
        float mediumZ = CalcMediumValue(irZ);

        for (int i = 0; i < CompareDifference(mediumX, ir[0], smoothening, maxCount); i++)
        {
            irX.Add(ir[0]);
        }

        for (int i = 0; i < CompareDifference(mediumZ, ir[1], smoothening, maxCount); i++)
        {
            irZ.Add(ir[1]);
        }

        RefineList(irX, maxCount);
        RefineList(irZ, maxCount);

        mediumX = CalcMediumValue(irX);
        mediumZ = CalcMediumValue(irZ);

        //aimRay = playerCamera.ScreenPointToRay(new Vector3((ir[0] * playerCamera.pixelWidth), (ir[1] * playerCamera.pixelHeight), ir[2]));
        aimRay = new Ray(aim.transform.position, new Vector3(mediumX * cameraMultiplier, -distanceToPlane, mediumZ * cameraMultiplier));
        bool boolHit = false;
        if (Physics.Raycast(aimRay, out aimRayHit, 1000f, LayerMask.GetMask("RaycastTarget")))
        {
            boolHit = true;
        }

        //Debug.Log(aimRayHit.point.x);

        //Debug.Log("Point: " + aimRayHit.point.x + " " + aimRayHit.point.y + " " + aimRayHit.point.z);

        if (boolHit == true)
        {
            linePositions[0] = crossbow.transform.position;
            linePositions[1] = aimRayHit.point;
            lineRenderer.SetPositions(linePositions);
            if (aimRayHit.collider.gameObject.tag == "Person")
            {
                aimRayHit.collider.gameObject.GetComponent<PersonScript>().AimedAt();
            }
            //Debug.Log((linePositions[0] - linePositions[1]).x);
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

    private float CalcMediumValue(List<float> floats)
    {
        float returnFloat = 0;
        if (floats.Count > 0)
        {
            for (int i = 0; i < floats.Count; i++)
            {
                returnFloat += floats[i];
            }
            returnFloat /= floats.Count;
        }
        return returnFloat;
    }

    private List<float> RefineList(List<float> list, int count)
    {
        for (int i = 0; i < list.Count - count; i++)
        {
            list.Remove(list[0]);
        }
        return list;
    }

    private int CompareDifference(float medium, float recent, float multiplier, int max)
    {
        int returnInt = 0;
        for (int i = 0; i < 1 + Mathf.FloorToInt(Mathf.Abs(medium - recent) * multiplier); i++)
        {
            if (i > max)
            {
                i = 1 + Mathf.FloorToInt(Mathf.Abs(medium - recent) * multiplier);
            }
            returnInt += 1;
        }
        return returnInt;
    }

    public Vector3 GetAimPoint()
    {
        return aimRayHit.point;
    }
}
