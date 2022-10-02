using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public class Cell
    {
        bool visited;
        public GameObject north;
        public GameObject east;
        public GameObject west;
        public GameObject south;


    }
    public GameObject wallObject;
    public float wallLenght = 1.0f;
    public int xLenght;
    public int yWidht;
    public Vector3 initialPos;
    public Cell[] cells;
    // Start is called before the first frame update
    void Start()
    {
        CreateWall();
    }

    void CreateWall()
    {
        GameObject tempWall;
        initialPos = new Vector3(0, 0f, 0);
        Vector3 pos = initialPos;
        for (int i = 0; i < xLenght; i++)
        {
            for (int j = 0; j <= yWidht; j++)
            {
                pos = new Vector3(initialPos.x + j * wallLenght, 0.0f, initialPos.z + i * wallLenght);
                tempWall = Instantiate(wallObject, pos, Quaternion.identity);
                tempWall.transform.parent = transform;
            }

        }
        for (int i = 0; i <= xLenght; i++)
        {
            for (int j = 0; j < yWidht; j++)
            {
                pos = new Vector3(initialPos.x + j * wallLenght + wallLenght / 2, 0.0f, initialPos.z + i * wallLenght - wallLenght / 2);
                tempWall = Instantiate(wallObject, pos, Quaternion.Euler(0f, 90.0f, 0));
                tempWall.transform.parent = transform;

            }
        }
        //createCells();
    }
/*
    void createCells(){
        int childCount=transform.childCount;
        int eastWestProcess=0;
        GameObject[] allWalls= new GameObject[childCount];
        cells= new Cell[xLenght*yWidht];
        for (int i=0;i<childCount;i++){
            allWalls[i]=transform.GetChild(i).gameObject;

        }
     for (int cellprocess=0;cellprocess<cells.Length;cellprocess++){
        cells[cellprocess]=new Cell();
        cells[cellprocess].east=allWalls[eastWestProcess];
        cells[cellprocess].south=allWalls[];
     }

    }*/

    // Update is called once per frame
    void Update()
    {

    }
}
