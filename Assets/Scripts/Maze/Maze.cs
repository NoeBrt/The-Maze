using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Maze : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector3 nodeScale;
    [SerializeField] Material finishMaterial;
    [SerializeField] Material startMaterial;
    [SerializeField] GameObject plane;

    public bool IsFinished { get; set; } = false;
    public List<MazeNode> Nodes { get; set; }

    public MazeNode StartNode { get; set; }
    public MazeNode FinishNode { get; set; }
    public Vector2Int Size { get => size; set => size = value; }
    public Vector3 NodeScale { get => nodeScale; set => nodeScale = value; }

    public Material StartMaterial { get => startMaterial; set => startMaterial = value; }
    public GameObject Plane { get => plane; set => plane = value; }
    public Material FinishMaterial { get => finishMaterial; set => finishMaterial = value; }


    public void setNodesVisibility(bool isNodesVisible)
    {
        foreach (MazeNode m in Nodes)
        {
            m.gameObject.SetActive(true);
        }

    }
}



// Update is called once per frame
