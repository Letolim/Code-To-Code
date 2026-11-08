using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscProjectile : Projectile
{
    //player
    //creepManager
    public ProjectileBuffer projectileBuffer;
    public GameObject player;
    public CreepManager creepManager;
    public float radius = 1f; 
    private float halfRadius = 1f;

    private List<int> targets = new List<int>();
    private List<Vector3> wayPoints = new List<Vector3>();
    private Vector3 dir;
    private int currentWayPoint = 0;

    public ParticleSystem pSystemTrail;

    public float speed = 5f;
    public float dmg = 5;
    public float delay = 3f;
    public int bounceCount = 3;

    public override void InitiateProjectile(int number)
    {
        this.transform.position = player.transform.position + Vector3.up * .5f;
        currentWayPoint = 0;
        wayPoints.Clear();
        CastRay();
        ParticleSystem.MainModule main = pSystemTrail.main;
        main.startSize = radius;
        dir = (wayPoints[0] - player.transform.position).normalized;
        halfRadius = radius / 2f;
        this.gameObject.SetActive(true);
        targets.Clear();
        pSystemTrail.Play();
        if (number == 0)
            PlaySFX(0,-1f,.35f);
    }

    void Update()
    {
        if (Vector3.Distance(this.transform.position, wayPoints[currentWayPoint]) < halfRadius)
        { 
            currentWayPoint++;
            PlaySFX(0, Vector3.Distance(player.transform.position, this.transform.position), .15f);
            if (currentWayPoint == wayPoints.Count)
            { 
                this.gameObject.SetActive(false);
                return;
            }
            dir = (wayPoints[currentWayPoint] - this.transform.position).normalized;
      
        }
        CheckForImpact(creepManager.boidsO);
        Move();
    }

    public override void CheckForImpact(List<GameObject> objects)
    {
        bool sameTarget = false;
        bool gotTarget = false;

        for (int i = 0; i < objects.Count; i++)
        {
            for (int n = 0; n < targets.Count; n++)
                if (i == targets[n])
                {
                    sameTarget = true;
                    break;
                }

            if (sameTarget)
            {
                sameTarget = false;
                continue;
            }

            float curDistance = Vector3.Distance(objects[i].transform.position, this.transform.position);

            if (curDistance < halfRadius)
            {
                gotTarget = true;
                dmgTextManager.GeneratenDmgText(this.transform.position, dmg + "");
                targets.Add(i);
                objects[i].GetComponent<BaseCreep>().hitPoints -= dmg;
                projectileBuffer.SpawnNewProjectile(11, objects[i]);
            }
        }

        if(gotTarget)
            PlaySFX(1, Vector3.Distance(player.transform.position, this.transform.position), .15f);
    }

    public override void Move()
    {
        this.transform.position += dir * speed * Time.deltaTime;
    }

    private void CastRay()
    {
        int count = bounceCount;

        RaycastHit hitinfo = new RaycastHit();

        Vector3 dir = Vector3.zero;

        if (Random.value > .5f)
            dir.x = Random.Range(.25f, .75f);
        else
            dir.x = Random.Range(-.75f, -.25f);

        if (Random.value > .5f)
            dir.z = Random.Range(.25f, .75f);
        else
            dir.z = Random.Range(-.75f, -.25f);

        while (true && count != 0)
        {
            if (count == bounceCount)
                Physics.Raycast(player.transform.position, dir, out hitinfo, 100f, 1 << LayerMask.NameToLayer("Bounding"));
            else
            {
                dir = Vector3.Reflect(dir, hitinfo.normal);
                dir.y = 0;


                Physics.Raycast(hitinfo.point + dir, dir, out hitinfo, 100f, 1 << LayerMask.NameToLayer("Bounding"));

                if (hitinfo.collider == null)
                {

                    wayPoints.Clear();
                    CastRay();


                    return;
                }
            }
            wayPoints.Add(hitinfo.point);
            count--;
        }

    }
}
