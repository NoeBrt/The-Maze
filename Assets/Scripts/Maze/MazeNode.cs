using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MazeNode : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;
    [SerializeField] Material wallMaterial;
    [SerializeField] Material floorMaterial;
    public GameObject[] Walls { get => walls; set => walls = value; }
    public enum NodeState
    {
        Available,
        Current,
        Completed,
        Played,
    }

    public bool RemoveWall(int wallToRemove)
    {
        //walls[wallToRemove].gameObject.SetActive(false);
        if (walls[wallToRemove] != null)
        {
            Destroy(walls[wallToRemove].gameObject);
            return true;
        }
        else return false;

    }
    // Start is called before the first frame update
    public void SetState(NodeState state)
    {
        switch (state)
        {

            case NodeState.Available:
                floor.gameObject.SetActive(true);
                floor.material.color = Color.white;
                break;
            case NodeState.Current:
                floor.gameObject.SetActive(true);
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.gameObject.SetActive(true);
                floor.material.color = Color.green;
                break;
            case NodeState.Played:
                floor.gameObject.SetActive(false);
                for (int i = 0; i < 4; i++)
                {
                    if (walls[i] != null && walls[i].tag != "StartWall" && walls[i].tag != "FinishWall")
                    {
                        walls[i].GetComponent<MeshRenderer>().material = wallMaterial;
                    }
                }
                break;
        }
    }

}
