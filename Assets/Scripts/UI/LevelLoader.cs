using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime;
    public void LoadLevelTransition(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("End");
        //   SoundFade.FadeIn(0, SettingManager.Instance.MusicSource, this);
        SoundFade.FadeIn(1.12f, SettingManager.Instance.MusicSource, this);
        if (SettingManager.Instance.SfxSounds.Count > 0)
        {
            foreach (AudioSource source in SettingManager.Instance.SfxSounds)
            {
                if (source != null)
                    SoundFade.FadeIn(1.12f, source, this);
            }
        }
        yield return new WaitForSecondsRealtime(1.12f);
        SceneManager.LoadScene(levelIndex);
    }

}
