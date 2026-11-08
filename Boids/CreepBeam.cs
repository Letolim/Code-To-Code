using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBeam : MonoBehaviour
{
    public Player player;
    public float dmgDelayTime = 3f;
    private float dmgTime = 0;
    private bool onDelay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        player.ChangeLife(-1);
        onDelay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onDelay)
        {
            dmgTime += Time.deltaTime;
            if (dmgTime > dmgDelayTime)
            {
                onDelay = false;
                dmgTime = 0;
            }
        }
    }
}
