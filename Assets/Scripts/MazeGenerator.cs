using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private Vector2Int mazeSize;
    [SerializeField] private Vector3 nodeScale;
    private Vector3 offsetParent;
    public MazeNode startNode;
    public MazeNode finishNode;
    private bool isFinished = false;
    private List<MazeNode> nodes;

    public Vector2Int MazeSize { get => mazeSize; set => mazeSize = value; }
    public Vector3 NodeScale { get => nodeScale; set => nodeScale = value; }
    public MazeNode StartNode { get => startNode; set => startNode = value; }
    public MazeNode FinishNode { get => finishNode; set => finishNode = value; }
    public bool IsFinished { get => isFinished; set => isFinished = value; }

    private void Start()
    {

        offsetParent = (transform.position + (Vector3.right * nodePrefab.transform.localScale.x / 2) + (Vector3.forward * nodePrefab.transform.localScale.z / 2));
        Debug.Log((Vector3.forward * nodePrefab.transform.localScale.z / 2f));

        nodePrefab.transform.localScale = nodeScale;
        StartCoroutine(GenerateMaze(mazeSize));
    }

    IEnumerator GenerateMaze(Vector2Int size)
    {
        nodes = new List<MazeNode>();
        yield return generateGrid(nodes, size);
        yield return generateSimpleMaze(nodes, size);
        generateStartAndEnd(nodes, size);
        yield return makeComplexMaze(nodes, size);
        Debug.Log("test");
        yield return setAllWallPlayedState(nodes, size);
        yield return new WaitForSeconds(3f);
        isFinished = true;


    }

    IEnumerator generateGrid(List<MazeNode> nodes, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3 nodePos = new Vector3((x - (size.x / 2f)) * nodePrefab.transform.localScale.x, 0, (z - (size.y / 2f)) * nodePrefab.transform.localScale.z);
                MazeNode newNode = Instantiate(nodePrefab, nodePos + offsetParent, transform.rotation, transform);
                nodes.Add(newNode);
                newNode.name = "Maze node " + (nodes.Count - 1);
                yield return null;

            }

        }
        Debug.Log(nodes.Count);

    }
    IEnumerator generateSimpleMaze(List<MazeNode> nodes, Vector2Int size)
    {

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
        currentPath[0].SetState(MazeNode.NodeState.Current);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }
            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

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
    void generateStartAndEnd(List<MazeNode> nodes, Vector2Int size)
    {

        startNode = nodes[Random.Range(0, size.y)];
        finishNode = nodes[Random.Range(nodes.Count - size.y, nodes.Count)];
        startNode.RemoveWall(1);
        finishNode.RemoveWall(0);
    }
    IEnumerator makeComplexMaze(List<MazeNode> nodes, Vector2Int size)
    {

        int countXHole = 0;
        int countZHole = 0;

        //for (int i = 0; i < Mathf.Max(size.x, size.y); i++)
        //{
        while (countXHole + countZHole <= Mathf.Max(size.x, size.y) * 2)
        {
            int index = Random.Range(0, nodes.Count);
            //Random.Range(0,nodes.Count);
            if ((index < nodes.Count - mazeSize.y) && countXHole <= Mathf.Max(size.x, size.y))
            {
                //Debug.Log(index + " " + (index + mazeSize.y));
                nodes[index].RemoveWall(0);
                nodes[index + mazeSize.y].RemoveWall(1);
                //Debug.Log(index + " " + (index + mazeSize.y));
                countXHole++;
                yield return null;
            }
            if ((index % size.y != size.y - 1) && countZHole <= Mathf.Max(size.x, size.y))
            {
                nodes[index].RemoveWall(2);
                nodes[index + 1].RemoveWall(3);
                // Debug.Log(index + " " + (index + 1));
                //Debug.Log(size.y+" "+(size.y-1) );
                countZHole++;
                yield return null;
            }
            Debug.Log(countXHole + " : " + countZHole);
        }//}
    }

    IEnumerator setAllWallPlayedState(List<MazeNode> nodes, Vector2Int size)
    {
        foreach (MazeNode m in nodes)
        {
            m.SetState(MazeNode.NodeState.Played);
            yield return null;

        }
    }

}



// Update is called once per frame
