using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    #region var definitions

    [Header("Timer")]
    [SerializeField] private Text timerText;
    private float timer;
    private string infoTime = "";
    [Header("Bonus")]
    [SerializeField]
    private Text bonusText;
    private Coroutine previousBonusDisplay;
    private Coroutine previousTimerInfoDisplay;
    [SerializeField] private Text messageText;
    [SerializeField] GameObject pauseUi;

    public Text TimerText { get => timerText; set => timerText = value; }
    public GameObject PauseUi { get => pauseUi; set => pauseUi = value; }
    #endregion

    private void Start()
    {
        displayMessage("Find the key and get out", 3f);
    }
    public void setVisible(bool a)
    {
        gameObject.SetActive(a);
    }

    private void Update()
    {
        if (!pauseUi.activeSelf)
            setTimer();
        Debug.Log(infoTime);
    }


    public void setTimeScale(float a)
    {
        Time.timeScale = a;
        Cursor.lockState = a == 0 ? CursorLockMode.None : CursorLockMode.Locked;
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
        StartCoroutine(FadeTextIn(text, 1f, 0.3f));
        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeTextOut(text, 1f));

        // text.text = "";
    }

    private IEnumerator FadeTextIn(Text text, float fadeDuration, float opacity)
    {
        Color col = text.color;
        col.a = 0;
        text.color = col;
        while (col.a < opacity)
        {
            col.a += fadeDuration * Time.smoothDeltaTime;
            text.color = col;
            yield return new WaitForSeconds(fadeDuration * Time.smoothDeltaTime);
        }

    }
    private IEnumerator FadeTextOut(Text text, float fadeDuration)
    {
        Color col = text.color;
        float beginOpacity = col.a;
        while (col.a >= 0)
        {
            col.a -= fadeDuration * Time.smoothDeltaTime;
            text.color = col;
            yield return new WaitForSeconds(fadeDuration * Time.smoothDeltaTime);
        }
    }


}
