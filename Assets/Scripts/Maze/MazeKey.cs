using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeKey : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject finalWall;
    Maze maze;
    [SerializeField] float rotateSpeed = 90f;
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
            playerUi.displayMessage("Key Founded", 5f);
            Destroy(gameObject);
        }
    }
}
