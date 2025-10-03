using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingProp : MonoBehaviour
{
    float time = 0;

    public GameObject[] props;

    private Vector3[] initialPositions;
    private float[] times;
    private float[] offsets; 
    private float[] speeds;


    void Start()
    {
        times = new float[props.Length];
        offsets = new float[props.Length];
        speeds = new float[props.Length];

        initialPositions = new Vector3[props.Length];

        for (int i = 0; i < props.Length; i++)
        { 
            times[i] = Random.Range(0, 5000);
            offsets[i] = Random.Range(10, 30);
            speeds[i] = Random.Range(.1f, .5f);
            initialPositions[i] = props[i].transform.position;
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        for (int i = 0; i < props.Length; i++)
            props[i].transform.position = initialPositions[i] + new Vector3(0, offsets[i] * Mathf.Sin(times[i] + time), 0) * speeds[i];
    }
}
