using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip discInitiation;
    public AudioClip discHit;

    public AudioClip chainLightInitation;
    public AudioClip chainLightImpact;

    public AudioClip creepDeath;

    public AudioClip fireballInitiation;
    public AudioClip fireballImpact;

    public AudioClip genericExplosion;

    public AudioClip genericLever;

    private float maxDistance = 100;

    

    private void Update()
    {
    
    }

    public void PlayAudioClip(int type,float distance,float delta)
    {
        if (distance > maxDistance)
            return;

        float volume;
        
        if(distance != -1)
            volume = 1f - (distance / maxDistance);
        else
            volume = 1f;

        volume *= delta;

        if (type == 0)
        {
            audioSource.PlayOneShot(discInitiation, volume);
            return;
        }
        if (type == 1)
        {
            audioSource.PlayOneShot(discHit, volume);
            return;
        }

        if (type == 2)
        {
            audioSource.PlayOneShot(chainLightInitation, volume);
            return;
        }
        if (type == 3)
        {
            audioSource.PlayOneShot(chainLightImpact, volume);
            return;
        }

        if (type == 4)
        {
            audioSource.PlayOneShot(creepDeath, volume);
            return;
        }

        if (type == 5)
        {
            audioSource.PlayOneShot(fireballInitiation, volume);
            return;
        }
        if (type == 6)
        {
            audioSource.PlayOneShot(fireballImpact, volume);
            return;
        }

        if (type == 7)
        {
            audioSource.PlayOneShot(genericExplosion, volume);
            return;
        }

        if (type == 8)
        {
            audioSource.PlayOneShot(genericLever, volume);
            return;
        }

    }



    private void PlayClip()
    {
        
    }
  
}
