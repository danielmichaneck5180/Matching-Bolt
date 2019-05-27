using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private int health;

    private void Awake()
    {
        health = 6;
    }

    void Update()
    {
        if (health < 1)
        {
            GetComponent<GameHandler>().GameOver();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void ReduceHealth(int i)
    {
        health -= i;
    }
}
