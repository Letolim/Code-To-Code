using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject navBlocker;
    public Vector3 finalPosition;
    public Trigger trigger;
    public float speed = 5f;

    public bool moving = false;

    // Update is called once per frame
    void Update()
    {
        if (trigger.triggered)
            moving = true;

        if (moving)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, finalPosition, speed * Time.deltaTime);

            if (Vector3.Distance(finalPosition, this.transform.position) < .05f)
            { 
                this.enabled = false;
                navBlocker.SetActive(false);
            }
        }

    }
}
