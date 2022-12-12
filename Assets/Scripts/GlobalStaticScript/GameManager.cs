using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int looseCount { get; set; } = 0;
    public int winCount { get; set; } = 0;
    public int gameCount { get; set; } = 0;

    public Vector2Int currentMazeSize { get; set; } = new Vector2Int(10, 10);


    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }
}
