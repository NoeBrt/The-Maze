using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpawnManager spawn;
    // Start is called before the first frame update
    public void setVisible(bool v)
    {
        gameObject.SetActive(v);
        if (v)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
        }

    }

    public void restartButton()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        spawn.spawnMaze(spawn.nodeScale,spawn.mazeSize);
        setVisible(false);

    }
}
