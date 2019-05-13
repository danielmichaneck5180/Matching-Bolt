using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float destroyBoundary;
    private Vector3 originPosition;
    private Vector3 velocityVector;

    public void SetVelocityVector(Vector3 vector, float speed)
    {
        velocityVector = Vector3.Normalize(vector) * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Person":
                other.gameObject.GetComponent<PersonScript>().HitPerson();
                Destroy(gameObject);
                break;
        }
    }

    private void Awake()
    {
        originPosition = transform.position;
    }

    void Update()
    {
        // Moves the projectile
        transform.Translate(velocityVector * Time.deltaTime * 60, Space.World);

        // Checks if the projectile is outside of destroyBoundary and if true destroys it
        if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.x) >= destroyBoundary)
        {
            Destroy(gameObject);
        }
    }
}
