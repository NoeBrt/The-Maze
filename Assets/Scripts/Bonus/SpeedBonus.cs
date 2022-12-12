using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float effectDuration = 10f;
    [SerializeField] float speedBonusGain = 5f;
    [SerializeField] AudioClip BonusSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            UIPlayerManager playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
            player.StopCoroutine("speedBonusEffect");
            player.GetComponentInChildren<AudioSource>().PlayOneShot(BonusSound, 0.1f);
            player.StartCoroutine(speedBonusEffect(effectDuration, player, playerUi));
            SettingManager.Instance.SfxSounds.Remove(GetComponent<AudioSource>());
            Destroy(gameObject);
        }
    }

    IEnumerator speedBonusEffect(float time, PlayerController player, UIPlayerManager playerUi)
    {
        playerUi.updateBonusText("Speed ++", time);
        player.speedGain = speedBonusGain;
        yield return new WaitForSeconds(time);
        player.speedGain = 0;
        //   playerUi.updateBonusText("");
    }

}
