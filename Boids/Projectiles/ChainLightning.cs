using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Projectile
{
    //player
    //creepManager
    public GameObject player;
    public CreepManager creepManager;
    public float maxDistance = 10f;
    public float impactDistance = 1f;
    public int maxTargets = 3;
    private int targetCount = 0;
    private int currentTarget = -1;
    private List<int> targets = new List<int>();

    public float maxParticleLifeTime = .5f;
    public ParticleSystem pSystemExplosion;
    public ParticleSystem pSystemTrail;

    public float speed = 350f;
    public float maxLifeTime = 6f;
    public float currentLifeTime = 0;
 
    public float dmg = 5;
    private float curDmg = 5;

    public override float Delay { get => delay; set => delay = value; }
    public float delay = 1.5f;

    public override void InitiateProjectile(int number)
    {
        this.transform.position = player.transform.position + Vector3.up * .2f;
        this.gameObject.SetActive(true);

        curDmg = dmg;
        currentLifeTime = 0;
        targetCount = 0;
        currentTarget = -1;
        targets.Clear();
        pSystemTrail.Play();

        if (number >= creepManager.indexs.Count)
        {
            float delta = (float)number / creepManager.indexs.Count;
            number = (int)Mathf.Clamp(delta * (float)creepManager.indexs.Count - Mathf.Ceil((float)creepManager.indexs.Count), 0, creepManager.indexs.Count - 1);
        }

        currentTarget = creepManager.indexs[number];
        targets.Add(creepManager.indexs[number]);
        if(Vector3.Distance(player.transform.position, creepManager.boidsO[currentTarget].transform.position) > maxDistance)
            this.gameObject.SetActive(false);
        if(number == 0)
            PlaySFX(2, -1, .75f);
    }

    private void UpdateTarget()
    {

        float distance = 500000;
        int index = -1;
        bool sameTarget = false;
        for (int i = 0; i < creepManager.boidsO.Count; i++)
        {
            for (int n = 0; n < targets.Count; n++)
                if (i == targets[n])
                {
                    sameTarget = true;
                    break;
                }
           
            if(sameTarget)
            {
                sameTarget = false;
                continue;
            }

            float curDistance = Vector3.Distance(creepManager.boidsO[i].transform.position, this.transform.position);

            if (curDistance < distance)
            {
                distance = curDistance;
                index = i;
            }
        }
        if (distance > maxDistance)
        { 
            currentLifeTime = maxLifeTime + .1f;
            return;
        }
        Debug.DrawLine(creepManager.boidsO[index].transform.position, this.transform.position, Color.black, 1f);
        Debug.DrawLine(creepManager.boidsO[index].transform.position, creepManager.boidsO[index].transform.position + Vector3.up, Color.black, 1f);

        currentTarget = index;
        targets.Add(index);
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxLifeTime)
        {
            if (currentLifeTime > maxLifeTime + maxParticleLifeTime)
                this.gameObject.SetActive(false);
            return;
        }

        CheckForImpact(creepManager.boidsO);
        Move();
    }

    public override void CheckForImpact(List<GameObject> objects)
    {
        if (creepManager.boidsO[currentTarget].transform.GetComponent<BaseCreep>().onRespawn)
            UpdateTarget();

        if (Vector3.Distance(this.transform.position, creepManager.boidsO[currentTarget].transform.position) < 1f)
        {
            PlaySFX(3, Vector3.Distance(player.transform.position, this.transform.position), .25f);
            creepManager.boidsO[currentTarget].transform.GetComponent<BaseCreep>().hitPoints -= curDmg;
            dmgTextManager.GeneratenDmgText(this.transform.position, curDmg + "");
            curDmg *= .9f;
            pSystemExplosion.Play();
            UpdateTarget();
            targetCount++;
        }
        if (targetCount == maxTargets + 1)
            currentLifeTime = maxLifeTime + .1f;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        pSystemExplosion.Play();
        currentLifeTime = maxLifeTime + .1f;
        pSystemTrail.Stop();
        return;
    }

    public override void Move()
    {
        this.transform.position += (creepManager.boidsO[currentTarget].transform.position - this.transform.position).normalized * speed * Time.deltaTime;
    }
}
