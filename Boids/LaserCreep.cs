using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCreep : BaseCreep
{
    public GameObject beam;
    public float rotationSpeed = 30f;

    public override void GameLoop()
    {
        base.GameLoop();
        beam.transform.rotation = Quaternion.Euler(beam.transform.rotation.eulerAngles + new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    public override void Initiation()
    {
        base.Initiation();
        speedMod = 1f;
    }

    public override void Respawn(bool hidden)
    {
        base.Respawn(hidden);
        rotationSpeed = 30f + 150f * (1f - strength / 5f);
    }

    public override void SetColor()
    {
        base.SetColor();
    }
}
