using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrapBeam : Projectile
{
    //player
    //creepManager

    public GameObject player;
    public float impactDistance = 1f;

    public float maxParticleLifeTime = .5f;
    public ParticleSystem pSystemExplosion;
    public ParticleSystem pSystemTrail;

    public float maxLifeTime = 6f;
    private float currentLifeTime = 0;

    private bool dimishing = false;
    private bool hitPlayer = false;




    public override void InitiateProjectile(Vector4 origin)
    {
        ParticleSystem.MainModule main = pSystemExplosion.main;

        //main.startSize3D
        main.startSizeXMultiplier = origin.w;
        main.startSizeZMultiplier = origin.w;

        //Debug.Log(emission.rateOverTime.curve.keys

        //emission.rateOverTime.curve.keys[0].value = origin.w;
        //emission.rateOverTime.curve.keys[1].value = origin.w;
        //emission.rateOverTime.curve.keys[5].value = origin.w;
        //emission.rateOverTime.curve.keys[6].value = origin.w;

        this.transform.position = origin;
        //this.transform.localScale = new Vector3(origin.w, 1f, origin.w);
        pSystemExplosion.Stop();
        this.gameObject.SetActive(true);
        currentLifeTime = 0;
        pSystemTrail.Play();
        dimishing = false;
        hitPlayer = false;
        Debug.DrawLine(this.transform.position + new Vector3(.1f,0,0), this.transform.position + Vector3.up + new Vector3(.1f, 0, 0), Color.red, 2);

    }


    void Update()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime > maxLifeTime)
        {
            if (!dimishing)
                End();

            if (!hitPlayer && Vector3.Distance(this.transform.position, player.transform.position) < impactDistance)
            {
                player.transform.GetComponent<Player>().ChangeLife(-1);
                hitPlayer = true;
            }

            if (currentLifeTime > maxLifeTime + maxParticleLifeTime)
            {
                Debug.DrawLine(this.transform.position, this.transform.position + Vector3.up * 5f, Color.magenta, 2);
                pSystemExplosion.Stop();
                this.gameObject.SetActive(false); 
            }

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
        pSystemTrail.Stop();
        Debug.DrawLine(this.transform.position + new Vector3(-.1f, 0, 0), this.transform.position + Vector3.up + new Vector3(-.1f, 0, 0), Color.black, 2);
    }

    public override void Move()
    {

    }
}
