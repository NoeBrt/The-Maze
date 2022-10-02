using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;
    public enum NodeState
    {
        Available,
        Current,
        Completed
    }


    public void RemoveWall(int wallToRemove){
       //walls[wallToRemove].gameObject.SetActive(false);
        Destroy(walls[wallToRemove].gameObject);

    }
    // Start is called before the first frame update
    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white;
                break;
            case NodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.material.color = Color.green;
                break;

        }
    }
}
