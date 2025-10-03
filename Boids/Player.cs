using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public float speed = 7.5f;
    public float cameraRotation = 45;

    public Stick movementStick;
    public Stick rotationStick;

    public Sprite filledHealthPoint;
    public Sprite emptydHealthPoint;

    public List<Image> healthPoints;
    public int healthPointsCount = 3;
    
    private Rigidbody body;
    private NavMeshAgent navAgent;

    public GameObject speedFX; 
    public ParticleSystem speedFX01;
    public ParticleSystem speedFX02;


    public RectMask2D shieldBar;
    public GameObject shieldObject;
    public ParticleSystem shieldFX01;
    public ParticleSystem shieldFX02;


    public AudioSource playerAudioSource;
    public AudioClip shieldSoundFadeIn;
    public AudioClip shieldSoundFadeOut;

    private float shieldDelay = 6f;
    private float currentShieldTimer = 0;
    private bool shieldIsActive = false;
    private float shieldEmission = 50f;

    private Timer healthDelay;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
        healthDelay = new Timer();
    }


    private void UpdateShield()
    {
        if (!shieldIsActive)
        {
            currentShieldTimer += Time.deltaTime;
            if (currentShieldTimer > shieldDelay)
            {
                playerAudioSource.PlayOneShot(shieldSoundFadeIn);
                shieldIsActive = true;
                currentShieldTimer = 0;
                return;
            }

            float delta = currentShieldTimer / shieldDelay;
            shieldBar.padding = new Vector4(0, 0, delta * -344, 0);
            Color color = shieldObject.GetComponent<MeshRenderer>().material.color;
            shieldObject.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, delta);

            ParticleSystem.EmissionModule emission = shieldFX01.emission;
            emission.rateOverTime = delta * shieldEmission;

        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShield();
        healthDelay.Update();
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    RaycastHit hitinfo = new RaycastHit();
        //    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, 50f, 1 << LayerMask.NameToLayer("World"));
        //    this.transform.GetComponent<NavMeshAgent>().SetDestination(hitinfo.point);
        //}

        //if (rotationStick.dragging)
        //    this.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(-rotationStick.dir.x, rotationStick.dir.y) * Mathf.Rad2Deg, 0);

        //if (movementStick.dragging)
        //{
        //    float angle = Mathf.Atan2(movementStick.dir.x, movementStick.dir.y) + (cameraRotation + this.transform.rotation.eulerAngles.y) * Mathf.Deg2Rad;
        //    body.velocity = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * speed; 
        //}
        //else
        //    body.velocity = Vector2.zero;

        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
            dir.x =  1;
        if (Input.GetKey(KeyCode.A))
            dir.x = -1;

        if (Input.GetKey(KeyCode.S))
            dir.y = -1;
        if (Input.GetKey(KeyCode.W))
            dir.y =  1;

        if (dir.magnitude != 0)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) + (this.transform.rotation.eulerAngles.y + 45f) * Mathf.Deg2Rad;
            body.velocity = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * speed; 
        }
        else
            body.velocity = Vector2.zero;


        CameraRotation();
    }


    private bool rotating = false;
    private Vector3 mouseDir;
    private Vector3 lastMousePosition;

    private void CameraRotation()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rotating = true;
            mouseDir = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
            rotating = false;

        if (rotating)
        {
            mouseDir = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
            mouseDir.x = Mathf.Clamp(mouseDir.x, -3f, 3f);
        }

        if (mouseDir.x != 0)
        {
            mouseDir.x = Mathf.MoveTowards(mouseDir.x, 0, Time.deltaTime * 2f);
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + mouseDir.x, 0);
            cameraRotation += mouseDir.x;
        }

    }

    public void UpdatePlayer(int type, int value1, float value2)
    {
        if (type == -1)
        {
            speed += value2;
            navAgent.speed = speed;

            if (!speedFX.active)
                speedFX.SetActive(true);

            ParticleSystem.EmissionModule emission = speedFX01.emission;
            emission.rateOverTime = 10 + emission.rateOverTime.constant;
            ParticleSystem.MainModule main = speedFX02.main;
            main.startLifetime = .25f + main.startLifetime.constant;
            main.startSize = .025f + main.startSize.constant;
        }

        if (type == -2)
        {
            shieldDelay += value2;
            shieldEmission += 30;
            ParticleSystem.EmissionModule emission = shieldFX02.emission;
            ParticleSystem.Burst burst = new ParticleSystem.Burst(.01f, emission.GetBurst(0).count.constant + 30);
            emission.SetBurst(0, burst);
        }

    }

    public void ChangeLife(int mod)
    {
        if (mod == -1)
        {

            if (!healthDelay.running)
                healthDelay.Start(.3f);
            else
                return;


            if (shieldIsActive)
            {
                shieldIsActive = false;
                playerAudioSource.PlayOneShot(shieldSoundFadeOut);
                shieldFX02.Play();
                return;
            }
        }

        int newHealthPointsCount = healthPointsCount + mod;

        if (newHealthPointsCount == -1)
            return;

        if (newHealthPointsCount > healthPointsCount)
            healthPoints[newHealthPointsCount - 1].sprite = filledHealthPoint;
        else
            healthPoints[healthPointsCount - 1].sprite = emptydHealthPoint;

        healthPointsCount = newHealthPointsCount;
    }
}