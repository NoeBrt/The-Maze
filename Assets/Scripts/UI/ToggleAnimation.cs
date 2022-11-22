using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleAnimation : MonoBehaviour
{
    ToggleGroup toggleGroup;
    private void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    // Start is called before the first frame update
    // Update is called once per frame
    void SetAnimation()
    {
        //toggleGroup.GetFirstActiveToggle().GetComponent<Animator>().SetBool("On", true);

    }
}
