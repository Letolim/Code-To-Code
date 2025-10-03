using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SquaredFloorTrap : MonoBehaviour
{
    public float scale = 1.5f;
    public float heightOffset = .5f;
    public float spacing = 1f;
    public int count = 5;
    public ProjectileBuffer projectileBuffer;

    private List<Vector3> emissionPoints = new List<Vector3>();
    private List<Timer> emissionTimer = new List<Timer>();

    public float delay = 1f;
    private float currentTime = 0;

    public AudioSource audioSourceFX;
    public AudioClip fadeInClip;
    public AudioClip mainClip;

    private List<float> mainSoundTimers = new List<float>();
    private float fadeInDuration = 6.5f;
    public Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        audioSourceFX.transform.SetParent(this.transform, true);
        this.transform.transform.localScale = Vector3.one;
        Vector3 initialPosition = this.transform.position;
        GenerateMesh();

        float distance = scale * 2f + spacing * 2f;
        //        this.transform.position = initialPosition -  new Vector3(distance * (float)(count) /2f - distance /2f, heightOffset, (distance * (float)((float)count +.5f)) / 2f - spacing * 2f);

        this.transform.position = initialPosition -  new Vector3(distance * (float)(count) /2f - distance /2f, heightOffset, distance * (float)(count) / 2f - distance / 2f);
        for (int i = 0; i < emissionPoints.Count; i++)
            emissionPoints[i] += this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > delay)
        {
            int index = Random.Range(0, emissionPoints.Count);

            if (!emissionTimer[index].running)
            {
                emissionTimer[index].Start(8);
                projectileBuffer.SpawnNewProjectile(6, new Vector4(emissionPoints[index].x, emissionPoints[index].y, emissionPoints[index].z, scale));
                currentTime = 0;
                //audioSourceFX.PlayOneShot(fadeInClip);
            }

        }

        for (int i = 0; i < emissionTimer.Count; i++)
            emissionTimer[i].Update();

        SoundLoop();
    }

    private void SoundLoop()
    {
        for (int i = 0; i < mainSoundTimers.Count; i++)
        {
            mainSoundTimers[i] += Time.deltaTime;

            if (mainSoundTimers[i] > fadeInDuration)
            {
                audioSourceFX.PlayOneShot(mainClip);
                mainSoundTimers.RemoveAt(i);
                i--;
            }
        }
    }

    private void GenerateMesh()
    {
        List<Vector3> verticis = new List<Vector3>();
        List<int> triangles = new List<int>();
        spacing /= 2f;

        float distance = scale * 2f + spacing * 2f;
        GenerateBorder(distance, spacing, verticis, triangles,heightOffset);
        for (int x = 0; x < count; x++)
            for (int z = 0; z < count; z++)
            {
                MeshPart meshPart = GenerateFlooring(new Vector3((float)x * distance, 0, (float)z * distance), scale, heightOffset, spacing, verticis.Count);
                emissionPoints.Add(new Vector3(distance * ((float)x), heightOffset, distance * ((float)z)));
                emissionTimer.Add(new Timer());
                verticis.AddRange(meshPart.verticis);
                triangles.AddRange(meshPart.triangles);
            }

        Mesh mesh = new Mesh();
        mesh.SetVertices(verticis);
        mesh.triangles = triangles.ToArray();
        mesh.UploadMeshData(false);
        mesh.RecalculateNormals();

        this.transform.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void GenerateBorder(float distance, float spacing, List<Vector3> verticis, List<int> triangles,float heightOffset)
    {
        Vector3 va = new Vector3(distance * (float)((float)count -.5f), heightOffset, distance * (float)((float)count -.5f));
        Vector3 vb = new Vector3(-distance / 2f, heightOffset, -distance / 2f);

        int triangleCount = 0;
        float lowerHeightOffset = heightOffset * 2f;
        verticis.Add(new Vector3(va.x, va.y - lowerHeightOffset, va.z)); //ax    bx
        verticis.Add(new Vector3(va.x, va.y, va.z));
        verticis.Add(new Vector3(vb.x, va.y, va.z));
        verticis.Add(new Vector3(vb.x, va.y - lowerHeightOffset, va.z));

        triangles.Add(1 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(0 + triangleCount);
        triangles.Add(3 + triangleCount);
        triangles.Add(0 + triangleCount);
        triangles.Add(2 + triangleCount);

        triangleCount += 4;

        verticis.Add(new Vector3(va.x, va.y - lowerHeightOffset, va.z));//   az  bz
        verticis.Add(new Vector3(va.x, va.y, va.z));
        verticis.Add(new Vector3(va.x, va.y, vb.z));
        verticis.Add(new Vector3(va.x, va.y - lowerHeightOffset, vb.z));

        triangles.Add(0 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(1 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(0 + triangleCount);
        triangles.Add(3 + triangleCount);

        triangleCount += 4;

        verticis.Add(new Vector3(vb.x, va.y - lowerHeightOffset, vb.z));//   
        verticis.Add(new Vector3(vb.x, va.y, vb.z));
        verticis.Add(new Vector3(vb.x, va.y, va.z));
        verticis.Add(new Vector3(vb.x, va.y - lowerHeightOffset, va.z));

        triangles.Add(0 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(1 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(0 + triangleCount);
        triangles.Add(3 + triangleCount);

        triangleCount += 4;

        verticis.Add(new Vector3(va.x, va.y - lowerHeightOffset, vb.z));
        verticis.Add(new Vector3(va.x, va.y, vb.z));
        verticis.Add(new Vector3(vb.x, va.y, vb.z));
        verticis.Add(new Vector3(vb.x, va.y - lowerHeightOffset, vb.z));

        triangles.Add(0 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(1 + triangleCount);
        triangles.Add(2 + triangleCount);
        triangles.Add(0 + triangleCount);
        triangles.Add(3 + triangleCount);
    }

    private MeshPart GenerateFlooring(Vector3 origin, float scale, float heightOffset, float halfSpacing,int triangleCount)
    {
        Vector3[] plane = new Vector3[] { new Vector3(-1f * scale, 0, 1f * scale) + origin, new Vector3(1f * scale, 0, 1f * scale) + origin, new Vector3(1f * scale, 0, -1f * scale) + origin, new Vector3(-1f * scale, 0, -1f * scale) + origin };
        Vector3[] borderA = new Vector3[] { plane[0], plane[0] + new Vector3(0, heightOffset, 0), plane[1] + new Vector3(0, heightOffset, 0), plane[1] };     // z+
        Vector3[] borderB = new Vector3[] { plane[1], plane[1] + new Vector3(0, heightOffset, 0), plane[2] + new Vector3(0, heightOffset, 0), plane[2] };   // x +
        Vector3[] borderC = new Vector3[] { plane[2], plane[2] + new Vector3(0, heightOffset, 0), plane[3] + new Vector3(0, heightOffset, 0), plane[3] };   // z -
        Vector3[] borderD = new Vector3[] { plane[3], plane[3] + new Vector3(0, heightOffset, 0), plane[0] + new Vector3(0, heightOffset, 0), plane[0] };   // x -

        Vector3[] spacingA = new Vector3[] { borderA[1], borderA[1] + new Vector3(-halfSpacing, 0, halfSpacing), borderA[2] + new Vector3(halfSpacing, 0, halfSpacing), borderA[2] };     // z+
        Vector3[] spacingB = new Vector3[] { borderB[1], borderB[1] + new Vector3(halfSpacing, 0, halfSpacing), borderB[2] + new Vector3(halfSpacing, 0, -halfSpacing), borderB[2] };     // x+
        Vector3[] spacingC = new Vector3[] { borderC[1], borderC[1] + new Vector3(halfSpacing, 0, -halfSpacing), borderC[2] + new Vector3(-halfSpacing, 0, -halfSpacing), borderC[2] };     // x+
        Vector3[] spacingD = new Vector3[] { borderD[1], borderD[1] + new Vector3(-halfSpacing, 0, -halfSpacing), borderD[2] + new Vector3(-halfSpacing, 0, halfSpacing), borderD[2] };

        List<int> triangles = new List<int>();

        for (int i = 0; i < 9; i++)
        {
            triangles.Add(triangleCount + 1);
            triangles.Add(triangleCount + 2);
            triangles.Add(triangleCount);

            triangles.Add(triangleCount + 3);
            triangles.Add(triangleCount);
            triangles.Add(triangleCount + 2);

            triangleCount += 4;
        }

        List<Vector3> verticis = new List<Vector3>();

        verticis.AddRange(plane);
        verticis.AddRange(borderA);
        verticis.AddRange(borderB);
        verticis.AddRange(borderC);
        verticis.AddRange(borderD);
        verticis.AddRange(spacingA);
        verticis.AddRange(spacingB);
        verticis.AddRange(spacingC);
        verticis.AddRange(spacingD);

        MeshPart part = new MeshPart(triangles, verticis);

        return part;
    }

    private class MeshPart
    {
        public List<int> triangles;
        public List<Vector3> verticis;

        public MeshPart(List<int> triangles, List<Vector3> verticis)
        {
            this.triangles = triangles;
            this.verticis = verticis;
        }

        public void clear()
        {
            triangles.Clear();
            verticis.Clear();
        }
    }


    private Vector3 editorPointA;
    private Vector3 editorPointb;
    private Color color = new Color(0.7f,0.7f,0.7f,0.25f);

    private void OnValidate()
    {
       // editorPointA = new Vector3(distance * (float)((float)count - .5f), heightOffset, distance * (float)((float)count - .5f));
       // editorPointb = new Vector3(-distance / 2f, heightOffset, -distance / 2f);
        if (UnityEngine.Application.isPlaying || UnityEngine.Application.isLoadingLevel)
            return;
        Gizmos.color = color;
        float distance = scale * 2f + (spacing / 2f) * 2f;
        //this.transform.transform.localScale = new Vector3(1f, 0, 1f) * (  (distance * ((float)count))) + new Vector3(0, heightOffset * 2f, 0);
                                                                           
        //this.transform.GetComponent<BoxCollider>().size = new Vector3(1f, 0, 1f) * ((distance * ((float)count))) + new Vector3(0, heightOffset * 2f, 0);
    }

    private void OnDrawGizmos()
    {
        float distance = 0;

        if (UnityEngine.Application.isPlaying || UnityEngine.Application.isLoadingLevel)
        { 
            distance = scale * 2f + spacing * 2f;
        }
        else
        {
            distance = scale * 2f + spacing;
            initialPosition = this.transform.position;
        }
        Gizmos.DrawCube(initialPosition - new Vector3(0, heightOffset, 0), new Vector3(1f, 0, 1f) * (distance * (float)count) + new Vector3(0, heightOffset * 2f, 0));

        //Gizmos.DrawSphere(initialPosition, distance * (float) count / 2f);
        //Gizmos.DrawCube(this.transform.position - new Vector3(0, heightOffset, 0), new Vector3(1f, 0, 1f) * ((distance * ((float)count - .5f))) + new Vector3(0, heightOffset * 2f, 0));
        //(distance * (float)((float)count - .5f), heightOffset, distance * (float)((float)count - .5f))

    }
}
