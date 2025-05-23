using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIA : MonoBehaviour
{
    [Header("Components")]
    public List<Transform> waypoints;
    public LayerMask playerLayer;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator anim;

    [Header("Variable")]
    public int currentWaypointIndex = 0;
    public float speed;
    private bool isPlayerDetected = false;
    private bool onRadius;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerDetected)
        {
            Walking();
        }
        else
        {
            StopWalking();
            anim.SetTrigger("attack");
        }
    }

    void Walking()
    {
        if(waypoints.Count == 0)
        {
            return;
        }

        float distanceToWaypoint = Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position);

        if(distanceToWaypoint <= 2)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        //anim.SetBool("move", true);
        onRadius = false;
    }

    void StopWalking()
    {
        navMeshAgent.isStopped = true;
        //anim.SetBool("move", false);
        onRadius = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = false;
            navMeshAgent.isStopped = false;
        }
    }
}
