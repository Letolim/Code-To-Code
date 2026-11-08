using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepExplosion : Projectile
{
    public GameObject player;
    public float splashRadius = 1f;

    public float maxParticleLifeTime = .5f;
    public ParticleSystem pSystemExplosion;

    public float maxLifeTime = 6f;
    public float currentLifeTime = 0;

    public float dmg = 10;
    public override float Delay  { get => delay; set => delay = value; }
    public float delay = 3f;

    private bool dimishing = false;

    public override void InitiateProjectile(GameObject origin)
    {
        this.transform.position = origin.transform.position;
        this.transform.rotation = origin.transform.rotation;
        //this.transform.localScale = origin.transform.localScale;

        if (Vector3.Distance(origin.transform.position, player.transform.position) < splashRadius)
            player.transform.GetComponent<Player>().ChangeLife(-1);

        ParticleSystem.ShapeModule mainShape = pSystemExplosion.shape;
        mainShape.scale = origin.transform.localScale;
        this.gameObject.SetActive(true);
        currentLifeTime = 10f;
        dimishing = false;
        PlaySFX(7, Vector3.Distance(player.transform.position, this.transform.position), .6f);
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxLifeTime)
        {

            if(!dimishing)
                End();
            if (currentLifeTime > maxLifeTime + maxParticleLifeTime)
                this.gameObject.SetActive(false);
            return;
        }
    }

    public override void CheckForImpact(List<GameObject> objects)
    {
      
    }

    private void End()
    {
        PlaySFX(7, Vector3.Distance(player.transform.position, this.transform.position), .25f);
        dimishing = true;
        pSystemExplosion.Play();
        currentLifeTime = maxLifeTime + .1f;
    }

    public override void Move()
    {

    }
}
