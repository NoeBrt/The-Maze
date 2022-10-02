using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGenerator : MonoBehaviour
{
    private int x;
    private int y;

    List<int> wall;
    List<int> line;
    List<List<int>> maze;
    public GameObject objectWall;


    public MazeGenerator(int x, int y, GameObject objectWall1)
    {
    Debug.Log("test");
        wall = new List<int>();
        line = new List<int>();
        this.objectWall = objectWall1;
        this.x=x;
        this.y=y;
        createGrid();
    }

    void createGrid()
    {
        int nb = 0;
        for (int i = 0; i < x; i++)
        {
            wall.Add(-1);
            if (i % 2 == 1)
            {
                line.Add(0);
            }
            else
            {
                line.Add(-1);
            }
        }
        for (int i = 0; i < y; i++)
        {
            if (i % 2 == 0)
            {
                maze.Add(wall);
            }
            else
            {
                maze.Add(line);
            }
        }
        for (int i = 0; i < x; i++)
        {
            for (int i1 = 0; i1 < y; i1++)
            {
                if (maze[i][i1] == 0)
                {
                    nb++;
                    maze[i][i1] = nb;
                }
            }
        }

        maze[1][0] = 0;
        maze[x - 1][y - 1] = 0;


    }

    void generateMaze()
    {
        createGrid();
        while (isFinished() == false)
        {
            int wallX = (int)Random.Range(1, x - 1);
            int wallY;
            if (wallX % 2 == 0)
            {
                wallY = (int)Random.Range(0, (y - 1) / 2) * 2 + 1;
            }
            else
            {
                wallY = (int)Random.Range(0, (y - 2) / 2) * 2 + 2;
            }

            int cell1;
            int cell2;

            if (maze[wallX - 1][wallY] == -1)
            {
                cell1 = maze[wallX][wallY - 1];
                cell2 = maze[wallX][wallY + 1];
            }
            else
            {
                cell1 = maze[wallX - 1][wallY];
                cell2 = maze[wallX + 1][wallY];
            }
            if (cell1 != cell2)
            {
                maze[wallX][wallY] = 0;


                for (int i = 0; i < x; i++)
                {
                    for (int i1 = 0; i1 < y; i1++)
                    {
                        if (maze[i][i1] == cell2)
                        {
                            maze[i][i1] = cell1;
                        }
                    }
                }



            }




        }
        for (int i = 0; i < x; i++)
        {
            int wallX = (int)Random.Range(1, x - 1);
            int wallY;
            if (wallX % 2 == 0)
            {
                wallY = (int)Random.Range(0, (y - 1) / 2) * 2 + 1;
            }
            else
            {
                wallY = (int)Random.Range(0, (y - 2) / 2) * 2 + 2;
            }
            maze[wallX][wallY] = 0;
        }

    }

    bool isFinished()
    {
        int value;
        for (int i = 0; i < x - 1; i++)
        {
            for (int i1 = 0; i1 < y; i1++)
            {
                if (maze[i][i1] > -1)
                {
                    value = maze[i][i1];
                    if (maze[i][i1] != value)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void showMaze()
    {
        for (int i = 0; i < x; i++)
        {
            for (int i1 = 0; i1 < y; i1++)
            {
                if (maze[i][i1] == -1)
                {
                    Instantiate(objectWall, new Vector3(i, 0.5f, i1), Quaternion.identity);
                }
            }

        }
    }


}



// Update is called once per frame
