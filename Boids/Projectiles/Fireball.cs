using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    //player
    //creepManager

    public GameObject player;
    public CreepManager creepManager;
    public float impactDistance = 1f;
    public float splashRadius = 1f;

    public float maxParticleLifeTime = .5f;
    public ParticleSystem pSystemExplosion;
    public ParticleSystem pSystemTrail;

    public Vector3 normalizedDirection = Vector3.zero;
    public float speed = 60f;
    public float maxLifeTime = 6f;
    public float currentLifeTime = 0;
    public Vector3 targetPosition;

    public float dmg = 10;
    public override float Delay  { get => delay; set => delay = value; }
    public float delay = 3f;

    private bool dimishing = false;

    public override void InitiateProjectile(int number)
    {
        this.transform.position = player.transform.position + Vector3.up * .2f;

        if (number >= creepManager.indexs.Count)
        {
            float delta = (float)number / creepManager.indexs.Count;
            number = (int)Mathf.Clamp(delta * (float)creepManager.indexs.Count - Mathf.Ceil((float)creepManager.indexs.Count), 0, creepManager.indexs.Count - 1);
        }

        targetPosition = (creepManager.boidsO[creepManager.indexs[number]].transform.position - this.transform.position).normalized;
        normalizedDirection = Vector3.up; 
        this.gameObject.SetActive(true);
        currentLifeTime = 0;
        pSystemTrail.Play();
        dimishing = false;
        if (number == 0)
            PlaySFX(5, -1, .35f);

        Debug.DrawLine(creepManager.boidsO[creepManager.indexs[number]].transform.position + Vector3.up, this.transform.position + Vector3.up * 3f, Color.blue,1f);
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

        normalizedDirection = Vector3.Lerp(normalizedDirection, targetPosition ,60f * Time.deltaTime);

        CheckForImpact(creepManager.boidsO);
        Move();
    }

    public override void CheckForImpact(List<GameObject> objects)
    {
        //if(this.transform.position.y < 0)
        //{
        //    End();
        //    return;
        //}

        bool didHit = false;
        int mainTarget = -1;
        for (int i = 0; i < objects.Count; i++)
            if (Vector3.Distance(this.transform.position, objects[i].transform.position) < impactDistance)
            {
                if (objects[i].GetComponent<BaseCreep>() != null)
                {
                    objects[i].GetComponent<BaseCreep>().hitPoints -= dmg;
                    dmgTextManager.GeneratenDmgText(this.transform.position, dmg + "");
                }
                mainTarget = i;
                didHit = true;
                break;
            }

        if(didHit)
        {
            for (int i = 0; i < objects.Count; i++)
                if (i != mainTarget && Vector3.Distance(this.transform.position, objects[i].transform.position) < splashRadius)
                {
                    if (objects[i].GetComponent<Creep>() != null)
                        objects[i].GetComponent<Creep>().hitPoints -= dmg * .25f;
                    dmgTextManager.GeneratenDmgText(objects[i].transform.position, dmg + "");

                    Debug.DrawLine(this.transform.position, objects[i].transform.position, Color.green, 1f);
                }

            End();
        }
    }

    private void End()
    {
        dimishing = true;
        pSystemExplosion.Play();
        currentLifeTime = maxLifeTime + .1f;
        pSystemTrail.Stop();
        PlaySFX(6, Vector3.Distance(player.transform.position, this.transform.position), .25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;
        End();
    }

    public override void Move()
    {
        this.transform.position = this.transform.position + normalizedDirection * speed * Time.deltaTime;
    }
}
