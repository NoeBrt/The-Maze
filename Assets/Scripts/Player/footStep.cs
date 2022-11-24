using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footStep : MonoBehaviour
{
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = transform.parent.GetComponent<CharacterController>();
        GetComponent<AudioSource>().spatialize = true;

    }

    // Update is called once per frame
    void Update()
    {
     /*   if (cc.isGrounded && new Vector2(cc.velocity.x, cc.velocity.z).magnitude > 2f && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().volume = Random.Range(0.5f, 1f);
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
            //GetComponent<AudioSource>().Play();
        }
        else
        {
            //   GetComponent<AudioSource>().Stop();
        }
*/
    }
}
