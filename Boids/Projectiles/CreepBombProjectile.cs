using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBombProjectile : Projectile
{
    public ProjectileBuffer projectileBuffer;
    public GameObject player;
    public float impactDistance = 1f;
    public float splashRadius = 1f;

    public float maxParticleLifeTime = .5f;
    public ParticleSystem pSystemExplosion;
    public ParticleSystem pSystemTrail;

    public Vector3 normalizedDirection = Vector3.zero;
    public float speed = 30f;
    public float maxLifeTime = 6f;
    public float currentLifeTime = 0;

    private Vector3[] route;
    private int currentPoint = 0;

    public float dmg = 10;
    public override float Delay { get => delay; set => delay = value; }
    public float delay = 3f;

    private bool dimishing = false;
    private float targetDistance = 10f;
    private float floatHeight = 8f;


    public override void InitiateProjectile(Vector3 origin)
    {
        this.transform.position = origin + Vector3.up * .2f;

        route = new Vector3[2];
        route[1] = GetTargetPosition();
        route[0] = (route[1] + origin) / 2f + Vector3.up * floatHeight;
        currentPoint = 0;
        normalizedDirection = Vector3.up;

        this.gameObject.SetActive(true);
        currentLifeTime = 0;
        pSystemTrail.Play();
        dimishing = false;
    }


    private Vector3 GetTargetPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        int count = 50;
        while (true && count != 0)
        {
            count--;
            spawnPosition = player.transform.position + new Vector3(Random.Range(-targetDistance, targetDistance), 0, Random.Range(-targetDistance, targetDistance));

            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hitinfo, 50f,1 << LayerMask.NameToLayer("World"));


            if (hitinfo.collider != null )
            {
                spawnPosition.y = hitinfo.point.y + .5f;
                break;
            }
        }

        return spawnPosition;
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxLifeTime)
        {
            if (!dimishing)
                End();

            if (currentLifeTime > maxLifeTime + maxParticleLifeTime)
                this.gameObject.SetActive(false);

            return;
        }

        normalizedDirection = Vector3.Lerp(normalizedDirection, (route[currentPoint] - this.transform.position).normalized, 12f * Time.deltaTime);

        if (Vector3.Distance(this.transform.position, route[currentPoint]) < 1f)
        {
            if (currentPoint == 0)
                currentPoint++;
            else
            {
                projectileBuffer.SpawnNewProjectile(8, route[currentPoint]);
                this.gameObject.SetActive(false);
            }
        }
        Move();
    }

    private void End()
    {
        dimishing = true;
        pSystemExplosion.Play();
        currentLifeTime = maxLifeTime + .1f;
        pSystemTrail.Stop();

    }

    public override void Move()
    {
        this.transform.position = this.transform.position + normalizedDirection * speed * Time.deltaTime;
    }
}
