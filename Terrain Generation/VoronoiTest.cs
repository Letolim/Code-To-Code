using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiTest : MonoBehaviour
{
    Texture2D mapDisplayTexture;
    Texture2D heightmapDisplayTexture;

    GameObject heightPlaneb;

    void Awake()
    {
        heightPlaneb = GameObject.CreatePrimitive(PrimitiveType.Plane);
        heightPlaneb.transform.position = new Vector3(0, 0, 10);

    }


    public void DrawVoronoi(int width, int height, int numbOfPoints)
    {
        heightmapDisplayTexture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);

        List<Vector2Int> points = new List<Vector2Int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < numbOfPoints; i++)
        {
            points.Add(new Vector2Int(Random.Range(1, width), Random.Range(1, width)));
            colors.Add(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
        }

        int[,] layerMap = new int[width, height];

      

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                layerMap[x , y] = -1;

        for (int i = 0; i < numbOfPoints; i++)
            layerMap[points[i].x, points[i].y] = i;

        bool hadZero = true;

        int n = 0;

        while (hadZero)
        {
            hadZero = false;

            for (int x = 1; x < width -1; x++)
                for (int y = 1; y < height -1; y++)
                {
                    if (layerMap[x, y] == -1)
                        hadZero = true;
                    else
                        {
                            if (layerMap[x + 1, y] == -1)
                                layerMap[x + 1, y] = layerMap[x, y];

                            if (layerMap[x, y + 1] == -1)
                                layerMap[x, y + 1] = layerMap[x, y];

                            if (layerMap[x, y - 1] == -1)
                                layerMap[x, y - 1] = layerMap[x, y];

                            if (layerMap[x - 1, y] == -1)
                                layerMap[x - 1, y] = layerMap[x, y];

                            if (layerMap[x + 1, y + 1] == -1)
                                layerMap[x + 1, y + 1] = layerMap[x, y];

                            if (layerMap[x + 1, y - 1] == -1)
                                layerMap[x + 1, y - 1] = layerMap[x, y];

                            if (layerMap[x - 1, y + 1] == -1)
                                layerMap[x - 1, y + 1] = layerMap[x, y];

                            if (layerMap[x - 1, y - 1] == -1)
                                layerMap[x - 1, y - 1] = layerMap[x, y];


                        }
                }
        }

        Debug.Log(n);

        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
            {
                if (layerMap[x, y] == -1)
                    heightmapDisplayTexture.SetPixel(x, y, Color.black);

                heightmapDisplayTexture.SetPixel(x, y, colors[layerMap[x, y]]);
            }

        heightmapDisplayTexture.Apply();
        heightPlaneb.transform.GetComponent<MeshRenderer>().material.mainTexture = heightmapDisplayTexture;


    }



    // Start is called before the first frame update
    void Start()
    {
        DrawVoronoi(256, 256, 25);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
