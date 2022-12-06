using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndScreenController : MonoBehaviour
{
    [SerializeField] SpawnManager spawn;
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LooseScreen;
    [SerializeField] Text TimerText;
    [SerializeField] GameObject monsterImage;


    // Start is called before the first frame update

    void setVisible(bool v)
    {
        gameObject.SetActive(v);

    }
    public void displayMonsterEye(bool a)
    {
        if (!a)
        {
            monsterImage.GetComponentInChildren<Animator>().SetTrigger("FadeOut");
        }
        else
        {
            monsterImage.SetActive(a);
        }

    }
    public void setEndTimer(string timer)
    {
        TimerText.text = "Time : " + timer;
    }

    public void setWinScreenVisible(bool v)
    {
        WinScreen.SetActive(v);
        if (v)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
        }

    }
    public void setLooseScreenVisible(bool v)
    {
        LooseScreen.SetActive(v);
        if (v)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

    }

    public void nexLevelButton()
    {
        /*
        monsterImage.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        spawn.spawnMaze(spawn.nodeScale, new Vector2Int(Random.Range(spawn.mazeSize.x, spawn.mazeSize.x + 5), Random.Range(spawn.mazeSize.x, spawn.mazeSize.y + 5)));
        WinScreen.SetActive(false);*/
    }
    public void restartButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.currentMazeSize = new Vector2Int(Random.Range(spawn.mazeSize.x, spawn.mazeSize.x + 5), Random.Range(spawn.mazeSize.x, spawn.mazeSize.y + 5));
        /*
        monsterImage.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        spawn.spawnMaze(spawn.nodeScale, spawn.mazeSize);
        LooseScreen.SetActive(false);*/

    }
}
