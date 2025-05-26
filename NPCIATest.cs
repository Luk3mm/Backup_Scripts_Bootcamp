using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class NPCIATest : MonoBehaviour
{
    [Header("Components")]
    public List<Transform> waypoints;
    public LayerMask playerLayer;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator anim;
    private PlayerAttackTest playerAttack;

    [Header("Variable")]
    public int currentWaypointIndex;
    public float speed;
    public int damageEnemy;
    public int maxLifeEnemy;
    private int currentLifeEnemy;
    private bool isPlayerDetected;

    [Header("Patrol")]
    public float idleTimeInWaypoint;
    private bool isWaiting;

    public Transform playerTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = speed;

        currentLifeEnemy = maxLifeEnemy;

        GoToNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerDetected && playerTarget != null)
        {
            float distance = Vector3.Distance(transform.position, playerTarget.position);

            if(distance <= 2f)
            {
                navMeshAgent.isStopped = true;
                anim.SetBool("move", true);
                anim.SetTrigger("attack");
                transform.LookAt(playerTarget.position);
            }
            else
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(playerTarget.position);
                anim.SetBool("move", true);
            }
        }
        else if(!navMeshAgent.pathPending && !navMeshAgent.hasPath && !isWaiting)
        {
            StartCoroutine(WaitInWaypoint());
        }
    }

    IEnumerator WaitInWaypoint()
    {
        isWaiting = true;
        navMeshAgent.isStopped = true;
        anim.SetBool("move", false);

        yield return new WaitForSeconds(idleTimeInWaypoint);

        GoToNextWaypoint();
        isWaiting = false;
    }

    private void GoToNextWaypoint()
    {
        if(waypoints.Count == 0)
        {
            return;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        anim.SetBool("move", true);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

        if(currentLifeEnemy <= 0)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void TakeDamage(int amount)
    {
        currentLifeEnemy -= amount;
        anim.SetTrigger("hurt");

        if (currentLifeEnemy <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        navMeshAgent.isStopped = true;
        anim.SetTrigger("death");
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 3f);
    }

    private void OnDamagePlayer()
    {
        if (playerAttack != null)
        {
            playerAttack.TakeDamage(damageEnemy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = true;

            playerAttack = other.GetComponent<PlayerAttackTest>();

            if (playerAttack != null)
            {
                InvokeRepeating("OnDamagePlayer", 0f, 2f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = false;
            navMeshAgent.isStopped = false;

            CancelInvoke("OnDamagePlayer");
        }
    }
}
