using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFade : MonoBehaviour
{

    public static void FadeIn(float finalVolume, AudioSource source, MonoBehaviour instance)
    {
        instance.StartCoroutine(FadeIn(finalVolume, source));
    }
    public static void FadeOut(float finalVolume, AudioSource source, MonoBehaviour instance)
    {
        instance.StartCoroutine(FadeOut(finalVolume, source));
    }


    static IEnumerator FadeOut(float finalVolume, AudioSource source)
    {
        while (source.volume < finalVolume)
        {
            source.volume += finalVolume * Time.smoothDeltaTime;
            yield return new WaitForSeconds(finalVolume * Time.smoothDeltaTime);
        }
    }
    public static IEnumerator FadeIn(float delay, AudioSource source)
    {
        while (source.volume >= 0f)
        {
            source.volume -= delay * Time.smoothDeltaTime;
            yield return new WaitForSeconds(delay * Time.smoothDeltaTime);
        }
    }
}
