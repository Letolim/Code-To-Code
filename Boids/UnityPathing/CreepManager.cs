using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;

public class CreepManager : MonoBehaviour
{
    public GameObject player;
    public ProjectileBuffer projectileBuffer;
    public GameWorld world;

    public int boidCount;

    private List<NavMeshAgent> boids;
    public List<int> indexs = new List<int>();
    public List<GameObject> boidsO;

    public int creepLimit = 150;
    public float spawnTime = 15f;
    private float currentSpawnTime = 0;

    public GameObject[] creeps;

    // Start is called before the first frame update
    void Start()
    {
        boids = new List<NavMeshAgent>();
        boidsO = new List<GameObject>();

        for (int i = 0; i < boidCount; i++)
            SpawnCreep();
    }

    private void SpawnCreep()
    {
        GameObject creep = GameObject.Instantiate(creeps[Random.Range(0, creeps.Length)]);
        creep.SetActive(true);
        boidsO.Add(creep);
        boids.Add(creep.transform.GetComponent<NavMeshAgent>());
        creep.GetComponent<BaseCreep>().Respawn(true);
        creep.GetComponent<BaseCreep>().onRespawn = false;
    }


    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;

        if (boidsO.Count != creepLimit && currentSpawnTime > spawnTime)
        {
            currentSpawnTime = 0;
            SpawnCreep();
        }

        List<float> distances = new List<float>();
        indexs.Clear();

        for (int i = 0; i < boids.Count; i++)
        {
            indexs.Add(i);
            distances.Add(Vector3.Distance(boidsO[i].transform.position, player.transform.position));
        }

        for (int i = 0; i < boids.Count; i++)
            for (int n = boids.Count - 1; n != 0; n--)
            {
                if (distances[i] > distances[n])
                {
                    float tmpDistance = distances[i];
                    int tmpIndex = indexs[i];
                    distances[i] = distances[n];
                    distances[n] = tmpDistance;
                    indexs[i] = indexs[n];
                    indexs[n] = tmpIndex;
                }
            }
    }

    //public void CalculateBoidCenter()
    //{
    //    while (!quit)
    //    {
    //        Vector3 newPosition = Vector3.zero;
    //        for (int i = 0; i < boids.Count; i++)
    //        {
    //            newPosition += boidsO[i].transform.position;
    //        }
    //        Vector3 boidCenterPosition = newPosition / (float)boids.Count;

    //    }
    //}
}
