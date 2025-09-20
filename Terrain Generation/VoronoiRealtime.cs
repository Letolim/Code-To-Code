using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiRealtime : MonoBehaviour
{
    public float delay = 1f;
    float curTime = 0f;

    int[,] layerMap;
    int[,] layerMapB;

    int width = 512;
    int height = 512;
    int numbOfPoints = 100;
    List<Vector2Int> points;
    
    List<Color> colors;
    Texture2D mapDisplayTexture;
    Texture2D heightmapDisplayTexture;

    GameObject heightPlaneb;
    void Awake()
    {
        heightPlaneb = GameObject.CreatePrimitive(PrimitiveType.Plane);
        heightPlaneb.transform.position = new Vector3(0, 2, 10);

    }
    // Start is called before the first frame update
    void Start()
    {

        layerMap = new int[width, height];
        layerMapB = new int[width, height];

        points = new List<Vector2Int>();
        colors = new List<Color>();

        for (int i = 0; i < numbOfPoints; i++)
        {
            points.Add(new Vector2Int(Random.Range(1, width), Random.Range(1, width)));
            colors.Add(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
        }
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            { 
                layerMap[x, y] = -1;
                layerMapB[x, y] = -1;
            }

        for (int i = 0; i < numbOfPoints; i++)
            layerMap[points[i].x, points[i].y] = i;
    }

    float currentDistance = 1;

    bool tick = true;
    int stepps = 0;

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > delay)
        {
            curTime = 0;
            stepps++;
            DrawEuclidian();



            //if (stepps % 3 != 0)
            //{
            //    DrawEucliadian();
            //    tick = false;
            //}
            //else
            //{
            //    DrawManhattan();
            //    tick = true;
            //}
           // 

        }
    }
    public void DrawEuclidian()
    {
        heightmapDisplayTexture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);

        for (int i = 0; i < numbOfPoints; i++)
        {
            for (int p = 0; p < 360; p += 1)
            {
                float dirX = Mathf.Cos((float)p * Mathf.Deg2Rad);
                float dirY = Mathf.Sin((float)p * Mathf.Deg2Rad);
                int posX = points[i].x + (int)((float)currentDistance * (float)dirX);
                int posY = points[i].y + (int)((float)currentDistance * (float)dirY);

                if (posX >= 0 && posY >= 0 && posX <= width - 1 && posY <= height - 1)
                {
                    if (layerMap[posX, posY] == -1)
                        layerMapB[posX, posY] = layerMap[points[i].x, points[i].y];
                }
            }
            layerMapB[points[i].x, points[i].y] = layerMap[points[i].x, points[i].y];
        }

        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
                layerMap[x, y] = layerMapB[x, y];

        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
            {
                if (layerMap[x, y] == -1)
                    heightmapDisplayTexture.SetPixel(x, y, Color.black);
                else
                    heightmapDisplayTexture.SetPixel(x, y, colors[layerMap[x, y]]);
            }

        heightmapDisplayTexture.Apply();
        heightPlaneb.transform.GetComponent<MeshRenderer>().material.mainTexture = heightmapDisplayTexture;

        currentDistance += .2f;
    }


    public void DrawManhattan()
    {
        heightmapDisplayTexture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);

        for (int x = 2; x < width - 2; x++)
            for (int y = 2; y < height - 2; y++)
            {

                if (layerMap[x, y] != -1)
                {
                    if (layerMap[x + 1, y] == -1)
                        layerMapB[x + 1, y] = layerMap[x, y];

                    if (layerMap[x, y + 1] == -1)
                        layerMapB[x, y + 1] = layerMap[x, y];

                    if (layerMap[x, y - 1] == -1)
                        layerMapB[x, y - 1] = layerMap[x, y];

                    if (layerMap[x - 1, y] == -1)
                        layerMapB[x - 1, y] = layerMap[x, y];



                    if (layerMap[x + 1, y + 1] == -1)
                        layerMapB[x + 1, y + 1] = layerMap[x, y];

                    if (layerMap[x + 1, y - 1] == -1)
                        layerMapB[x + 1, y - 1] = layerMap[x, y];

                    if (layerMap[x - 1, y + 1] == -1)
                        layerMapB[x - 1, y + 1] = layerMap[x, y];

                    if (layerMap[x - 1, y - 1] == -1)
                        layerMapB[x - 1, y - 1] = layerMap[x, y];



                }
            }

        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
                layerMap[x, y] = layerMapB[x, y];

        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
            {
                if (layerMap[x, y] == -1)
                    heightmapDisplayTexture.SetPixel(x, y, Color.black);
                else
                    heightmapDisplayTexture.SetPixel(x, y, colors[layerMap[x, y]]);
            }

        heightmapDisplayTexture.Apply();
        heightPlaneb.transform.GetComponent<MeshRenderer>().material.mainTexture = heightmapDisplayTexture;
    }


}
