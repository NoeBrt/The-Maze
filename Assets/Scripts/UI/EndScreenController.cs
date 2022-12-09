using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndScreenController : MonoBehaviour
{
    [SerializeField] SpawnManager spawn;
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LooseScreen;
    [SerializeField] Text TimerText;
    [SerializeField] GameObject monsterImage;
    [SerializeField] Text scoreMonster;
    [SerializeField] Text scorePlayer;
    [SerializeField] GameObject scoreScreen;



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
        Time.timeScale = 1;
        GameManager.Instance.currentMazeSize = new Vector2Int(Random.Range(spawn.mazeSize.x, spawn.mazeSize.y + 3), Random.Range(spawn.mazeSize.x, spawn.mazeSize.y + 3));
        StartCoroutine(DisplayScoreAndLoadScene(SceneManager.GetActiveScene().buildIndex, 2f));
    }

    public void restartButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.currentMazeSize = new Vector2Int(Random.Range(spawn.mazeSize.x, spawn.mazeSize.x), Random.Range(spawn.mazeSize.y, spawn.mazeSize.y));
        StartCoroutine(DisplayScoreAndLoadScene(SceneManager.GetActiveScene().buildIndex, 2f));
    }

    IEnumerator DisplayScoreAndLoadScene(int index, float delay)
    {
        scoreMonster.text = GameManager.Instance.looseCount + "";
        scorePlayer.text = GameManager.Instance.winCount + "";
        scorePlayer.transform.parent.gameObject.SetActive(true);
        scoreScreen.SetActive(true);
        yield return new WaitForSeconds(delay);
        GetComponent<LevelLoader>().LoadLevelTransition(index);
    }
}
