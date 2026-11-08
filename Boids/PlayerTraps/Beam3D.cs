using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam3D : MonoBehaviour
{
    public BeamTrap3D trap;

    public bool triggered = false;
    public float speed = 0;
    public int currentPoint = 0;

    public float dmgDelayTime = 3f;
    private float dmgTime = 0;
    private bool onDelay = false;

    private Material material;
    private Color color;
    private float distance = 0;
    private bool nextPoint = false;
    public float distanceStartPoints = 0;
    public float distanceEndPoints = 0;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        color = material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onDelay || other.tag != "Player")
            return;

        triggered = true;
        onDelay = true;
    }

    public void Reset()
    {
        currentPoint = 0;
        triggered = false;
        onDelay = false;
        dmgTime = 0;
        color.a = 1;
        this.transform.position = trap.points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, trap.points[currentPoint].position, offset);

        if (Vector3.Distance(this.transform.position, trap.points[currentPoint].position) < .025f)
        { 
            currentPoint++;
            nextPoint = true;
            distance = 0;
        }

        if (currentPoint == 1 && !nextPoint)
        {
            distance += offset;
            color.a = distance / distanceStartPoints;
            material.color = color;
        }

        if (currentPoint == trap.points.Length - 1)
        {
            distance += offset;
            color.a = 1f - distance / distanceEndPoints;
            material.color = color;
        }

        if (currentPoint == trap.points.Length)
            this.transform.gameObject.SetActive(false);

        if (onDelay)
        {
            dmgTime += Time.deltaTime;
            if (dmgTime > dmgDelayTime)
            {
                onDelay = false;
                dmgTime = 0;
            }
        }
        nextPoint = false;
    }
}
