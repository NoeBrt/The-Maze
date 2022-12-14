using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginCameraMovement : MonoBehaviour
{
    public Camera cam;
    private Maze maze;

    // Start is called before the first frame update
    private void Start()
    {
        maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<Maze>();
        Cursor.lockState=CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = ((maze.Size.y + maze.Size.x) / 2f) * 5.5f * ((maze.NodeScale.x + maze.NodeScale.z) / 2f) / 10f;
    }
}
