using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    MazeGenerator maze;
    public GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");
        //maze = GetComponent<MazeGenerator>();
        maze = new MazeGenerator(10, 10, wall);
        maze.showMaze();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
