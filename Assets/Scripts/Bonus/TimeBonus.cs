using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBonus : MonoBehaviour
{
    [SerializeField] float timeToRemove = 10f;
    [SerializeField] float bonusEffectDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            UIPlayerManager playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
            playerUi.removeTime(timeToRemove);
            playerUi.updateBonusText("Time Bonus", bonusEffectDuration);
            Destroy(gameObject);
        }
    }
}
