using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePointer : MonoBehaviour
{
    public GameObject player;
    public Player playerScript;
    public GameObject[] upgrades;
    public RectTransform uITransformDirection;
    public RectTransform uITransformOffset;
    public Image offsetImage;

    // Update is called once per frame
    void Update()
    {
        float distance = 500000000000000;
        int index = -1;

        for (int i = 0; i < upgrades.Length; i++)
        {
            float curDistance = Vector3.Distance(player.transform.position, upgrades[i].transform.position);

            if (curDistance < distance)
            {
                distance = curDistance;
                index = i; 
            }
        }

        float height = (player.transform.position.y - upgrades[index].transform.position.y) * (player.transform.position.y - upgrades[index].transform.position.y) + (upgrades[index].transform.position.y - player.transform.position.y);
        
        if(height < 3f)
            offsetImage.enabled = false;
        else
        {
            offsetImage.enabled = true;


            if (player.transform.position.y < upgrades[index].transform.position.y)
                uITransformOffset.rotation = Quaternion.Euler(0,0, 180);
            else
                uITransformOffset.rotation = Quaternion.Euler(0, 0, 0);
        }

        float angle =  Quaternion.LookRotation(player.transform.position - upgrades[index].transform.position, Vector3.up).eulerAngles.y - playerScript.cameraRotation;
        uITransformDirection.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
