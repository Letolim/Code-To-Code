using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCreep : BaseCreep
{
    public float delay = 5f;

    private float currentDelay = 0;

    public override void GameLoop()
    {
        base.GameLoop();

        if(Vector3.Distance(this.transform.position, player.transform.position) < 20f)
            currentDelay += Time.deltaTime;

        if (currentDelay > delay)
        {
            projectileBuffer.SpawnNewProjectile(7, this.transform.position);
            currentDelay = 0;
        }
    }

    public override void Initiation()
    {
        base.Initiation();
    }

    public override void Respawn(bool hidden)
    {
        base.Respawn(hidden);
        delay = 1f + 4f * (1f - strength / 5f);
    }

    public override void SetColor()
    {
        base.SetColor();
    }
}
