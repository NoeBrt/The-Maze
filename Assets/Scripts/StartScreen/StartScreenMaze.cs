using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenMaze : MonoBehaviour
{
    [SerializeField] Camera cam;
    Maze startMaze;
    [SerializeField] float speed;
    bool outofBound = false;
    // Start is called before the first frame update
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

    }
    private void Start()
    {
        startMaze = GetComponent<Maze>();
        startMaze.Nodes = new List<MazeNode>();
        startMaze.Nodes.AddRange(GetComponentsInChildren<MazeNode>());
        Debug.Log(startMaze.Nodes);
        foreach (MazeNode m in startMaze.Nodes)
        {
            meshFilters.AddRange(m.gameObject.GetComponentsInChildren<MeshFilter>());
        }
        // meshFusion();
    }
    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x >= 35 && !outofBound)
        {
            outofBound = true;
            Instantiate(startMaze, startMaze.transform.position + Vector3.right * -startMaze.Size.x * startMaze.NodeScale.x, startMaze.transform.rotation).name = "startMaze2";
        }
        if (transform.position.x >= startMaze.Size.x * startMaze.NodeScale.x)
        {
            Destroy(gameObject);
        }
    }
    List<MeshFilter> meshFilters = new List<MeshFilter>();

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
        transform.position = startMaze.transform.position;
    }
    // Update is called once per frame

}
