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

    public GameObject playerProjectile;
    public float projectileSpeed;

    private void Awake()
    {
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true || GetComponent<WiimoteScript>().GetBButtonDown() == true)
        {
            GameObject projectile = GameObject.Instantiate(playerProjectile, playerCamera.gameObject.transform);
            projectile.GetComponent<ProjectileScript>().SetVelocityVector(GetComponent<PlayerAim>().GetAimPoint(), projectileSpeed);
        }
    }
}
