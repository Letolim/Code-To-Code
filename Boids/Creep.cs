using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creep : MonoBehaviour
{
    public ProjectileBuffer projectileBuffer;
    public GameWorld world;
    public GameObject player;
    public bool onRespawn = false;
    public float baseHitPoints = 200f;
    public float hitPoints = 100f;

    private float strength;
    private float respawnTime = 0;
    private float maxRespawnTime = 2f;

    private bool proximity = false;
    private float proxTime = 0;
    private float proxTimeDelta = 1;
    private float c1 = 0;
    private float c2 = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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

        if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
            Explode();

        else if (proximity)
        {
            proxTimeDelta = 1;
            proxTime = 0;
            proximity = false;
            SetColor();
            this.transform.GetComponent<NavMeshAgent>().speed = 1.5f + 7.5f * (1f - strength / 5f);
        }

        if (hitPoints < 0)
            Respawn(false);
    }

    public void Explode()
    {
        if (!proximity)
            this.transform.GetComponent<NavMeshAgent>().speed = this.transform.GetComponent<NavMeshAgent>().speed * 1.4f;

        proximity = true;

        proxTime += Time.deltaTime * proxTimeDelta;

        if (proxTimeDelta < 15f)
            proxTimeDelta += Time.deltaTime * 5f;
        else
        {
            projectileBuffer.SpawnNewProjectile(2,this.gameObject);
            Respawn(true);
        }

        this.transform.GetComponent<MeshRenderer>().material.color = new Color(Mathf.Sin(proxTime), c2, c1); //1.1111111111

    }

    private void SetColor()
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
        //SetColor();
        if(!hidden)
            projectileBuffer.SpawnNewProjectile(3, this.gameObject);

        strength = Random.Range(1, 5);
   
        this.transform.GetComponent<NavMeshAgent>().Warp(world.GetDistantSpawnPosition());
        hitPoints = baseHitPoints * (strength / 5f);
        this.transform.GetComponent<NavMeshAgent>().speed = 1.5f + 7.5f * (1f - strength / 5f);
        this.transform.localScale = new Vector3(strength,strength, strength);

        SetColor();

        onRespawn = true;
    }

}
