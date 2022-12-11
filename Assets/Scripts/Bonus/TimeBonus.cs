using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBonus : MonoBehaviour
{
    [SerializeField] float timeToRemove = 10f;
    [SerializeField] float bonusEffectDuration = 3f;
    [SerializeField] AudioClip BonusSound;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            UIPlayerManager playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
            playerUi.removeTime(timeToRemove);
            other.gameObject.transform.GetComponentInChildren<AudioSource>().PlayOneShot(BonusSound, 1f);
            playerUi.updateBonusText("Time Bonus", bonusEffectDuration);
            Destroy(gameObject);
        }
    }
}
