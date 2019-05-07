﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteScript : MonoBehaviour
{
    private Wiimote wiimote1;
    private Wiimote wiimote2;
    private float[] ir;
    private float ir0;
    private float ir1;
    private float ir2;
    private bool bHeld;
    private bool bDown;
    public int Camera;
    public int Crossbow;
    private int cameraWiimote;
    private int crossWiimote;


    private void Awake()
    {
        WiimoteManager.FindWiimotes();
        wiimote1 = WiimoteManager.Wiimotes[Camera];
        wiimote2 = WiimoteManager.Wiimotes[Crossbow];
        wiimote1.SetupIRCamera(IRDataType.BASIC);
        wiimote2.SetupIRCamera(IRDataType.BASIC);
        ir = new float[3];
        cameraWiimote = 0;
        crossWiimote = 1;
    }

    void Update()
    {
        Updatewiimote(wiimote1);
        Updatewiimote(wiimote2);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwtichWiimotes();
        }

    }

    public void SwtichWiimotes()
    {
        if(cameraWiimote == 0)
        {
            cameraWiimote = 1;
            crossWiimote = 0;
        }
        else
        {
            cameraWiimote = 0;
            crossWiimote = 1;
        }
    }

    private void Updatewiimote( Wiimote wiimote)
    {
        wiimote.ReadWiimoteData();

        float[] tempir = wiimote.Ir.GetPointingPosition();
        ir0 = tempir[0];
        ir1 = tempir[1];

        float[,] sensorIR = wiimote.Ir.GetProbableSensorBarIR(true);
        float xDis = sensorIR[0, 0] - sensorIR[0, 1];
        float yDis = sensorIR[1, 0] - sensorIR[1, 1];
        ir2 = Vector2.Distance(new Vector2(sensorIR[0, 0], sensorIR[0, 1]), new Vector2(sensorIR[1, 0], sensorIR[1, 1]));

        Debug.Log("IR0: " + ir0.ToString());
        Debug.Log("IR[0]: " + ir[0].ToString());
        Debug.Log("IR1: " + ir1.ToString());
        Debug.Log("IR[1]: " + ir[1].ToString());
        Debug.Log("IR2: " + ir2.ToString());
        Debug.Log("IR[2]: " + ir[2].ToString());

        if (ir0 > 0)
        {
            ir[0] = ir0;
        }

        if (ir1 > 0)
        {
            ir[1] = ir1;
        }

        if (ir2 > 0)
        {
            ir[2] = ir2;
        }

        if (bHeld == true)
        {
            bDown = false;
        }
        else
        {
            bDown = true;
        }

        if (wiimote.Button.b == true)
        {
            bHeld = true;
        }
        else
        {
            bHeld = false;
        }
    }

    public float[] GetWiimotePosition()
    {
        return ir;
    }

    public int GetWiimoteWidth()
    {
        return 1023;
    }

    public int GetWiimoteHeight()
    {
        return 767;
    }

    public bool GetBButton()
    {
        return bHeld;
    }

    public bool GetBButtonDown()
    {
        if (bHeld == true)
        {
            return bDown;
        }
        else
        {
            return false;
        }
    }
}
