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
    /*
    private GameObject line;
    private LineRenderer lineRenderer;
    private Vector3[] linePositions;*/
    private Ray ray;

    private bool gonnaHit;
    private Vector3 previousPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollision")
        {
            GameObject.FindGameObjectWithTag("Controller").GetComponent<HealthScript>().ReduceHealth(1);
            GameObject.FindGameObjectWithTag("Controller").GetComponent<AudioManager>().PlaySound("PlayerHit");
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        previousPosition = transform.position;/*
        line = transform.Find("Line").gameObject;
        linePositions = new Vector3[2];
        lineRenderer = line.GetComponent<LineRenderer>();*/
        gonnaHit = false;
        ray = new Ray
        {
            origin = transform.position
        };


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

        float difMulti = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameHandler>().GetDifficultyMultiplier() / 3f;
        if (difMulti > 3f)
        {
            difMulti = 3f;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime * difMulti);

        // Checks if the projectile is outside of destroyBoundary and if true destroys it
        if (originPosition.x + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.y + Mathf.Abs(transform.position.x) >= destroyBoundary || originPosition.z + Mathf.Abs(transform.position.x) >= destroyBoundary)
        {
            Destroy(gameObject);
        }

        RotateToCamera();
        //line.SetActive(false);
        CheckCollisionCourse();
    }

    private void RotateToCamera()
    {
        sprite.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(sprite.transform.rotation.eulerAngles, player.transform.Find("Main Camera").transform.rotation.eulerAngles, 10000f, 1000f));
        sprite.transform.rotation = Quaternion.Euler(new Vector3(sprite.transform.rotation.eulerAngles.x, 0, sprite.transform.rotation.eulerAngles.z));
    }

    private void CheckCollisionCourse()
    {
        Vector3 rayDirection = targetPosition - transform.position;
        //Debug.Log(rayDirection.x + " " + rayDirection.y + " " + rayDirection.z);
        rayDirection.Normalize();
        ray = new Ray(transform.position, rayDirection);
        if (Vector3.Distance(ray.GetPoint(Vector3.Distance(transform.position, targetPosition)), targetPosition) < 2f)
        {
            //Debug.Log("OI");
        }

        //Debug.Log(ray.GetPoint(Vector3.Distance(transform.position, targetPosition)).y);
        //Debug.Log(ray.GetPoint(10f).y);
        bool tempHit = gonnaHit;
        gonnaHit = Physics.Raycast(ray, out RaycastHit rayHit, Mathf.Infinity, LayerMask.GetMask("Player"));
        if (gonnaHit != tempHit)
        {
            Debug.Log("DespairProjectileScirpt::CheckCollisionCourse HIT");
            SetSprite(gonnaHit);
        }
        //if (gonnaHit == true)
        /*

        {
            linePositions[0] = transform.position;
            linePositions[1] = ray.GetPoint(Vector3.Distance(transform.position, targetPosition));
            lineRenderer.SetPositions(linePositions);
        }/*
        previousPosition = transform.position;*/
    }

    private void SetSprite(bool hit)
    {
        switch (hit)
        {
            case true:
                Debug.Log("HEY");
                sprite.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetDespairProjectile("Warning");
                break;

            default:
                Debug.Log("YO");
                sprite.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("Controller").GetComponent<SpriteReferences>().GetDespairProjectile("Normal");
                break;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray);
    }
}
