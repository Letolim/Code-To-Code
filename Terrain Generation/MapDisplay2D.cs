using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay2D : MonoBehaviour
{
    Texture2D mapDisplayTexture;
    Texture2D heightmapDisplayTexture;
    Texture2D layerHeightmapDisplayTexture;

    GameObject heightPlane;
    GameObject layerHeightPlane;

    void Awake()
    {
        heightPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        heightPlane.transform.position = new Vector3(-5, 0, 0);
        layerHeightPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        layerHeightPlane.transform.position = new Vector3(5, 0, 0);
    }
    public void DrawBoolMap(bool[,] map)
    {
        heightmapDisplayTexture = new Texture2D(map.GetLength(0), map.GetLength(1), TextureFormat.RGBA32, false, true);

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                float c = 0;
                if (map[x, y])
                    c = 1;
           
                heightmapDisplayTexture.SetPixel(x, y, new Color(c, c, c));
            }
        heightmapDisplayTexture.Apply();
        heightPlane.transform.GetComponent<MeshRenderer>().material.mainTexture = heightmapDisplayTexture;
        //heightPlane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", heightmapDisplayTexture);
    }
    public void DrawBoolMap(bool[,] map, List<Vector2Int> startPositions)
    {
        heightmapDisplayTexture = new Texture2D(map.GetLength(0), map.GetLength(1), TextureFormat.RGBA32, false, true);

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                float c = 0;
                if (map[x, y])
                    c = 1;

                heightmapDisplayTexture.SetPixel(x, y, new Color(c, c, c));
            }

        for(int i = 0; i < startPositions.Count; i ++)
            heightmapDisplayTexture.SetPixel(startPositions[i].x, startPositions[i].y, new Color(1, .3f, .3f));

        heightmapDisplayTexture.Apply();
        heightPlane.transform.GetComponent<MeshRenderer>().material.mainTexture = heightmapDisplayTexture;
        //heightPlane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", heightmapDisplayTexture);
    }

    public void DrawHeightMap(float[,] map, float maxMapValue)
    {
        heightmapDisplayTexture = new Texture2D(map.GetLength(0), map.GetLength(1), TextureFormat.RGBA32, false, true);

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                float c = map[x, y];

                //if (maxMapValue >= 1f)
                   // c /= maxMapValue;

                heightmapDisplayTexture.SetPixel(x, y, new Color(c,c,c));
            }
        heightmapDisplayTexture.Apply();
        heightPlane.transform.GetComponent<MeshRenderer>().material.mainTexture = heightmapDisplayTexture;
        //heightPlane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", heightmapDisplayTexture);
    }



    public void DrawLayerHeightMap(int[,] map, SurfaceLayer[] layer)
    {
        layerHeightmapDisplayTexture = new Texture2D(map.GetLength(0), map.GetLength(1), TextureFormat.RGBA32, false, true);

        Color[] colors = new Color[layer.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            if (layer[i].surfaceName == "Sand")
            { 
                colors[i] = new Color(0.7f, 0.7f, 0.3f);
                continue;
            }
            if (layer[i].surfaceName == "Gravel")
            {
                colors[i] = new Color(0.7f, 0.7f, 0.7f);
                continue;
            }
            if (layer[i].surfaceName == "Grass")
            {
                colors[i] = new Color(0.5f, 0.8f, 0.5f);
                continue;
            }
            if (layer[i].surfaceName == "Dirt")
            {
                colors[i] = new Color(0.5f, 0.5f, 0.2f);
                continue;
            }
            if (layer[i].surfaceName == "Forest")
            {
                colors[i] = new Color(0.25f, 0.5f, 0.25f);
                continue;
            }
            if (layer[i].surfaceName == "Rock")
            {
                colors[i] = new Color(0.2f, 0.2f, 0.1f);
                continue;
            }
        }

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                //Color color = colors[map[x,y]];
                layerHeightmapDisplayTexture.SetPixel(x, y, colors[map[x, y]]);
            }

        layerHeightmapDisplayTexture.Apply();
        layerHeightPlane.transform.GetComponent<MeshRenderer>().material.mainTexture = layerHeightmapDisplayTexture;
        //layerHeightPlane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", layerHeightmapDisplayTexture);
    }



    //new SurfaceLayer("Sand", new int[] { -2, -1 , 0}, new float[] { 30, 70, 5 },null), new SurfaceLayer("Gravel", new int[] { -2, -1, 1 }, new float[] { 70, 30, 50 }, null),
    //                                                            new SurfaceLayer("Grass", new int[] { 0 }, new float[] { 60 }, null), new SurfaceLayer("Dirt", new int[] { 0 }, new float[] { 10 }, null), 
    //                                                            new SurfaceLayer("Forest", new int[] { 0 }, new float[] { 20 }, null), new SurfaceLayer("Rock", new int[] { 1, 2 }, new float[] { 70, 100 }, null)};


    public void DrawLayerHeightMap(int[,] map, int layerCount)
    {
        layerHeightmapDisplayTexture = new Texture2D(map.GetLength(0), map.GetLength(1), TextureFormat.RGBA32, false, true);

        Color[] colors = new Color[layerCount];

        for (int i = 0; i < colors.Length; i++)
            colors[i] = new Color(Random.Range(.0f, 1f), Random.Range(.0f, 1f), Random.Range(.0f, 1f));
        

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                //Color color = colors[map[x,y]];
                layerHeightmapDisplayTexture.SetPixel(x, y, colors[map[x, y]]);
            }

        layerHeightmapDisplayTexture.Apply();
        layerHeightPlane.transform.GetComponent<MeshRenderer>().material.mainTexture = layerHeightmapDisplayTexture;
        //layerHeightPlane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", layerHeightmapDisplayTexture);
    }

    public void DrawLayerHeightMap(TerrainDataMap map,TerrainMapHeightLayer[] heightLayers)
    {
        layerHeightmapDisplayTexture = new Texture2D(map.layerMap.GetLength(0), map.layerMap.GetLength(1), TextureFormat.RGBA32, false, true);

        for (int x = 0; x < map.layerMap.GetLength(0); x++)
            for (int y = 0; y < map.layerMap.GetLength(1); y++)
            {
                layerHeightmapDisplayTexture.SetPixel(x, y, heightLayers[map.layerMap[x, y]].color);
            }

        layerHeightmapDisplayTexture.Apply();
        layerHeightPlane.transform.GetComponent<MeshRenderer>().material.mainTexture = layerHeightmapDisplayTexture;
        //layerHeightPlane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", layerHeightmapDisplayTexture);
    }


    public void DrawMap(float[,] map, float maxMapValue)
    {
        mapDisplayTexture = new Texture2D(map.GetLength(0), map.GetLength(1),TextureFormat.RGBA32,false,true);

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                
                float c = map[x, y];

                if(maxMapValue >= 1f)
                    c /= maxMapValue;
                Color color = Color.white;
                if (c < .4f)
                    color = new Color(.4f, .4f, 1f);
                    else if(c < .6)
                        color = new Color(.4f, .3f, .2f);
                            else if (c < .8)
                                color = new Color(.2f, .5f, .2f);
                                    else if (c < .9)
                                        color = new Color(.7f, .7f, .7f);

                mapDisplayTexture.SetPixel(x, y, color);
            }

        mapDisplayTexture.Apply();
        //plane.transform.GetComponent<MeshRenderer>().material.mainTexture = mapDisplayTexture;
        //plane.transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", mapDisplayTexture);

    }






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
