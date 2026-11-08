using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTrap3D : MonoBehaviour
{
    public GameObject player;
    public Transform[] points;
    public float speed = 0;
    public float delay = 0;
    public bool loop;

    public GameObject beamTemplate;

    private List<GameObject> beams = new List<GameObject>();
    private List<Beam3D> beamScripts = new List<Beam3D>();

    private float spawnTimer = 0;
    private float distanceStartPoints = 0;
    private float distanceEndPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        beamTemplate.transform.SetParent(this.transform, true);
        spawnTimer = Random.Range(0, delay);

        distanceStartPoints = Vector3.Distance(points[0].position,points[1].position);
        distanceEndPoints = Vector3.Distance(points[points.Length - 2].position, points[points.Length -1].position);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > delay)
        {
            spawnTimer = 0;
            SpawnBeam();
        }

        for (int i = 0; i < beams.Count; i++)
            if (beamScripts[i].isActiveAndEnabled && beamScripts[i].triggered)
            {
                    beamScripts[i].triggered = false;
                    player.transform.GetComponent<Player>().ChangeLife(-1);
            }
    }

    public void SpawnBeam()
    {
        for (int i = 0; i < beams.Count; i++)
            if (!beams[i].activeInHierarchy)
            {
                InitiateBeam(i);
                return;
            }
        InitiateBeam(-1);
    }

    public void InitiateBeam(int index)
    {
        if (index == -1)
        {
            GameObject beam = GameObject.Instantiate(beamTemplate);
            beam.transform.position = points[0].transform.position;
            beam.transform.rotation = this.transform.rotation;
            beam.transform.GetComponent<Beam3D>().trap = this;
            beam.transform.GetComponent<Beam3D>().speed = speed;
            beam.transform.GetComponent<Beam3D>().dmgDelayTime = delay;
            beam.transform.GetComponent<Beam3D>().distanceStartPoints = distanceStartPoints;
            beam.transform.GetComponent<Beam3D>().distanceEndPoints = distanceEndPoints;

            beamScripts.Add(beam.transform.GetComponent<Beam3D>());
            beams.Add(beam);
            return;
        }

        beams[index].SetActive(true);
        beamScripts[index].Reset();
    }
}
