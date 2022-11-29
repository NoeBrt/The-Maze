using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
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
    [SerializeField] private Text messageText;
    #endregion

   public void setVisible(bool a)
    {
        gameObject.SetActive(a);
    }

    private void FixedUpdate()
    {
        setTimer();
        Debug.Log(infoTime);
    }


    public void setTimer()
    {
        timer += Time.fixedDeltaTime;
        int minutes = (int)(timer / 60f);
        int seconds = (int)(timer % 60f);
        int milliSecond = (int)((timer * 100f) % 100f);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliSecond.ToString("00") + " " + infoTime;
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

    public void updateBonusText(string s, float seconds)
    {
        StartCoroutine(displayText(bonusText, seconds, s));
    }

    public void displayMessage(string s, float seconds)
    {
        StartCoroutine(displayText(messageText, seconds, s));
    }

    private IEnumerator displayText(Text text, float seconds, string s)
    {
        text.text = s;
        yield return new WaitForSeconds(seconds);
        text.text = "";
    }


}
