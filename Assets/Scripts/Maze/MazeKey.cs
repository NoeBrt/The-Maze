using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MazeKey : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject finalWall;
    Maze maze;
    [SerializeField] float rotateSpeed = 90f;
    [SerializeField] float monsterPatrolingSpeedGain = 5f;
        [SerializeField] float monsterChaseSpeedGain = 3f;


    [SerializeField] AudioClip keySound;
    void Start()
    {
        maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<Maze>();
        finalWall = maze.FinishNode.Walls[0];
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            maze.setFinishWallsSound();
            finalWall.GetComponent<MeshRenderer>().material = maze.FinishMaterial;
            finalWall.GetComponent<BoxCollider>().isTrigger = true;
            other.gameObject.transform.GetComponentInChildren<AudioSource>().PlayOneShot(keySound, 1f);
            UIPlayerManager playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
            SpawnManager spawn = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            foreach (GameObject monster in spawn.MonstersInScene)
            {
                monster.GetComponent<MonsterBehaviour>().PatrolingSpeed += monsterPatrolingSpeedGain;
                monster.GetComponent<MonsterBehaviour>().ChaseSpeed += monsterChaseSpeedGain;
            }
            playerUi.displayMessage("Key Founded", 5f);
            Destroy(gameObject);
        }
    }
}
