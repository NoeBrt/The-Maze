using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float effectDuration = 10f;
    [SerializeField] float speedBonusGain = 7f;
    [SerializeField] AudioClip BonusSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            UIPlayerManager playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
            player.StopCoroutine("speedBonusEffect");
            player.GetComponentInChildren<AudioSource>().PlayOneShot(BonusSound,0.5f);
            player.StartCoroutine(speedBonusEffect(effectDuration, player, playerUi));
            Destroy(gameObject);
        }
    }

    IEnumerator speedBonusEffect(float time, PlayerController player, UIPlayerManager playerUi)
    {
        playerUi.updateBonusText("Speed ++");
        player.WalkSpeed += speedBonusGain;
        player.SprintSpeed += speedBonusGain;
        yield return new WaitForSeconds(time);
        player.WalkSpeed -= speedBonusGain;
        player.SprintSpeed -= speedBonusGain;
        playerUi.updateBonusText("");
    }

}
