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
    //states;
    public float sightRange, attackRange;
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
    [SerializeField] float maxChaseDistance = 25f;


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
        playerInSightRange = player.GetComponentInChildren<Torch>().LightTorch.activeSelf && Physics.SphereCast(transform.position, transform.localScale.x, elements.transform.forward, out RaycastHit hitInfo, 60f, PlayerMask);
        // Debug.DrawRay(transform.position, elements.transform.forward * 50f, Color.magenta);
        playerInHeardRange = Physics.CheckSphere(transform.position, sightRange * player.GetComponent<PlayerController>().stepSoundVolume.x, PlayerMask);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerMask);
        if ((!playerInHeardRange || !playerInSightRange) && !isChasing) Patroling();
        if (playerInHeardRange || playerInSightRange) ChasePlayer();
    }

    private void Patroling()
    {
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
        //  Debug.Log(new Vector3(transform.position.x, 0, transform.position.z) + "  " + new Vector3(transform.position.x, 0, transform.position.z));

    }


    private void ChasePlayer()
    {
        if (monsterSound.clip != ChaseSound)
        {
            monsterSound.clip = ChaseSound;
            monsterSound.Play();
        }
        monsterSound.PlayOneShot(ScreamSoundFX);
        hand.SetActive(true);
        elements.transform.LookAt(player);
        agent.speed = 35;
        agent.SetDestination(player.position);
        // transform.LookAt(player);
        if (Vector3.Distance(transform.position, player.transform.position) >= maxChaseDistance)
        {
            isChasing = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange * player.GetComponent<PlayerController>().stepSoundVolume.y);



    }

    private void OnDrawGizmos()
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
