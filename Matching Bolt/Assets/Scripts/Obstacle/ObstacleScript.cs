using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public int obstacleSize;
    private GameObject sprite;
    private GameObject player;

    private void Awake()
    {
        sprite = transform.Find("Rotation").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        RotateToCamera();
    }

    public int GetObstacleSize()
    {
        return obstacleSize;
    }

    private void RotateToCamera()
    {
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 1000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x, 0, sprite.transform.rotation.eulerAngles.z));
    }
}
