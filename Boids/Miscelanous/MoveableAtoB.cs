using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableAtoB : MonoBehaviour
{
    public GameObject movable;

    public Vector3 positionOpened;
    public Vector3 positionClosed;

    public Trigger trigger;
    public float speed = 2.5f;

    public bool opened = false;

    private bool moving = false;
    private Vector3 targetPosition;
    private bool stateSwitched = false;

    // Update is called once per frame
    void Update()
    {
        if (trigger.triggered)
        {
            if (stateSwitched)
                return;

            moving = true;


            if (opened)
                targetPosition = positionClosed;
            else
                targetPosition = positionOpened;
        }
        else if (stateSwitched)
            stateSwitched = false;

        if (moving)
        {
            movable.transform.localPosition = Vector3.MoveTowards(movable.transform.localPosition, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(targetPosition, movable.transform.localPosition) < .05f)
            {
                if (!opened)
                    opened = true;
                else
                    opened = false;

                stateSwitched = true;
                moving = false;
            }
        }
    }
}
