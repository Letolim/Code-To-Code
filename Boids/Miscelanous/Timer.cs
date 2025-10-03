using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float curentTime = 0;
    public float maxTime = 1f;

    public bool finished = false;
    public bool running = false;

 

    public void Start()
    {
        curentTime = 0;
        finished = false;
        running = true;
    }

    public void Start(float time)
    {
        curentTime = 0;
        maxTime = time;
        finished = false;
        running = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if(!running) 
            return;

        curentTime += Time.deltaTime;

        if (curentTime > maxTime)
        { 
            finished = true;
            running = false;
        }
    }

    public void Delete()
    {
        curentTime = 0;
        finished = false;
        running = false;
    }

    public float Delta()
    {
        return curentTime / maxTime;
    }
}
