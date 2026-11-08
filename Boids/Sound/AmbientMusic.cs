using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip introClip;
    public AudioClip mainClip;



    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = introClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = mainClip;
            audioSource.Play();
        }
    }
}
