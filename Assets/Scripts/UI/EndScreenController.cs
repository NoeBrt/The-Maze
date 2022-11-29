using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] SpawnManager spawn;
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LooseScreen;
    [SerializeField] GameObject monsterImage;


    // Start is called before the first frame update

    void setVisible(bool v)
    {
        gameObject.SetActive(v);

    }
    public void displayMonsterImage(bool a)
    {
        monsterImage.SetActive(a);
    }


    public void setWinScreenVisible(bool v)
    {
        WinScreen.SetActive(v);
        if (v)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
        }

    }
    public void setLooseScreenVisible(bool v)
    {
        LooseScreen.SetActive(v);
        if (v)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
        }

    }

    public void nexLevelButton()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        spawn.spawnMaze(spawn.nodeScale, new Vector2Int(Random.Range(spawn.mazeSize.x, spawn.mazeSize.x + 5), Random.Range(spawn.mazeSize.x, spawn.mazeSize.y + 5)));
        WinScreen.SetActive(false);
    }
    public void restartButton()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        spawn.spawnMaze(spawn.nodeScale, spawn.mazeSize);
        LooseScreen.SetActive(false);

    }
}
