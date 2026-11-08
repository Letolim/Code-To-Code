using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBeam : MonoBehaviour
{
    public GameObject player;
    public GameObject beamJoint;

    private float delay = 3f;
    private bool onDelay = false;

    private float beamLength = 10f;
    private float rotationSpeed = 5f;
    private float angle = 0;

    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initiate(float angle,float beamLength, float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
        this.beamLength = beamLength;
        this.transform.SetParent(beamJoint.transform,false);
        this.transform.localPosition = new Vector3(0, 0, beamLength / 2f);
        this.transform.localScale = new Vector3(beamLength, .2f, .2f);
        timer = new Timer();

        if(angle == -1)
            this.angle = Random.Range(0, 360);
        else
            this.angle = angle;
    }


    public void ExternalUpdate(float playerAngle, float playerDistance) 
    {
        angle += Time.deltaTime * rotationSpeed;

        beamJoint.transform.rotation = Quaternion.Euler(0, angle, 0);
        
        if (onDelay && !timer.finished)
        {
            timer.Update();
            if(timer.finished)
                onDelay = false;

            return;
        }

        if (Mathf.Abs(Mathf.DeltaAngle(angle, playerAngle)) < 5f && playerDistance < beamLength)
        {
            player.transform.GetComponent<Player>().ChangeLife(-1);
            timer.Start(delay);
            onDelay = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = player.transform.position - this.transform.position;
        //float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //if (Mathf.Abs(Mathf.DeltaAngle(angle, 0)) < 5f)
        //{
        //    Debug.DrawLine(this.transform.position, this.transform.position + dir * 10f);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (onDelay || other.tag != "Player")
        //    return;
        //trap.PlaySound(2);
        //triggered = true;             17.03       17Uhr
        //onDelay = true;
    }
}
