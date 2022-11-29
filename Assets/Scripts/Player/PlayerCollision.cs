using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip preLooseSound;
    [SerializeField] EndScreenController endScreen;
    private void Start() {
        endScreen=GameObject.Find("Canvas").GetComponent<EndScreenController>();
    }
    public bool IsCaughtByMonster { get; set; }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            GetComponent<UIPlayerManager>().setVisible(false);
            endScreen.displayMonsterImage(true);
            source.PlayOneShot(preLooseSound);
            endScreen.setLooseScreenVisible(true);
        }
    }




}
