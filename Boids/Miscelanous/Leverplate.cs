using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leverplate : Trigger
{
    public ProjectileSoundManager soundManager;

    public GameObject player;
    public GameObject plate;

    public Vector3 positionInitial;
    public Vector3 positionFinished;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (selfReset)
        {
            if (triggered)
                plate.transform.localPosition = Vector3.Lerp(positionFinished, positionInitial, timer.Delta());
            
            if (timer.finished)
            {
                soundManager.PlayAudioClip(8, Vector3.Distance(this.transform.position, player.transform.position), .75f);
                timer.Delete();
            }
        }

        if (!triggered)
        {
            float distance = Vector3.Distance(this.transform.position, player.transform.position);

            if (distance < 1f)
            {
                soundManager.PlayAudioClip(8, distance, .75f);
                plate.transform.localPosition = positionFinished;
                triggered = true;
            }
        }

        base.Update();
    }
}
