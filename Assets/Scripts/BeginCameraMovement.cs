using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginCameraMovement : MonoBehaviour
{
    public Camera cam;
    public MazeGenerator maze;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = ((maze.MazeSize.y + maze.MazeSize.x) / 2f) * 5.5f * ((maze.NodeScale.x + maze.NodeScale.z) / 2f) / 10f;
    }
}
