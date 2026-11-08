using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTrap : MonoBehaviour
{
    public GameObject player;
    public Trigger trigger;

    public float min = 0;
    public float max = 0;
    public float speed = 0;
    public float delay = 0;

    public GameObject beamTemplate;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    public int axis = 0;
    private List<GameObject> beams = new List<GameObject>();
    private List<Beam> beamScripts = new List<Beam>();

    private float spawnTimer = 0;


    public AudioSource audioSourceFadeFX;
    public AudioSource audioSourceContiniousFX;
    public AudioClip fadeInClip;
    public AudioClip fadeOutClip;


    // Start is called before the first frame update
    void Start()
    {
        beamTemplate.transform.SetParent(this.transform, true);
        spawnTimer = Random.Range(0, delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger != null && !trigger.triggered)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer > delay)
        {
            spawnTimer = 0;
            SpawnBeam();
        }

        bool beamIsActive = false;

        for (int i = 0; i < beams.Count; i++)
            if (beamScripts[i].isActiveAndEnabled)
            {
                beamIsActive = true;
                if (beamScripts[i].triggered)
                { 
                    beamScripts[i].triggered = false;
                    player.transform.GetComponent<Player>().ChangeLife(-1); 
                }
            }

        if (audioSourceContiniousFX.isPlaying && !beamIsActive)
            audioSourceContiniousFX.Stop();

    }


    public void PlaySound(int type)
    {
        if(type == 0)
            audioSourceFadeFX.PlayOneShot(fadeInClip);
        if (type == 1)
            audioSourceFadeFX.PlayOneShot(fadeOutClip);

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
            beam.transform.SetParent(this.transform,false);

            if (axis == 0)
                beam.transform.localPosition = new Vector3(min, initialPosition.y, initialPosition.z);
            if (axis == 1)
                beam.transform.localPosition = new Vector3(initialPosition.x, min, initialPosition.z);
            if (axis == 2)
                beam.transform.localPosition = new Vector3(initialPosition.x, initialPosition.y, min);

            beam.transform.rotation = initialRotation;

            beam.transform.GetComponent<Beam>().axis = axis;
            beam.transform.GetComponent<Beam>().initial = min;
            beam.transform.GetComponent<Beam>().speed = speed;
            beam.transform.GetComponent<Beam>().max = max;
            beam.transform.GetComponent<Beam>().dmgDelayTime = delay;
            beam.transform.GetComponent<Beam>().trap = this;

            beamScripts.Add(beam.transform.GetComponent<Beam>());
            beams.Add(beam);
            InitiateSound();
            return;
        }
        InitiateSound();
        beams[index].SetActive(true);
        beamScripts[index].Reset(min);
    }

    private void InitiateSound()
    {
        if(!audioSourceContiniousFX.isPlaying)
            audioSourceContiniousFX.Play();
        PlaySound(0);

    }
}
