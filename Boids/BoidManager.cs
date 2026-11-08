using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    //public Boid[,] chunks;
    public GameObject targetBoid;
    public int testBoidCount = 50;
    public Transform playerTransform;

    private List<GameObject> boids; 
    private List<Boid> boidBehaviors;


    bool[,] tileMap;
    // Start is called before the first frame update
    void Start()
    {
        boids = new List<GameObject>();
        boidBehaviors = new List<Boid>();
        tileMap = new bool[300, 300];

        for (int i = 0; i < testBoidCount; i++)
        {
            GameObject boidMan = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boids.Add(boidMan);
            if (i < 10)
            {
                boidBehaviors.Add(new Boid(0));
                boids[i].transform.localScale = new Vector3(1, 2f, 1);
            }
            else
            {
                boidBehaviors.Add(new Boid(1));
                boids[i].transform.localScale = new Vector3(.1f, .5f, .1f);
            }

            boidBehaviors[i].position = new Vector3(Random.Range(-60, 60), 0, Random.Range(-60, 60));

        }

        //boidBehaviors[boidBehaviors.Count - 1].teamID = 3;
        //boids[boidBehaviors.Count - 1].transform.localScale = new Vector3(2f, 2f, 2f);

        //boidBehaviors.Add(new Boid(0));

    }
    int areaSize = 50;
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < testBoidCount; i++)
        {
            //if (i < 10)
            //    boidBehaviors[i].UpdateAvoidTeamBehavior(boidBehaviors, 1,i);
            //else
            //    boidBehaviors[i].UpdateFollowTeamBehavior(boidBehaviors, 0,i);

            if (i < 10)
                boidBehaviors[i].UpdateAvoidTeamBehavior(boidBehaviors, 1, i, playerTransform);
            else
                boidBehaviors[i].UpdateFollowTeamBehavior(boidBehaviors, 0, i, playerTransform);
            
            //if (boidBehaviors[i].position.x > areaSize)
            //    boidBehaviors[i].position.x = -areaSize / 2;
            //if (boidBehaviors[i].position.x < -areaSize)
            //    boidBehaviors[i].position.x = areaSize / 2;
            //if (boidBehaviors[i].position.z > areaSize)
            //    boidBehaviors[i].position.z = -areaSize / 2;
            //if (boidBehaviors[i].position.z < -areaSize)
            //    boidBehaviors[i].position.z = areaSize / 2;


            boids[i].transform.position = boidBehaviors[i].position;

            boids[i].transform.LookAt(boidBehaviors[i].direction + boidBehaviors[i].position,Vector3.up);

        }
        //boidBehaviors[boidBehaviors.Count - 1].UpdateFollowTeamBehavior(boidBehaviors, 1, boidBehaviors.Count - 1);
        //boids[boidBehaviors.Count - 1].transform.position = boidBehaviors[boidBehaviors.Count - 1].position;


    }



}
