using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        float timeToRemove = 10f;
        float bonusEffectDuration = 10f;
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.PlayerUi.removeTime(timeToRemove);
            player.PlayerUi.updateBonusText("Time Bonus", bonusEffectDuration);
            Destroy(gameObject);

        }
    }
}
