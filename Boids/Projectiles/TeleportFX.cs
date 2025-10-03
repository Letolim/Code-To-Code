using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFX : Projectile
{

    public float maxParticleLifeTime = .5f;
    private float currentLifeTime = 0;
    public ParticleSystem pSystemExplosion;

    public override void InitiateProjectile(Vector3 origin)
    {
        Debug.DrawLine(origin, origin + Vector3.up, Color.red, 30f);
        currentLifeTime = 0;
        this.transform.position = origin;
        this.gameObject.SetActive(true);
        pSystemExplosion.Play();
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxParticleLifeTime)
            this.gameObject.SetActive(false);
    }

    public override void CheckForImpact(List<GameObject> objects)
    {

    }

    private void End()
    {

    }

    public override void Move()
    {

    }
}
