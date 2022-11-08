using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region var definitions

    [Header("Timer")]
    [SerializeField]
    private Text timerText;
    private float timer;
    private string infoTime = "";
    [Header("Bonus")]
    [SerializeField]
    private Text bonusText;
    private Coroutine previousBonusDisplay;
    private Coroutine previousTimerInfoDisplay;

    #endregion

    private void FixedUpdate()
    {
        setTimer();
        Debug.Log(infoTime);
    }


    public void setTimer()
    {
        timer += Time.fixedDeltaTime;
        int minutes = (int)(timer / 60f);
        int second = (int)(timer % 60f);
        int milliSecond = (int)((timer * 100f) % 100f);
        timerText.text = minutes.ToString("00") + ":" + second.ToString("00") + ":" + milliSecond.ToString("00") + " " + infoTime;
    }
    public void resetTimer()
    {
        timer = 0;
    }
    public void addTime(float time)
    {
        timer += time;
        StartCoroutine(addTimerInfo(3, " +" + time));
    }

    public void removeTime(float time)
    {
        if (timer < time)
        {
            timer = 0;
        }
        else
        {
            timer -= time;
        };
        StartCoroutine(addTimerInfo(3, " -" + time));
        // timerText.text += " -" + time;


    }
    private IEnumerator addTimerInfo(float delay, string s)
    {
        infoTime = s;
        yield return new WaitForSeconds(delay);
        infoTime = "";
    }


    public void updateBonusText(string s)
    {
        bonusText.text = s;
    }

    public void updateBonusText(string s, float second)
    {
        StartCoroutine(displayBonusText(second, s));
    }

    private IEnumerator displayBonusText(float seconds, string s)
    {
        bonusText.text = s;
        yield return new WaitForSeconds(seconds);
        bonusText.text = "";
    }


}
