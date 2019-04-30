using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteScript : MonoBehaviour
{
    private Wiimote wiimote;
    private float[] ir;
    private float ir0;
    private float ir1;
    private float ir2;

    private void Awake()
    {
        WiimoteManager.FindWiimotes();
        wiimote = WiimoteManager.Wiimotes[0];
        wiimote.SetupIRCamera(IRDataType.BASIC);
        ir = new float[3];
    }

    void Update()
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
}
