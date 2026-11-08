using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool doesRespawn = false;

    public UpgradeManager upgradeManager;
    public GameObject player;
    public GameWorld world;

    public GameObject hull;
    private float colorFadeTime = 3f;
    private Color nextColor;
    private Color currentColor;
    private Timer colorTimer;

    // Start is called before the first frame update
    void Start()
    {
        colorTimer = new Timer();
        nextColor = Random.ColorHSV();
        nextColor.a = .25f;

        UpdateColor();
        Respawn(false);
    }

    private void UpdateColor()
    {
        currentColor = nextColor;

        nextColor = Random.ColorHSV();
        nextColor.a = .25f;

        colorTimer.Start(colorFadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!colorTimer.finished)
        {
            hull.transform.GetComponent<MeshRenderer>().material.SetColor("_TintColor", Color.Lerp(currentColor, nextColor, colorTimer.Delta()));
            colorTimer.Update();
        }
        else
            UpdateColor();

        this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + Time.deltaTime + 2.5f, 0);

        if (Vector3.Distance(player.transform.position, this.transform.position) < 5f)
        {
            upgradeManager.ApplyUpdate();

            if (doesRespawn)
                Respawn(true);
            else
                this.gameObject.SetActive(false);
        }
    }

    private void Respawn(bool changePosition)
    {
        if(changePosition)
            this.transform.position = world.GetRandomSpawnPosition() + Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position,1f);
    }
}
