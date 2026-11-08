using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseCreep : MonoBehaviour
{
    public ProjectileBuffer projectileBuffer;
    public GameWorld world;
    public GameObject player;
    public NavMeshAgent navAgent;

    public bool onRespawn = false;
    public bool scaleOnRespawn = false;
    public bool changeSpeedOnRespawn = false;
    public float hitPoints = 100;
    public float baseHitPoints = 200f;

    public float strength;
    private float respawnTime = 0;
    private float maxRespawnTime = 2f;

    public float c1 = 0;
    public float c2 = 0;

    public float updatePathTime = 1f;
    private float currentUpdatePathTime = 0f;
    public float speedMod = 1f;

    void Start()
    {
        Initiation();
    }



    void Update()
    {
        GameLoop();

    }

    public virtual void Initiation()
    {
        if (onRespawn)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime > maxRespawnTime)
            {
                onRespawn = false;
                respawnTime = 0;
            }
        }

        if (hitPoints < 0)
            Respawn(false);
    }

    public virtual void GameLoop()
    {
        currentUpdatePathTime += Time.deltaTime;

        if (currentUpdatePathTime > updatePathTime)
        {
            currentUpdatePathTime = 0;
            UpdatePath();
        }

        if (navAgent.isOnOffMeshLink)
        {
            navAgent.currentOffMeshLinkData.offMeshLink.gameObject.GetComponent<Teleporter>().SpawnFX();
            navAgent.CompleteOffMeshLink();
        }

        if (onRespawn)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime > maxRespawnTime)
            {
                onRespawn = false;
                respawnTime = 0;
            }
        }

        if (hitPoints < 0)
            Respawn(false);
    }

    private void UpdatePath()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (distance > 20)
            distance = 20;

        float delta = 15 * (distance / 20);

        if (delta < 2)
        {
            delta = 0;
        }

        Vector3 offset = new Vector3(Random.Range(-delta, delta), 0, Random.Range(-delta, delta));

        if (!navAgent.SetDestination(player.transform.position + offset))
        {
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.up * 50f, Color.green, 30f);
        }
    }

    public virtual void SetColor()
    {
        c1 = strength / 5f;
        c2 = 1f - strength;

        if (c1 > c2)
        {
            c2 = .55f;
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(.55f, .55f, c1);
        }
        else
        {
            c1 = .55f;
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(.55f, c2, .55f);
        }
    }

    public virtual void Respawn(bool hidden)
    {
        if (!hidden)
            projectileBuffer.SpawnNewProjectile(3, this.gameObject);

        strength = Random.Range(1, 5);
        hitPoints = baseHitPoints * (strength / 5f) + world.sceneTimer.Delta() * baseHitPoints * 1.5f;

        this.transform.GetComponent<NavMeshAgent>().Warp(world.GetDistantSpawnPosition());
        
        if(changeSpeedOnRespawn)
            this.transform.GetComponent<NavMeshAgent>().speed = 1.5f + 7.5f * (1f - strength / 5f) * speedMod;
        if(scaleOnRespawn)
            this.transform.localScale = new Vector3(strength, strength, strength);

        SetColor();

        onRespawn = true;
    }

}
