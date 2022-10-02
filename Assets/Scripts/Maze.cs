using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public GameObject wallObject;
    public float wallLenght = 1.0f;
    public int xLenght;
    public int yWidht;
    public Vector3 initialPos;
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
