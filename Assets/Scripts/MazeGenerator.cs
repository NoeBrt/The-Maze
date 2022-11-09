using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private Vector2Int mazeSize;
    [SerializeField] public Vector3 nodeScale;
    [SerializeField] Material finishMaterial;
    [SerializeField] Material startMaterial;
    [SerializeField] GameObject plane;

    private Vector3 offsetParent;
    public MazeNode startNode;
    public MazeNode finishNode;

    public bool IsFinished { get; set; }
    public List<MazeNode> Nodes { get; set; }


    public Vector2Int MazeSize { get => mazeSize; set => mazeSize = value; }
    public Vector3 NodeScale { get => nodeScale; set => nodeScale = value; }
    public MazeNode StartNode { get => startNode; set => startNode = value; }
    public MazeNode FinishNode { get => finishNode; set => finishNode = value; }
    public Material FinishMaterial { get => finishMaterial; set => finishMaterial = value; }
    public Material StartMaterial { get => startMaterial; set => startMaterial = value; }

    private void Start()
    {

        offsetParent = (transform.position + (Vector3.right * nodePrefab.transform.localScale.x / 2) + (Vector3.forward * nodePrefab.transform.localScale.z / 2));
        Debug.Log((Vector3.forward * nodePrefab.transform.localScale.z / 2f));
        nodePrefab.transform.localScale = nodeScale;
        StartCoroutine(GenerateMaze(mazeSize));
    }

    IEnumerator GenerateMaze(Vector2Int size)
    {
        plane.SetActive(true);
        Nodes = new List<MazeNode>();
        yield return generateGrid(Nodes, size);
        yield return generateSimpleMaze(Nodes, size);
        generateStartAndEnd(Nodes, size);
        yield return makeComplexMaze(Nodes, size);
        Debug.Log("test");
        yield return setAllWallsPlayedState(Nodes, size);
        yield return new WaitForSeconds(3f);
        setFloor();
        supressOverlapWall();
        //meshFusion();
        IsFinished = true;


    }

    IEnumerator generateGrid(List<MazeNode> Nodes, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3 nodePos = new Vector3((x - (size.x / 2f)) * nodePrefab.transform.localScale.x, 0, (z - (size.y / 2f)) * nodePrefab.transform.localScale.z);
                MazeNode newNode = Instantiate(nodePrefab, nodePos + offsetParent + new Vector3(0.1f, 0, 0), transform.rotation, transform);
                Nodes.Add(newNode);
                newNode.name = "Maze node " + (Nodes.Count - 1);
                yield return null;
            }
        }
        Debug.Log(Nodes.Count);

    }
    IEnumerator generateSimpleMaze(List<MazeNode> Nodes, Vector2Int size)
    {

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(Nodes[Random.Range(0, Nodes.Count)]);
        currentPath[0].SetState(MazeNode.NodeState.Current);

        while (completedNodes.Count < Nodes.Count)
        {
            // Check Nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = Nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(Nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(Nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(Nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(Nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(Nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(Nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(Nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(Nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }
            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = Nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
                chosenNode.SetState(MazeNode.NodeState.Current);
                yield return null;
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(MazeNode.NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
                yield return null;
            }

        }
        yield return null;

    }
    void generateStartAndEnd(List<MazeNode> Nodes, Vector2Int size)
    {

        startNode = Nodes[Random.Range(0, size.y)];
        finishNode = Nodes[Random.Range(Nodes.Count - size.y, Nodes.Count)];
        startNode.Walls[1].tag = "StartWall";
        startNode.Walls[1].gameObject.GetComponent<MeshRenderer>().material = startMaterial;
        finishNode.Walls[0].tag = "FinishWall";
       // finishNode.Walls[0].gameObject.GetComponent<MeshRenderer>().material = finishMaterial;
        finishNode.Walls[0].gameObject.GetComponent<BoxCollider>().isTrigger=true;
        //  startNode.RemoveWall(1);
        //finishNode.RemoveWall(0);
    }
    IEnumerator makeComplexMaze(List<MazeNode> Nodes, Vector2Int size)
    {

        int countXHole = 0;
        int countZHole = 0;

        //for (int i = 0; i < Mathf.Max(size.x, size.y); i++)
        //{
        while (countXHole <= Mathf.Max(size.x, size.y) && countZHole <= Mathf.Max(size.x, size.y))
        {
            int index = Random.Range(0, Nodes.Count);
            //Random.Range(0,Nodes.Count);
            if ((index < Nodes.Count - mazeSize.y) && countXHole <= Mathf.Max(size.x, size.y))
            {
                //Debug.Log(index + " " + (index + mazeSize.y));
                if (Nodes[index].RemoveWall(0) || Nodes[index + mazeSize.y].RemoveWall(1))
                {
                    Nodes[index + mazeSize.y].RemoveWall(1);
                    Nodes[index].RemoveWall(0);
                    countXHole++;
                }
                //Debug.Log(index + " " + (index + mazeSize.y));
                yield return null;
            }
            if ((index % size.y != size.y - 1) && countZHole <= Mathf.Max(size.x, size.y))
            {
                if (Nodes[index].RemoveWall(2) || Nodes[index + 1].RemoveWall(3))
                {
                    Nodes[index + 1].RemoveWall(3);
                    Nodes[index].RemoveWall(2);
                    countZHole++;
                }
                // Debug.Log(index + " " + (index + 1));
                //Debug.Log(size.y+" "+(size.y-1) );
                yield return null;
            }
            Debug.Log(countXHole + " : " + countZHole);
        }//}
    }
    void supressOverlapWall()
    {
        for (int i = 0; i < Nodes.Count - 1; i++)
        {
            if ((i < Nodes.Count - mazeSize.y))
            {
                Nodes[i].RemoveWall(0);
            }
            if (i % mazeSize.y != mazeSize.y - 1)
            {
                Debug.Log(mazeSize);
                Nodes[i].RemoveWall(2);

            }
        }

    }




    IEnumerator setAllWallsPlayedState(List<MazeNode> Nodes, Vector2Int size)
    {
        foreach (MazeNode m in Nodes)
        {
            m.SetState(MazeNode.NodeState.Played);
            /*
            m.RemoveWall(2);
            m.RemoveWall(1);
*/
            yield return null;
        }
    }
    //  List<MeshFilter> meshFilters = new List<MeshFilter>();
    /*
        void meshFusion()
        {
            CombineInstance[] combine = new CombineInstance[meshFilters.Count];
            int i = 0;
            while (i < meshFilters.Count)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.gameObject.SetActive(true);
        }*/
    void setFloor()
    {
        plane = Instantiate(plane, new Vector3(0, transform.position.y / 8.3f, 0), Quaternion.identity, transform);
        plane.transform.localScale = new Vector3(MazeSize.x * nodeScale.x / 10f, 1, mazeSize.y * nodeScale.z / 10f);
    }

}



// Update is called once per frame
