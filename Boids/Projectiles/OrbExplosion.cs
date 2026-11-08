using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbExplosion : Projectile
{
    //player
    //creepManager

    public float splashRadius = 1f;

    public float maxParticleLifeTime = .5f;
    public ParticleSystem pSystemExplosion;

    public float maxLifeTime = 6f;
    public float currentLifeTime = 0;

    public float dmg = 5;
    public override float Delay { get => delay; set => delay = value; }
    public float delay = 3f;

    private bool dimishing = false;

    private GameObject originObject;
    private BaseCreep creepScript;

    public override void InitiateProjectile(GameObject origin)
    {
        originObject = origin;
        creepScript = origin.transform.GetComponent<BaseCreep>();
        creepScript.hitPoints -= dmg;
        dmgTextManager.GeneratenDmgText(this.transform.position, dmg + "");
        this.transform.position = origin.transform.position;
        this.transform.rotation = origin.transform.rotation;
        ParticleSystem.ShapeModule mainShape = pSystemExplosion.shape;
        mainShape.mesh = origin.transform.GetComponent<MeshFilter>().mesh;
        mainShape.scale = origin.transform.localScale;
        this.gameObject.SetActive(true);
        currentLifeTime = 10f;
        dimishing = false;
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        Move();


        if (currentLifeTime > maxLifeTime)
        {

            if (!dimishing)
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
        dimishing = true;
        pSystemExplosion.Play();
        currentLifeTime = maxLifeTime + .1f;
    }

    public override void Move()
    {
        if (creepScript.onRespawn)
            return;

        this.transform.position = originObject.transform.position;
        this.transform.rotation = originObject.transform.rotation;

    }
}
