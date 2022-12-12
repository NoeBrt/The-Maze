using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MazeGenerator : MonoBehaviour
{

    static Vector3 offsetParent;
    public static Maze mazePrefab;
    public static MazeNode nodePrefab;
    static bool isProgressive;


    public static Maze GenerateMaze(Vector2Int size, Vector3 nodeScale, Vector3 position, Quaternion rotation, bool isProgressive)
    {
        MazeGenerator.isProgressive = isProgressive;

        mazePrefab = Resources.Load<GameObject>("Maze").GetComponent<Maze>();
        nodePrefab = Resources.Load<GameObject>("MazeNode").GetComponent<MazeNode>();

        Maze maze = Instantiate(mazePrefab, position, rotation);
        if (!isProgressive)
        {
            //  nodePrefab.SetState(MazeNode.NodeState.Played);
            //    nodePrefab.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            //  nodePrefab.GetComponent<MeshRenderer>().enabled = true;
        }
        maze.name = "Maze";
        maze.NodeScale = nodeScale;
        maze.Size = size;
        offsetParent = (maze.transform.position + (Vector3.right * nodePrefab.transform.localScale.x / 2) + (Vector3.forward * nodePrefab.transform.localScale.z / 2));
        nodePrefab.transform.localScale = nodeScale;
        maze.StartCoroutine(GenerateMazeEnumerator(maze));
        return maze;
    }


    static IEnumerator GenerateMazeEnumerator(Maze maze)
    {

        maze.Plane.SetActive(true);
        maze.Nodes = new List<MazeNode>();
        yield return generateGrid(maze.Nodes, maze.Size, maze.transform);
        yield return generateSimpleMaze(maze.Nodes, maze.Size);
        maze.StartNode = getStart(maze.Nodes, maze.Size);
        maze.FinishNode = getEnd(maze.Nodes, maze.Size);
        yield return makeComplexMaze(maze.Nodes, maze.Size);
        Debug.Log("test");
        yield return setAllWallsPlayedState(maze.Nodes, maze.Size);
        if (isProgressive)
            yield return new WaitForSeconds(2f);
        setFloor(maze);
        supressOverlapWall(maze.Nodes, maze.Size);
        // meshFusion(maze);
        if (!isProgressive)
        {
            maze.setNodesVisibility(true);
        }
        maze.IsFinished = true;
        meshFusion(maze);
    }


    static IEnumerator generateGrid(List<MazeNode> Nodes, Vector2Int size, Transform parent)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3 nodePos = new Vector3((x - (size.x / 2f)) * nodePrefab.transform.localScale.x, 0, (z - (size.y / 2f)) * nodePrefab.transform.localScale.z);
                MazeNode newNode = Instantiate(nodePrefab, nodePos + offsetParent + new Vector3(0.1f, 0, 0), parent.rotation, parent);
                Nodes.Add(newNode);
                newNode.name = "Maze node " + (Nodes.Count - 1);
                if (isProgressive && Nodes.Count % (GameManager.Instance.gameCount + 1) == 0)
                    yield return new WaitForSeconds(1f / (size.x * size.y * 2) * (GameManager.Instance.gameCount + 1)); ;
            }
        }

    }

    static IEnumerator generateSimpleMaze(List<MazeNode> Nodes, Vector2Int size)
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
                if (isProgressive)
                    yield return new WaitForSeconds(1f / ((Nodes.Count * 2) * (GameManager.Instance.gameCount + 1))); ;
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(MazeNode.NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
                if (isProgressive)
                    yield return new WaitForSeconds(1f / ((Nodes.Count * 2) * (GameManager.Instance.gameCount + 1)));
            }

        }
        if (isProgressive)
            yield return new WaitForSeconds(1f / ((Nodes.Count * 2) * (GameManager.Instance.gameCount + 1)));

    }
    static MazeNode getStart(List<MazeNode> Nodes, Vector2Int size)
    {
        MazeNode startNode;
        startNode = Nodes[Random.Range(0, size.y)];
        startNode.Walls[1].tag = "StartWall";
        return startNode;
    }
    static MazeNode getEnd(List<MazeNode> Nodes, Vector2Int size)
    {
        MazeNode finishNode;
        finishNode = Nodes[Random.Range(Nodes.Count - size.y, Nodes.Count)];
        finishNode.tag = "FinishNode";
        finishNode.Walls[0].tag = "FinishWall";
        // finishNode.Walls[0].gameObject.GetComponent<BoxCollider>().isTrigger = false;
        return finishNode;
    }





    static IEnumerator makeComplexMaze(List<MazeNode> Nodes, Vector2Int size)
    {

        int countXHole = 0;
        int countZHole = 0;

        //for (int i = 0; i < Mathf.Max(size.x, size.y); i++)
        //{
        while (countXHole <= Mathf.Max(size.x, size.y) && countZHole <= Mathf.Max(size.x, size.y))
        {
            int index = Random.Range(0, Nodes.Count);
            //Random.Range(0,Nodes.Count);
            if ((index < Nodes.Count - size.y) && countXHole <= Mathf.Max(size.x, size.y))
            {
                //Debug.Log(index + " " + (index + mazeSize.y));
                if (Nodes[index].RemoveWall(0) || Nodes[index + size.y].RemoveWall(1))
                {
                    Nodes[index + size.y].RemoveWall(1);
                    Nodes[index].RemoveWall(0);
                    countXHole++;
                }
                //Debug.Log(index + " " + (index + mazeSize.y));
                if (isProgressive)
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
                if (isProgressive)
                    yield return null;
            }
            Debug.Log(countXHole + " : " + countZHole);
        }//}
    }

    static void supressOverlapWall(List<MazeNode> Nodes, Vector2Int size)
    {
        for (int i = 0; i < Nodes.Count - 1; i++)
        {
            if ((i < Nodes.Count - size.y))
            {
                Nodes[i].RemoveWall(0);
            }
            if (i % size.y != size.y - 1)
            {
                Debug.Log(size);
                Nodes[i].RemoveWall(2);

            }
        }

    }




    static IEnumerator setAllWallsPlayedState(List<MazeNode> Nodes, Vector2Int size)
    {
        meshFilters = new List<MeshFilter>();
        int i = 0;
        foreach (MazeNode m in Nodes)
        {
            i++;
            m.SetState(MazeNode.NodeState.Played);
            /*
            m.RemoveWall(2);
            m.RemoveWall(1);
    */
            if (m.CompareTag("FinishNode"))
            {
                foreach (GameObject wall in m.Walls)
                {
                    if (wall != null && !wall.CompareTag("FinishWall"))
                        meshFilters.Add(wall.GetComponent<MeshFilter>());
                }
            }
            else
                meshFilters.AddRange(m.GetComponentsInChildren<MeshFilter>());
            if (isProgressive && i % (GameManager.Instance.gameCount + 1) == 0)
                yield return null;
        }
    }
    static List<MeshFilter> meshFilters = new List<MeshFilter>();

    static void meshFusion(Maze maze)
    {
        //  meshFilters.Add(maze.Plane.GetComponent<MeshFilter>());
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            Matrix4x4 mat = meshFilters[i].transform.localToWorldMatrix;
            mat[1, 3] -= 25;
            combine[i].transform = mat;
            meshFilters[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            meshFilters[i].transform.parent.gameObject.SetActive(false);
            meshFilters[i].transform.gameObject.SetActive(false);
            i++;
        }
        maze.GetComponent<MeshFilter>().mesh = new Mesh();
        maze.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true, true);
        maze.GetComponent<MeshFilter>().mesh.Optimize();
        maze.GetComponent<MeshCollider>().sharedMesh = maze.GetComponent<MeshFilter>().mesh;
        maze.FinishNode.Walls.ToList().Find(wall => wall.CompareTag("FinishWall")).GetComponent<MeshRenderer>().material = maze.GetComponent<MeshRenderer>().material;
        maze.FinishNode.gameObject.SetActive(true);
        maze.gameObject.SetActive(true);
    }
    static void setFloor(Maze maze)
    {
        maze.Plane = Instantiate(maze.Plane, new Vector3(0, maze.transform.position.y / 8.3f, 0), Quaternion.identity, maze.transform);
        maze.Plane.transform.localScale = new Vector3(maze.Size.x * maze.NodeScale.x / 10f, 1, maze.Size.y * maze.NodeScale.z / 10f);
    }

}



// Update is called once per frame
