using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorObject;
    public GameObject navBlocker;

    public Vector3 positionOpened;
    public Vector3 positionClosed;

    public Trigger[] trigger;
    public float speed = 2.5f;

    public bool opened = false;
    public bool disableTrigger = false;

    private bool moving = false;
    private Vector3 targetPosition;
    private bool stateSwitched = false;

    // Update is called once per frame
    void Update()
    {
        //doorObject.transform.SetParent(this.transform,false);

        for(int i = 0; i < trigger.Length; i++)
            if (trigger[i].triggered)
            {
                if (disableTrigger)
                    trigger[i].disableTrigger = true;

                Debug.Log("doorTrigger");

                i = trigger.Length;
                if (stateSwitched)
                    return;
                moving = true;

                if (opened)
                {
                    targetPosition = positionClosed;
                    navBlocker.SetActive(true);
                }
                else
                    targetPosition = positionOpened;
            }
            else if (stateSwitched)
                stateSwitched = false;

        if (moving)
        {
            doorObject.transform.localPosition = Vector3.MoveTowards(doorObject.transform.localPosition, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(targetPosition, doorObject.transform.localPosition) < .05f)
            {
                if (!opened)
                {
                    opened = true;
                    navBlocker.SetActive(false);
                }
                else
                    opened = false;
                stateSwitched = true;
                moving = false;
            }
        }
    }
}
