using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimer : MonoBehaviour
{
    private float timer;
    private void Awake()
    {
        Debug.Log("Spawned");
        timer = 3f;
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
