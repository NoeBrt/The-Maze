using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MazeGenerator Maze;
    [SerializeField] private GameObject Player;
    [SerializeField] private Camera BeginCamera;
    [SerializeField] private Vector2Int mazeSize;
    [SerializeField] private Vector3 nodeScale;
    [SerializeField] private Vector3 position;
    [SerializeField] private int bonusCount;
    List<GameObject> bonusItem=new List<GameObject>();

    bool playerInstanciated = false;

    void Start()
    {
        Maze.NodeScale = nodeScale;
        Maze.MazeSize = mazeSize;
        Maze = Instantiate(Maze, new Vector3(0, 0, 0), Quaternion.identity);

        // Debug.Log(Maze.GetComponent<MazeGenerator>().startNode.gameObject.transform.position);
        //  Instantiate(Maze,new Vector3(Maze.transform.position.x*10,0,Maze.transform.position.z),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Maze.IsFinished);
        if (Maze.IsFinished && !playerInstanciated)
        {
            Player = Instantiate(Player, Maze.startNode.transform.position, Quaternion.Euler(0, 90, 0));
            playerInstanciated = true;
            Player.SetActive(true);
            BeginCamera.gameObject.SetActive(false);
        }
    }

    void spawnBonusItem(){

    }
}
