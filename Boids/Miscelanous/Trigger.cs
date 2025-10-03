using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    public bool triggered = false;
    private bool triggerFired = false;
    public bool selfReset = false;
    public bool disableTrigger = false;
    public float resetTime = 3f;
    public Timer timer;
    public GameObject[] switchStateOnTrigger;



    // Start is called before the first frame update
    public virtual void Start()
    {
        if (selfReset)
            timer = new Timer();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (triggered && !triggerFired)
        {
            triggerFired = true;

            for (int i = 0; i < switchStateOnTrigger.Length; i++)
                if (switchStateOnTrigger[i].active)
                    switchStateOnTrigger[i].SetActive(false);
                else
                    switchStateOnTrigger[i].SetActive(true);
        }

        if (selfReset && triggered)
        {
            if (!timer.running)
                timer.Start(resetTime);

            timer.Update();

            if (timer.finished)
                triggered = false;
        }

        if (disableTrigger)
        {
            triggered = false;
            enabled = false;
        }
    }

    public float ResetDelta()
    {
        return timer.Delta();
    }

}
