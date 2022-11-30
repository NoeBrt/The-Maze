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
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    
}
