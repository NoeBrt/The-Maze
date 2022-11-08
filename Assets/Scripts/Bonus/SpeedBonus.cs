using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float effectDuration = 10f;
    [SerializeField] float speedBonusGain = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>();
        }
    }

    IEnumerator speedBonusEffect(float time, PlayerController player)
    {
        uiManager.updateBonusText("Speed ++");
        player.WalkSpeed += speedBonusGain;
        player.SprintSpeed += speedBonusGain;
        yield return new WaitForSeconds(time);
        player.WalkSpeed -= speedBonusGain;
        player.SprintSpeed -= speedBonusGain;
        player.UiManager.updateBonusText("");
    }

}
