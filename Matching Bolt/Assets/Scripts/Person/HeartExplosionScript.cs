using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartExplosionScript : MonoBehaviour
{
    private float timer;
    private void Awake()
    {
        Debug.Log("Spawned");
        timer = 2f;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }
    }
}
