using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Maze currentMaze;

    [SerializeField] private GameObject Player;
    [SerializeField] private Camera BeginCamera;
    [SerializeField] public Vector2Int mazeSize;
    [SerializeField] public Vector3 nodeScale;
    [SerializeField] private Vector3 position;
    [SerializeField] private int bonusCount;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject MazeKey;

    [SerializeField] List<GameObject> bonusItem = new List<GameObject>();

    bool playerInstanciated = false;

    void Start()
    {
        currentMaze = MazeGenerator.GenerateMaze(mazeSize, nodeScale, new Vector3(0, nodeScale.y / 2, 0), Quaternion.identity); //Instantiate(Maze, new Vector3(0, Maze.NodeScale.y / 2, 0), Quaternion.identity);

        // Debug.Log(Maze.GetComponent<MazeGenerator>().startNode.gameObject.transform.position);
        //  Instantiate(Maze,new Vector3(Maze.transform.position.x*10,0,Maze.transform.position.z),Quaternion.identity);
    }

    public void spawnMaze(Vector3 nodeScale, Vector2Int mazeSize)
    {
        BeginCamera.gameObject.SetActive(true);
        Player.SetActive(false);
        monster.SetActive(false);
        playerInstanciated = false;
        Destroy(currentMaze.gameObject);
        currentMaze = MazeGenerator.GenerateMaze(mazeSize, nodeScale, new Vector3(0, nodeScale.y / 2, 0), Quaternion.identity);
        currentMaze.name = "Maze";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMaze.IsFinished && !playerInstanciated)
        {
            spawnBonusItem(bonusCount);
            spawnKey();
            Player = Instantiate(Player, currentMaze.startNode.transform.position, Quaternion.Euler(0, 90, 0));
            monster = Instantiate(monster, currentMaze.Nodes[Random.Range(mazeSize.y, currentMaze.Nodes.Count)].transform.position - new Vector3(0, 20, 0), Quaternion.identity);
            playerInstanciated = true;
            Player.SetActive(true);
            monster.SetActive(true);

            BeginCamera.gameObject.SetActive(false);
        }
    }

    void spawnBonusItem(int bonusCount)
    {
        for (int i = 0; i < bonusCount; i++)
        {
            Vector3 nodePosition = currentMaze.Nodes[Random.Range(0, currentMaze.Nodes.Count)].transform.position;
            Debug.Log("bonus Position" + nodePosition);
            Vector3 bonusItemPosition = new Vector3(nodePosition.x, currentMaze.NodeScale.y / 10f, nodePosition.z);
            Instantiate(bonusItem[Random.Range(0, bonusItem.Count)], bonusItemPosition, Quaternion.identity, currentMaze.transform);
        }
    }
    void spawnKey()
    {
        Vector3 nodePosition = currentMaze.Nodes[Random.Range(0, currentMaze.Nodes.Count)].transform.position;
        Vector3 keyPos = new Vector3(nodePosition.x, currentMaze.NodeScale.y / 10f, nodePosition.z);
        Instantiate(MazeKey, keyPos, Quaternion.identity);
    }
}
