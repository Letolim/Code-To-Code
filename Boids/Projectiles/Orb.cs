using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Projectile
{
    //player
    //creepManager
    public ProjectileBuffer projectileBuffer;
    public GameObject player;
    public CreepManager creepManager;
    public float maxDistance = 5f;
    private float currentDistance = 0;
    private List<int> targets = new List<int>();

    public ParticleSystem pSystemTrail;
    private ParticleSystem.ShapeModule pSystemTrailShape;

    public float speed = 5f;
    private float currentLifeTime = 0;
    public float dmg = 2.5f;

    private bool targetsCleard = false;

    public override void InitiateProjectile(int number)
    {
        this.transform.position = player.transform.position + Vector3.up * .5f;
        this.gameObject.SetActive(true);
        pSystemTrailShape = pSystemTrail.shape;
        currentLifeTime = 0;
        targets.Clear();
        pSystemTrail.Play();
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime * speed;

        if (currentLifeTime >= 9999999999999999999f)
            currentLifeTime = 0;

        currentDistance = maxDistance * (Mathf.Sin(currentLifeTime) + 1f) / 2f;

        if (currentDistance < maxDistance * .1f )
        {
            if (!targetsCleard)
            {
                targets.Clear();
                targetsCleard = true;
            }
        }
        else targetsCleard = false;

        currentDistance += 1f;
        pSystemTrailShape.radius = currentDistance;
        CheckForImpact(creepManager.boidsO);
        Move();
    }

    public override void CheckForImpact(List<GameObject> objects)
    {
        bool sameTarget = false;
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

            if (curDistance < currentDistance)
            {
                targets.Add(i);
                projectileBuffer.SpawnNewProjectile(5, objects[i]);
            }
        }
    }

  

    public override void Move()
    {
        this.transform.position = player.transform.position + Vector3.up * .5f;
    }
}