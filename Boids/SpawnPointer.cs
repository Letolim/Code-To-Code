using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnPointer
{
    public static Vector3 GetSpawnPosition(Vector3 playerPosition,int playAreaMinX,int playAreaMaxX,int playAreaMinY,int playAreaMaxY)
    {
        Vector3 spawnPosition;

        while (true)
        {
            spawnPosition = new Vector3(Random.Range(playAreaMinX, playAreaMaxX), 0, Random.Range(playAreaMinY, playAreaMaxY));
            if (Vector3.Distance(spawnPosition, playerPosition) < 35f)
                continue;

            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hitinfo, 50f);

            if (hitinfo.collider != null && hitinfo.collider.tag == "GameField")
                break;
        }


        return spawnPosition;
    }

    public static Vector3 GetSpawnPosition(int playAreaMinX, int playAreaMaxX, int playAreaMinY, int playAreaMaxY)
    {
        Vector3 spawnPosition;

        while (true)
        {
            spawnPosition = new Vector3(Random.Range(playAreaMinX, playAreaMaxX), 0, Random.Range(playAreaMinY, playAreaMaxY));

            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hitinfo, 50f);

            if (hitinfo.collider != null && hitinfo.collider.tag == "GameField")
                break;
        }


        return spawnPosition;
    }


}
