using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleTrigger : MonoBehaviour
{
    public GameObject player;
    public bool triggerd = true;

    public GameObject[] toActivate;

    // Update is called once per frame
    void Update()
    {
        if (triggerd)
            return;

        if (Vector3.Distance(this.transform.position, player.transform.position) < 1f)
        { 
            triggerd = true;
            for (int i = 0; i < toActivate.Length; i++)
                toActivate[i].SetActive(true);
        }
    }
}
