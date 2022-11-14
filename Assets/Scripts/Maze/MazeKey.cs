using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeKey : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject finalWall;
    Maze maze;
    void Start()
    {
        maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<Maze>();
        finalWall = maze.FinishNode.Walls[0];
    }
    // Update is called once per frame
    private void  OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.gameObject.CompareTag("Player"))
        {
            finalWall.GetComponent<MeshRenderer>().material = maze.FinishMaterial;
            other.gameObject.GetComponent<PlayerController>().PlayerUi.displayMessage("Key Founded", 5f);
              Destroy(gameObject);
        }
    }
}
