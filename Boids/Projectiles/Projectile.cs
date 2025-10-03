using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public FloatingDmgTextManager dmgTextManager;
    public ProjectileSoundManager soundManager;
    public virtual float Delay { get; set; }

    public virtual void InitiateProjectile(int number)
    {

    }

    public virtual void InitiateProjectile(GameObject origin)
    {

    }

    public virtual void InitiateProjectile(Vector3 origin)
    {

    }

    public virtual void InitiateProjectile(Vector4 origin)
    {

    }

    public virtual void PlaySFX(int type,float distance,float delta)
    {
        soundManager.PlayAudioClip(type, distance, delta);
    }

    public virtual void Move()
    {
        
    }

    public virtual void CheckForImpact(List<GameObject> objects)
    {
        
    }

    void Update()
    {
    }

        //public float baseMoveSpeed { get; set; }
        //public Vector3 position { get; set; }
        //// public float BaseMoveSpeed { get; set; }

        //public void DealSplashDamage(float radius,List<int> targets) 
        //{
        //    Debug.DrawLine(Vector3.zero, Vector3.zero);
        //}


        //public void Move()
        //{
        //    Debug.DrawLine(Vector3.zero, Vector3.zero);
        //}

    }
