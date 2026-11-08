using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Unity3DBoid : MonoBehaviour
{
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 1f) > 0.98)
            agent.SetDestination(new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50)));
    }
}
