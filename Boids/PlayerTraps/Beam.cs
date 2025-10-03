using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public BeamTrap trap;

    public bool triggered = false;
    public float speed = 0;
    public float max = 0;

    public float dmgDelayTime = 3f;
    private float dmgTime = 0;
    private bool onDelay = false;

    private Material material;
    private Color color;
    public float initial;
    public int axis;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        color = material.color;

        if (axis == 0)
            initial = this.transform.localPosition.x;
        if (axis == 1)
            initial = this.transform.localPosition.y;
        if (axis == 2)
            initial = this.transform.localPosition.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onDelay || other.tag != "Player")
            return;
        trap.PlaySound(2);
        triggered = true;
        onDelay = true;
    }

    public void Reset(float startPos)
    {
        triggered = false;
        onDelay = false;
        dmgTime = 0;
        color.a = 1;

        if(axis == 0)
            this.transform.localPosition = new Vector3(startPos, this.transform.localPosition.y, this.transform.localPosition.z);
        if (axis == 1)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, startPos, this.transform.localPosition.z);
        if (axis == 2)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, startPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (axis == 0)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + speed * Time.deltaTime, this.transform.localPosition.y, this.transform.localPosition.z);
        if (axis == 1)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + speed * Time.deltaTime, this.transform.localPosition.z);
        if (axis == 2)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + speed * Time.deltaTime);

        if (axis == 0)
            color.a = Mathf.Sin(((this.transform.localPosition.x - initial) / (max - initial)) * 3.05f);
        if (axis == 1)
            color.a = Mathf.Sin(((this.transform.localPosition.y - initial) / (max - initial)) * 3.05f);
        if (axis == 2)
            color.a = Mathf.Sin(((this.transform.localPosition.z - initial) / (max - initial)) * 3.05f);

        material.color = color;

        if ((this.transform.localPosition.x >= max && axis == 0) || (this.transform.localPosition.y >= max && axis == 1) || (this.transform.localPosition.z >= max && axis == 2))
        {
            trap.PlaySound(1);
            this.transform.gameObject.SetActive(false); 
        }

        if (onDelay)
        {
            dmgTime += Time.deltaTime;
            if (dmgTime > dmgDelayTime)
            {
                onDelay = false;
                dmgTime = 0;
            }
        }
    }
}
