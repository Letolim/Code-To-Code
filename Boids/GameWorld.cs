using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class GameWorld : MonoBehaviour
{
    public Volume mainSkyAndFogVolume;
    public Light sunLight;

    public GameObject player;
    public ProjectileBuffer projectileBuffer;
    public int playAreaMinX;
    public int playAreaMaxX; 
    public int playAreaMinZ;
    public int playAreaMaxZ;

    private Fog fogProfile;

    public float levelTime = 30f;
    public Timer sceneTimer = new Timer();

    public float finalFogAttenuation = 20f;
    private float startFogAttenuation = 4f;
    public Color finalFogColor;
    private Color startFogColor;
    public float finalLightIntensity = 4f;
    private float startLightIntensity = 4f;

    // Start is called before the first frame update
    void Start()
    {
        mainSkyAndFogVolume.profile.TryGet<Fog>(out fogProfile);
        startFogAttenuation = fogProfile.meanFreePath.value;
        startFogColor = fogProfile.albedo.value;

        sceneTimer.Start(levelTime);

        startLightIntensity = sunLight.intensity;
     }

    // Update is called once per frame
    void Update()
    {
        ApplySceneSettings();
    }


    private void ApplySceneSettings()
    {
        if (sceneTimer.finished)
            return;
        sceneTimer.Update();
        fogProfile.meanFreePath.value = Mathf.Lerp(startFogAttenuation, finalFogAttenuation, sceneTimer.Delta());
        fogProfile.albedo.value = Color.Lerp(startFogColor, finalFogColor, sceneTimer.Delta());

        sunLight.intensity = Mathf.Lerp(startLightIntensity, finalLightIntensity, sceneTimer.Delta());


    }

    public Vector3 GetDistantSpawnPosition()
    {
        Vector3 spawnPosition;

        while (true)
        {
            spawnPosition = new Vector3(Random.Range(playAreaMinX, playAreaMaxX), 0, Random.Range(playAreaMinZ, playAreaMaxZ));
            if (Vector3.Distance(spawnPosition, player.transform.position) < 35f)
                continue;

            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(spawnPosition + Vector3.up * 100f, Vector3.down, out hitinfo, 150f);

            if (hitinfo.collider != null && hitinfo.collider.tag == "GameField")
            {
                Debug.DrawLine(spawnPosition, spawnPosition + Vector3.up * 10f, Color.red, 30f);

                spawnPosition.y = hitinfo.point.y + .5f;
                break; 
            }
        }

        return spawnPosition;
    }

    public Vector3 GetCloseSpawnPosition()
    {
        Vector3 spawnPosition;

        while (true)
        {
            spawnPosition = new Vector3(Random.Range(playAreaMinX, playAreaMaxX), 0, Random.Range(playAreaMinZ, playAreaMaxZ));

            float distance = Vector3.Distance(spawnPosition, player.transform.position);
            if (distance < 35f || distance > 55f)
                continue;

            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hitinfo, 50f);

            if (hitinfo.collider != null && hitinfo.collider.tag == "GameField")
            {
                spawnPosition.y = hitinfo.point.y + .5f;
                break;
            }
        }

        return spawnPosition;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;

        while (true)
        {
            spawnPosition = new Vector3(Random.Range(playAreaMinX, playAreaMaxX), 0, Random.Range(playAreaMinZ, playAreaMaxZ));

            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hitinfo, 50f);

            if (hitinfo.collider != null && hitinfo.collider.tag == "GameField")
            {
                spawnPosition.y = hitinfo.point.y + .5f;
                break; 
            }
        }

        return spawnPosition;
    }
}
