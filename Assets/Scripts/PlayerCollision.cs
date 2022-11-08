using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private UIManager uiManager;
    private PlayerController player;
    private WinScreenController EndScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBonus"))
        {
            Destroy(other.gameObject);
            StartCoroutine(speedBonusEffect(bonusEffectDuration));
            uiManager.updateBonusText("Speed Bonus", bonusEffectDuration);
        }
        else if (other.CompareTag("TimeBonus"))
        {
            Destroy(other.gameObject);
            uiManager.removeTime(timeToRemove);
            uiManager.updateBonusText("Time Bonus", bonusEffectDuration);
        }
        else if (other.CompareTag("FinishWall"))
        {
            EndScreen.setWinScreenVisible(true);
            Cursor.lockState = CursorLockMode.None;

        }
        else if (other.CompareTag("monster"))
        {
            EndScreen.setLooseScreenVisible(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (other.CompareTag("key"))
        {

        }
    }
    IEnumerator speedBonusEffect(float time)
    {
        uiManager.updateBonusText("Speed ++");
        walkSpeed += speedBonusGain;
        sprintSpeed += speedBonusGain;
        yield return new WaitForSeconds(time);
        walkSpeed -= speedBonusGain;
        sprintSpeed -= speedBonusGain;
        uiManager.updateBonusText("");
    }

}