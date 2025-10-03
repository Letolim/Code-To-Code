using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour
{

    public ProjectileBuffer projectileBuffer;
    public GameObject player;
    public float radius = 2f;
    public Transform transformA;
    public Transform transformB;
    public bool biderectional = false; 


    private float delayTimer = 0;
    private float delay = 3f;
    private bool onDelay = false;

    public Teleporter twoWayLink;

    // Start is called before the first frame update
    void Start()
    {
    
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onDelay)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer > delay)
            {
                onDelay = false;
                delayTimer = 0;
            }

            return;
        }

        if (Vector3.Distance(player.transform.position, transformA.position) < radius)
        {
            onDelay = true;
            player.transform.position = transformB.position;
            player.GetComponent<NavMeshAgent>().Warp(transformB.position);
            SpawnFX();
            if (twoWayLink != null)
                twoWayLink.onDelay = true;
        }
        else if (biderectional && Vector3.Distance(player.transform.position, transformB.position) < radius)
            {
                onDelay = true;
                player.transform.position = transformA.position;
                player.GetComponent<NavMeshAgent>().Warp(transformA.position);
                SpawnFX();
            }
    }

    public void SpawnFX()
    {
        projectileBuffer.SpawnNewProjectile(9, transformA.position);
        projectileBuffer.SpawnNewProjectile(9, transformB.position);

    }


}
