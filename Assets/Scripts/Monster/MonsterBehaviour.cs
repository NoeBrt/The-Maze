using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MonsterBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundMask, PlayerMask;
    //attacking
    [Range(0, 360)]
    [SerializeField] public float angle = 100;
    //states;
    public float hearRange, attackRange;
    public bool playerInHeardRange, playerInSightRange, PlayerInAttackRange;
    public GameObject hand;
    public Transform elements;
    Maze maze;
    bool isDestinationReached = true;
    Vector3 destination;
    AudioSource monsterSound;
    [SerializeField] AudioClip ScreamSoundFX;
    [SerializeField] AudioClip ChaseSound;
    [SerializeField] AudioClip PatrollingSound;
    bool isChasing = false;
    [SerializeField] float fieldOfViewMagnitude = 30f;

    [SerializeField] float maxChaseDistance = 55f;
    float torchFactor;
    float heardFactor;
    bool isSeePlayer = false;
    [SerializeField] float chaseSpeed = 35f;
    [SerializeField] float patrolingSpeed = 20f;
    public float ChaseSpeed { get => chaseSpeed; set => chaseSpeed = value; }
    public float PatrolingSpeed { get => patrolingSpeed; set => patrolingSpeed = value; }




    // Start is called before the first frame update
    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<Maze>();
        agent = GetComponent<NavMeshAgent>();
        elements = transform.Find("Elements");
        hand = elements.Find("MonsterHand").gameObject;
        monsterSound = GetComponent<AudioSource>();
        SettingManager.Instance.addSfxSound(monsterSound);
    }
    private void Update()
    {
        FieldOfViewCheck();
        setHear();
        torchFactor = player.GetComponentInChildren<Torch>().LightTorch.activeSelf ? 2 : 1;
        Debug.DrawRay(transform.position, elements.transform.forward * fieldOfViewMagnitude * torchFactor, Color.magenta);
        if ((!playerInHeardRange || !isSeePlayer) && !isChasing) Patroling();
        if (playerInHeardRange || isSeePlayer || isChasing) ChasePlayer();
    }


    void setHear()
    {
        Vector2 velocityPlayer = new Vector2(player.GetComponent<PlayerController>().Velocity.x, player.GetComponent<PlayerController>().Velocity.z);
        heardFactor = velocityPlayer.magnitude * player.GetComponent<PlayerController>().stepSoundVolume.x;
        hearRange = heardFactor * 5f + 10f;
        playerInHeardRange = Physics.CheckSphere(transform.position, hearRange, PlayerMask);

    }
    private void Patroling()
    {
        agent.speed = patrolingSpeed;
        if (monsterSound.clip != PatrollingSound)
        {
            monsterSound.clip = PatrollingSound;
            monsterSound.Play();
        }
        isDestinationReached = new Vector3(transform.position.x, 0, transform.position.z) == new Vector3(agent.destination.x, 0, agent.destination.z);
        hand.SetActive(false);
        elements.localRotation = Quaternion.Euler(0, 0, 0);
        if (isDestinationReached)
        {
            destination = maze.transform.TransformPoint(maze.Nodes[Random.Range(0, maze.Nodes.Count)].transform.position);
            agent.SetDestination(destination);
        }

    }


    private void ChasePlayer()
    {
        isChasing = true;
        if (monsterSound.clip != ChaseSound)
        {
            monsterSound.clip = ChaseSound;
            monsterSound.Play();
        }
        monsterSound.PlayOneShot(ScreamSoundFX);
        hand.SetActive(true);
        elements.transform.LookAt(player);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        // transform.LookAt(player);
        if (Vector3.Distance(transform.position, player.transform.position) > maxChaseDistance * torchFactor)
        {
            isChasing = false;
        }
    }


    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fieldOfViewMagnitude * torchFactor, PlayerMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(elements.transform.forward, directionToTarget) < (torchFactor == 2 ? 360 : angle) / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, GroundMask))
                    isSeePlayer = true;
                else
                    isSeePlayer = false;
            }
            else
                isSeePlayer = false;
        }
        else if (isSeePlayer)
            isSeePlayer = false;
    }



    private void OnDrawGizmosSelected()
    {



    }

    private void OnDrawGizmos()
    {
        if (player == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearRange);
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
