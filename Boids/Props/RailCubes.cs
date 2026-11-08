using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCubes : MonoBehaviour
{
    public GameObject template;

    public GameObject[] objects;
    public Vector3[] points;
    public int[] currentPoint;
    public float speed = 5f;

    public int maxCount = 10;
    private int count = 1;

    private float spawnDistance = 0;
    private float curObjectSpawnDistance = 0;
    private Vector3 curObjectLastPosition;

    // Start is called before the first frame update
    void Start()
    {
        objects = new GameObject[maxCount];
        currentPoint = new int[maxCount];
        curObjectLastPosition = points[0];

        float distance = Vector3.Distance(points[0], points[points.Length - 1]);

        for (int i = 1; i < points.Length; i++)
            distance += Vector3.Distance(points[i - 1], points[i]);

        spawnDistance = distance / (float)maxCount;

        objects[0] = GameObject.Instantiate(template);
        objects[0].SetActive(true);
        objects[0].transform.position = points[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (count < maxCount)
        {
            if (curObjectSpawnDistance < spawnDistance)
            {
                curObjectSpawnDistance += Vector3.Distance(objects[count - 1].transform.position, curObjectLastPosition);
                curObjectLastPosition = objects[count - 1].transform.position;
            }
            else
            {
                curObjectLastPosition = points[0];
                objects[count] = GameObject.Instantiate(template);
                objects[count].SetActive(true);
                objects[count].transform.position = points[0];
                curObjectSpawnDistance = 0;
                count++;
            }
        }

        for (int i = 0; i < count; i++)
        {
            objects[i].transform.position = Vector3.MoveTowards(objects[i].transform.position, points[currentPoint[i]], Time.deltaTime * speed);

            if (Vector3.Distance(objects[i].transform.position, points[currentPoint[i]]) < .1f)
            {
                currentPoint[i]++;
                if (currentPoint[i] == points.Length)
                    currentPoint[i] = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (points.Length <= 2)
            return;

        Gizmos.DrawLine(points[0], points[points.Length - 1]);

        for (int i = 1; i < points.Length; i++)
            Gizmos.DrawLine(points[i], points[i - 1]);
    }
}
