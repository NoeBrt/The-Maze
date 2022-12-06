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
    public bool IsCaughtByMonster { get; set; }
    bool firstCollision = true;

    private void Start()
    {
        endScreen = GameObject.Find("Canvas").GetComponent<EndScreenController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EndTriggerBehavior(other);
    }

    void EndTriggerBehavior(Collider other)
    {
        if (other.gameObject.CompareTag("FinishWall") && firstCollision)
        {
            endScreen.setEndTimer(transform.Find("PlayerCanvas").GetComponent<UIPlayerManager>().TimerText.text);
            transform.Find("PlayerCanvas").GetComponent<UIPlayerManager>().setVisible(false);
            firstCollision = false;
            other.gameObject.transform.root.gameObject.SetActive(false);
            Time.timeScale = 0;
            GameManager.Instance.looseCount++;
            source.PlayOneShot(gameOverSound);
            endScreen.setWinScreenVisible(true);
        }
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        MonsterCollisionHitBehavior(hit);
    }
    void MonsterCollisionHitBehavior(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Monster") && firstCollision)
        {
            firstCollision = false;
            hit.collider.gameObject.transform.root.gameObject.SetActive(false);
            Time.timeScale = 0;
            GameManager.Instance.looseCount++;
            StartCoroutine(looseBehavior(hit.collider.gameObject));
        }
    }

    IEnumerator looseBehavior(GameObject monster)
    {
        transform.Find("PlayerCanvas").GetComponent<UIPlayerManager>().setVisible(false);
        source.PlayOneShot(preLooseSound);
        endScreen.displayMonsterEye(true);
        yield return new WaitForSecondsRealtime(1f);
        endScreen.displayMonsterEye(false);
        yield return new WaitForSecondsRealtime(1f);
        source.PlayOneShot(gameOverSound);
        endScreen.setLooseScreenVisible(true);

    }



}
