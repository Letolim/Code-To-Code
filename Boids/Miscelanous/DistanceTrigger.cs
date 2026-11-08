using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTrigger : Trigger
{
    public GameObject player;
    public float triggerDistance = 1f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if ((triggered && !selfReset) || disableTrigger)
            return;


        if (Vector3.Distance(this.transform.position, player.transform.position) < triggerDistance)
            triggered = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, .25f);
        Gizmos.DrawSphere(this.transform.position,triggerDistance);
    }
}
