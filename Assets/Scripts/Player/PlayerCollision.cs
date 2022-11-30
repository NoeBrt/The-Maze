using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip preLooseSound;
    [SerializeField] EndScreenController endScreen;
    public bool IsCaughtByMonster { get; set; }
    bool firstCollision = true;

    private void Start()
    {
        endScreen = GameObject.Find("Canvas").GetComponent<EndScreenController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
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
        endScreen.displayMonsterEye(true);
        yield return new WaitForSecondsRealtime(1f);
        endScreen.displayMonsterEye(false);
        yield return new WaitForSecondsRealtime(1f);
        source.PlayOneShot(preLooseSound);
        endScreen.setLooseScreenVisible(true);

    }



}
