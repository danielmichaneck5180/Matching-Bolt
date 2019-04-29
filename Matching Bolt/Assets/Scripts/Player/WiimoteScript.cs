using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteScript : MonoBehaviour
{
    private Wiimote wiimote;
    private float[] ir;

    private void Awake()
    {
        WiimoteManager.FindWiimotes();
        wiimote = WiimoteManager.Wiimotes[0];
        wiimote.SetupIRCamera(IRDataType.BASIC);
    }

    void Update()
    {
        wiimote.ReadWiimoteData();
        ir = new float[3];
        float[] tempir = wiimote.Ir.GetPointingPosition();
        ir[0] = tempir[0];
        ir[1] = tempir[1];

        float[] ret = new float[2];
        float[,] sensorIR = wiimote.Ir.GetProbableSensorBarIR(true);
        float xDis = sensorIR[0, 0] - sensorIR[0, 1];
        float yDis = sensorIR[1, 0] - sensorIR[1, 1];
        ir[2] = Vector2.Distance(new Vector2(sensorIR[0, 0], sensorIR[0, 1]), new Vector2(sensorIR[1, 0], sensorIR[1, 1]));
        Debug.Log("IR[2]: " + ir[2]);
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
