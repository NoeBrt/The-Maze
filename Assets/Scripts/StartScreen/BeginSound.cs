using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSound : MonoBehaviour
{
    [SerializeField] AudioClip beginSound;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(beginSound);
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
