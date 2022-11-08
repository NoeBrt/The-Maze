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
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            StopCoroutine("speeBonusEffect");
            StartCoroutine(speedBonusEffect(effectDuration, player));
            Destroy(gameObject);
        }
    }

    IEnumerator speedBonusEffect(float time, PlayerController player)
    {
        player.PlayerUi.updateBonusText("Speed ++");
        player.WalkSpeed += speedBonusGain;
        player.SprintSpeed += speedBonusGain;
        yield return new WaitForSeconds(time);
        player.WalkSpeed -= speedBonusGain;
        player.SprintSpeed -= speedBonusGain;
        player.PlayerUi.updateBonusText("");
    }

}
