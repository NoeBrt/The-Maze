using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip lightToggleOnFX;
    [SerializeField] AudioClip lightToggleOffFX;
    [SerializeField] private GameObject lightTorch;

    // Update is called once per frame
    void Update()
    {
        toggleLight();
    }

    void toggleLight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lightTorch.activeSelf)
            {
                GetComponent<AudioSource>().PlayOneShot(lightToggleOnFX, Random.Range(0.3f, 0.5f));
            }
            else
                GetComponent<AudioSource>().PlayOneShot(lightToggleOffFX, Random.Range(0.3f, 0.5f));

            lightTorch.SetActive(!lightTorch.activeSelf);
        }
    }
}
