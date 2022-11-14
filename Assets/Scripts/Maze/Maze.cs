using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Maze: MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector3 nodeScale;
    [SerializeField] Material finishMaterial;
    [SerializeField] Material startMaterial;
    [SerializeField] GameObject plane;
    public MazeNode startNode;
    public MazeNode finishNode;

    public bool IsFinished { get; set; }
    public List<MazeNode> Nodes { get; set; }


    public Vector2Int Size { get => size; set => size = value; }
    public Vector3 NodeScale { get => nodeScale; set => nodeScale = value; }
    public MazeNode StartNode { get => startNode; set => startNode = value; }
    public MazeNode FinishNode { get => finishNode; set => finishNode = value; }
    public Material StartMaterial { get => startMaterial; set => startMaterial = value; }
    public MazeNode NodePrefab { get => nodePrefab; set => nodePrefab = value; }
    public GameObject Plane { get => plane; set => plane = value; }
    public Material FinishMaterial { get => finishMaterial; set => finishMaterial = value; }


void initialize(){
    
}
}



// Update is called once per frame
