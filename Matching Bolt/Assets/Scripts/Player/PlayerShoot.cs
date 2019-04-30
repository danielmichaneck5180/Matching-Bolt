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

    private float widthRatio;
    private float heightRatio;

    public GameObject playerProjectile;
    public float projectileSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            GameObject projectile = GameObject.Instantiate(playerProjectile, transform);
            projectile.GetComponent<ProjectileScript>().SetVelocityVector(GetComponent<PlayerAim>().GetAimPoint(), projectileSpeed);
        }
    }

}
