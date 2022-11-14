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
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.PlayerUi.removeTime(timeToRemove);
            player.PlayerUi.updateBonusText("Time Bonus", bonusEffectDuration);
            Destroy(gameObject);

        }
    }
}
