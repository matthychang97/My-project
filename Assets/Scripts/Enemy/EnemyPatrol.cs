using UnityEngine;
using UnityEngine.AI;
//Made by Matthew Chang
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float waypointTolerance = 0.5f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 areaCenter;
    private float waitTimer;
    private bool waiting;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        areaCenter = transform.position; // wander around spawn point
        SetNewDestination();
    }

    void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                SetNewDestination();
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            waiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += areaCenter;

        // Make sure the random point actually lands on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}