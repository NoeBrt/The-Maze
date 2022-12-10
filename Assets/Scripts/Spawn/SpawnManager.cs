using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Maze currentMaze;
    NavMeshSurface surface;
    [SerializeField] private GameObject Player;
    [SerializeField] private Camera BeginCamera;
    public Vector2Int mazeSize;
    [SerializeField] public Vector3 nodeScale;
    [SerializeField] private Vector3 position;
    [SerializeField] private int bonusCount;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject MazeKey;
    [SerializeField] List<GameObject> bonusItem = new List<GameObject>();
    private List<int> itemPositionList;
    bool playerInstanciated = false;
    [SerializeField] List<Material> wallMaterials;
    [SerializeField] List<Material> floorMaterials;
    [SerializeField] Material basicMat;

    [SerializeField] AudioReverbZone audioReverb;

    void Start()
    {

        mazeSize = GameManager.Instance.currentMazeSize;
        itemPositionList = new List<int>();
        surface = GetComponent<NavMeshSurface>();
        audioReverb.minDistance = new Vector2(mazeSize.x * nodeScale.x, mazeSize.y * nodeScale.x).magnitude / 2f;
        audioReverb.maxDistance = audioReverb.minDistance;
        generateMaze();

    }

    void generateMaze()
    {
        Material _wallMaterial = basicMat;
        Material _floorMaterial = basicMat;
        if (QualitySettings.GetQualityLevel() >= 3)
        {
            _wallMaterial = wallMaterials[Random.Range(0, wallMaterials.Count)];
            _floorMaterial = floorMaterials[Random.Range(0, floorMaterials.Count)];
        }
        currentMaze = MazeGenerator.GenerateMaze(mazeSize, nodeScale, new Vector3(0, nodeScale.y / 2, 0), Quaternion.identity, true); //Instantiate(Maze, new Vector3(0, Maze.NodeScale.y / 2, 0), Quaternion.identity);
        currentMaze.Plane.GetComponent<Renderer>().material = _floorMaterial;
        currentMaze.setWallsMaterial(_wallMaterial);
    }

    // Update is called once per frameF
    void Update()
    {
        if (currentMaze.IsFinished && !playerInstanciated)
        {
            spawnBonusItem(bonusCount);
            spawnKey();
            Player = Instantiate(Player, currentMaze.StartNode.transform.position, Quaternion.Euler(0, 90, 0));
            surface.BuildNavMesh();
            spawnMonster();
            playerInstanciated = true;
            Player.SetActive(true);
            monster.SetActive(true);
            BeginCamera.gameObject.SetActive(false);

        }

    }

    void spawnMonster()
    {
        monster = InstantiateAtNode(monster, Random.Range(mazeSize.y + mazeSize.y / 2, currentMaze.Nodes.Count), 9.9f, Quaternion.AngleAxis(90f, Vector3.right));

    }

    void spawnBonusItem(int bonusCount)
    {
        for (int i = 0; i < bonusCount; i++)
        {
            int itemsPosition = randomIntExcept(0, currentMaze.Nodes.Count - 1, itemPositionList);
            itemPositionList.Add(itemsPosition);
            // int indexItem= Random.Range(0f,1f)>=0.7f ? 1:0; 
            InstantiateAtNode(bonusItem[Random.Range(0, bonusItem.Count)], itemsPosition, currentMaze.NodeScale.y / 10f, Quaternion.identity, currentMaze.transform);
        }
    }

    void spawnKey()
    {
        int itemsPosition = randomIntExcept(mazeSize.y + mazeSize.y / 2, currentMaze.Nodes.Count, itemPositionList);
        itemPositionList.Add(itemsPosition);
        InstantiateAtNode(MazeKey, itemsPosition, currentMaze.NodeScale.y / 10f, Quaternion.AngleAxis(90f, Vector3.right));
    }


    private GameObject InstantiateAtNode(GameObject obj, int nodeIndex, float height, Quaternion rotation)
    {
        Vector3 nodePosition = currentMaze.Nodes[nodeIndex].transform.position;
        return Instantiate(obj, new Vector3(nodePosition.x, height, nodePosition.z), rotation);
    }
    private GameObject InstantiateAtNode(GameObject obj, int nodeIndex, float height, Quaternion rotation, Transform transform)
    {
        Vector3 nodePosition = currentMaze.Nodes[nodeIndex].transform.position;
        return Instantiate(obj, new Vector3(nodePosition.x, height, nodePosition.z), rotation, transform);
    }


    private int randomIntExcept(int min, int max, List<int> except)
    {
        int result = Random.Range(min, max - 1);
        while (except.Contains(result))
        {
            result = Random.Range(min, max - 1);
        }
        return result;
    }
}
