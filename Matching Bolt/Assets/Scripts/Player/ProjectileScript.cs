using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float destroyBoundary;
    private Vector3 originPosition;
    private Vector3 velocityVector;
    private float speed;
    private bool hit;

    public void SetVelocityVector(Vector3 vector, float s)
    {
        /*
        Debug.Log("X: " + transform.position.x + " " + vector.x);
        Debug.Log("Y: " + transform.position.y + " " + vector.y);
        Debug.Log("Z: " + transform.position.z + " " + vector.z);
        */
        velocityVector = vector;
        speed = s;
        //velocityVector = Vector3.Normalize(vector) * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Person":
                if (hit == false)
                {
                    other.gameObject.GetComponent<PersonScript>().HitPerson();
                }
                hit = true;
                Destroy(gameObject);
                break;

            case "Despair":
                other.gameObject.GetComponent<DespairScript>().HitDespair();
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }

    private void Awake()
    {
        hit = false;
        originPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, velocityVector) < 1f)
        {
            velocityVector = new Vector3(velocityVector.x, -200f, velocityVector.z);
        }

        // Moves the projectile
        //transform.Translate(velocityVector * Time.deltaTime, Space.World);
        transform.position = Vector3.MoveTowards(transform.position, velocityVector, speed * Time.deltaTime);
        
        // Checks if the projectile is outside of destroyBoundary and if true destroys it
        if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.y) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.z) >= destroyBoundary)
        {
            Destroy(gameObject);
        }
    }
}
