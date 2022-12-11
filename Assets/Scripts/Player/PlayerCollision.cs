using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] AudioClip preLooseSound;
    [SerializeField] EndScreenController endScreen;
    [SerializeField] UIPlayerManager playerUi;

    public bool IsCaughtByMonster { get; set; }
    bool firstCollision = true;

    private void Start()
    {
        endScreen = GameObject.Find("Canvas").GetComponent<EndScreenController>();
        playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
        playerUi.setVisible(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        MonsterCollisionHitBehavior(other.gameObject);
        EndTriggerBehavior(other);
    }

    void EndTriggerBehavior(Collider other)
    {
        if (other.gameObject.CompareTag("FinishWall") && firstCollision)
        {
            GetComponent<PlayerController>().enabled = false;
            endScreen.setEndTimer(playerUi.TimerText.text);
            playerUi.setVisible(false);
            firstCollision = false;
            // other.gameObject.transform.root.gameObject.SetActive(false);
            Time.timeScale = 0;
            GameManager.Instance.winCount++;
            GameManager.Instance.gameCount++;
            source.PlayOneShot(winSound);
            endScreen.setWinScreenVisible(true);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        MonsterCollisionHitBehavior(hit.gameObject);
    }

    void MonsterCollisionHitBehavior(GameObject monster)
    {
        if (monster.CompareTag("Monster") && firstCollision)
        {
            GetComponent<PlayerController>().enabled = false;
            firstCollision = false;
            monster.gameObject.transform.root.gameObject.SetActive(false);
            Time.timeScale = 0;
            GameManager.Instance.looseCount++;
            GameManager.Instance.gameCount++;
            StartCoroutine(looseBehavior(monster.gameObject));
        }
    }

    IEnumerator looseBehavior(GameObject monster)
    {
        playerUi.setVisible(false);
        source.PlayOneShot(preLooseSound);
        endScreen.displayMonsterEye(true);
        yield return new WaitForSecondsRealtime(1f);
        endScreen.displayMonsterEye(false);
        yield return new WaitForSecondsRealtime(1f);
        source.PlayOneShot(gameOverSound);
        endScreen.setLooseScreenVisible(true);

    }



}
