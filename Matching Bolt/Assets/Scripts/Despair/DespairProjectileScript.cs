using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespairProjectileScript : MonoBehaviour
{
    public float speed;
    public float destroyBoundary;

    private Vector3 targetPosition;
    private Vector3 originPosition;
    private GameObject sprite;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollision")
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        originPosition = transform.position;
        sprite = transform.Find("Rotation").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        transform.SetParent(null);
    }

    public void SetupProjectile(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            targetPosition = new Vector3(0f, 1000f, 0f);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        RotateToCamera();

        // Checks if the projectile is outside of destroyBoundary and if true destroys it
        if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.x) >= destroyBoundary)
        {
            Destroy(gameObject);
        }
    }

    private void RotateToCamera()
    {
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 1000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x, 0, sprite.transform.rotation.eulerAngles.z));
    }
}
