using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBeamTrap : MonoBehaviour
{
    public GameObject player;
    public GameObject beamTemplate;

    public float beamSpeed = 5f;
    public float beamLength = 5f;
    public int beamCount = 4;
    public float offset = 45;

    private List<RotationBeam> beams = new List<RotationBeam>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < beamCount; i++)
        {
            GameObject beam = GameObject.Instantiate(beamTemplate);
            beam.transform.SetParent(this.transform, false);
            beam.SetActive(true);
            RotationBeam beamScript = beam.transform.GetComponentInChildren<RotationBeam>();
            beam.transform.localPosition = Vector3.zero;

            if (offset == -1)
                beamScript.Initiate(360f * (float)((float)i / (float)beamCount) , beamLength, beamSpeed);
            else
                beamScript.Initiate(offset, beamLength, beamSpeed);

            beams.Add(beamScript);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        for (int i = 0; i < beams.Count; i ++)
            beams[i].ExternalUpdate(angle,distance);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .5f, .5f, .25f);
        Gizmos.DrawSphere(this.transform.position, beamLength);
    }
}
