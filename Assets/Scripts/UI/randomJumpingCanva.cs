using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomJumpingCanva : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeStartTime = 0.04f;
    [SerializeField] Vector2 timeBetweenFadeRange = new Vector2(0.03f, 0.1f);
    [SerializeField] float fadeEndTime = 0.1f;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip soundFadeStart;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(jumpingCanvas());
    }

    // Update is called once per frame
    IEnumerator jumpingCanvas()
    {
        while (true)
        {
            canvasGroup.alpha = 1;
            yield return fadeCanvasStart(fadeStartTime);
            if (audioSource != null)
            {
                audioSource.PlayOneShot(soundFadeStart);
            }
            yield return new WaitForSeconds(Random.Range(timeBetweenFadeRange.x, timeBetweenFadeRange.y));
            yield return fadeCanvasEnd(fadeEndTime);
            yield return new WaitForSeconds(Random.Range(1f, 10f));
        }

    }
    IEnumerator fadeCanvasStart(float deltaTime)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 1 * deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }
    }
    IEnumerator fadeCanvasEnd(float deltaTime)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 * deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }
    }
}
