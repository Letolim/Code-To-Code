using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCreep : BaseCreep
{

    private bool proximity = false;
    private float proxTime = 0;
    private float proxTimeDelta = 1;

    public override void GameLoop()
    {
        base.GameLoop();

        if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
            Explode();

        else if (proximity)
        {
            proxTimeDelta = 1;
            proxTime = 0;
            proximity = false;
            SetColor();
            navAgent.speed = 1.5f + 7.5f * (1f - strength / 5f);
        }
    }

    public void Explode()
    {
        if (!proximity)
            navAgent.speed = navAgent.speed * 1.4f;

        proximity = true;

        proxTime += Time.deltaTime * proxTimeDelta;

        if (proxTimeDelta < 15f)
            proxTimeDelta += Time.deltaTime * 5f;
        else
        {
            projectileBuffer.SpawnNewProjectile(2, this.gameObject);
            Respawn(true);
        }

        this.transform.GetComponent<MeshRenderer>().material.color = new Color(Mathf.Sin(proxTime), c2, c1); //1.1111111111
    }

    public override void Initiation()
    {
        base.Initiation();
        speedMod = 1.5f;
    }

    public override void Respawn(bool hidden)
    {
        base.Respawn(hidden);
    }

    public override void SetColor()
    {
        base.SetColor();
    }

}
