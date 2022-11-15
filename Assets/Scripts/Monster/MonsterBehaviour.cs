using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MonsterBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundMask, PlayerMask;
    //patrolling
    public Vector3 walkPoint;
    bool walkPointset;
    public float walkPointRange;
    //attacking
    public float timeBetweenAtack;
    bool alreadyAttacked;
    //states;
    public float sightRange, attackRange;
    public bool playerInSightRange, PlayerInAttackRange;


    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerMask);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerMask);
        if (!playerInSightRange && !PlayerInAttackRange) Patroling();
        if (playerInSightRange) ChasePlayer();
       // if (playerInSightRange && PlayerInAttackRange) AttackPlayer();

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundMask)) ;
        walkPointset = true;
    }

    private void Patroling()
    {
        if (!walkPointset) SearchWalkPoint();
        if (walkPointset)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //WalkpointSet
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointset = false;
    }

    private void ChasePlayer()
    {
                agent.speed += 30;

        agent.SetDestination(player.position);
        transform.LookAt(player);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(player.position);

        transform.LookAt(player);
        if (Vector3.Distance(transform.position,player.transform.position)<=50f){
            Patroling();
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
