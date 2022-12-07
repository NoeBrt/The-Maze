using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class ToggleAnimation : MonoBehaviour
{
    ToggleGroup toggleGroup;
    [SerializeField] List<Toggle> Toggles;

    private void Start()
    {
        init();
    }
    private void OnEnable()
    {
        init();
    }
    void init()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        foreach (Toggle toggle in Toggles)
        {
            toggle.GetComponent<Animator>().SetBool("On", toggle.isOn);
            if (toggle.isOn)
            {
                toggle.GetComponent<Animator>().SetTrigger("On1");
            }
        }
    }

    public void SetAnimation()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.GetComponent<Animator>())
        {
            Toggle currentToogle = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
            bool currentState = currentToogle.GetComponent<Animator>().GetBool("On");
            currentToogle.GetComponent<Animator>().SetBool("On", currentToogle.isOn);
            if (currentToogle.isOn)
            {
                currentToogle.GetComponent<Animator>().SetTrigger("On1");
            }
        }
        foreach (Toggle toggle in Toggles)
        {
            toggle.GetComponent<Animator>().SetBool("On", toggle.isOn);
        }
    }
}
