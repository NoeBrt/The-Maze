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
    public GameObject hand;
    public Transform elements;
    Maze maze;
    bool isDestinationReached = true;
    Vector3 destination;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<Maze>();
        agent = GetComponent<NavMeshAgent>();
        elements = transform.Find("Elements");
        hand = elements.Find("MonsterHand").gameObject;
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerMask);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerMask);
        if (!playerInSightRange) Patroling();
        if (playerInSightRange) ChasePlayer();
        // if (playerInSightRange && PlayerInAttackRange) AttackPlayer();

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundMask))
            walkPointset = true;
    }


    private void Patroling()
    {
        isDestinationReached = new Vector3(transform.position.x, 0, transform.position.z) == new Vector3(agent.destination.x, 0, agent.destination.z);
        hand.SetActive(false);
        elements.localRotation = Quaternion.Euler(0, 0, 0);
        if (isDestinationReached)
        {
            destination = maze.transform.TransformPoint(maze.Nodes[Random.Range(0, maze.Nodes.Count)].transform.position);
            agent.SetDestination(destination);
        }
        //  Debug.Log(new Vector3(transform.position.x, 0, transform.position.z) + "  " + new Vector3(transform.position.x, 0, transform.position.z));

    }


    private void ChasePlayer()
    {
        hand.SetActive(true);
        elements.transform.LookAt(player);
        agent.speed = 30;
        agent.SetDestination(player.position);
        // transform.LookAt(player);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(player.position);

        //   transform.LookAt(player);
        if (Vector3.Distance(transform.position, player.transform.position) <= 50f)
        {
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
    void OnDrawGizmos()
    {
        if (player == null) return;

        var path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas, path);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.up);
        Gizmos.DrawRay(player.transform.position, Vector3.up);
        Gizmos.color = Color.green;
        var offset = 0.2f * Vector3.up;
        for (int i = 1; i < path.corners.Length; ++i)
            Gizmos.DrawLine(path.corners[i - 1] + offset, path.corners[i] + offset);
    }
}
