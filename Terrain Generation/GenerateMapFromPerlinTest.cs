using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GenerateMapFromPerlinTest : MonoBehaviour
{
    public GameObject meshTemplate;
    private GameObject[,] meshTemplates;
    public GameObject[] treeTemplates;
    public GameObject grassTemplate;
    public int width = 1024;
    public int height = 1024;
    public float scale = 60f;
    public float persistance = 0.5f;
    public float lacunarity = 2f;
    public int octaves = 2;
    public bool generate = false;
    public MapDisplay2D mapDisplay;
    public MeshFilter[] meshFilter;
    PerlinNoise perl;
    CellNoise cell;
    private GenerateMeshTest meshGenerationTest;
    private TerrainDataMap map;
    TerrainMapHeightLayer[] terrainMapHeightLayers;
    MapGenerationBorderGenerator borderGenerator;
    MapGenerationPointCloud pointCloud;
    GabrielGraph gabrielGraph;
    Color[] colors;
    TerrainDataGenerator mapGen;

    // Start is called before the first frame update
    void Start()
    {
        mapGen = new TerrainDataGenerator();

        borderGenerator = new MapGenerationBorderGenerator();
        map = new TerrainDataMap(width, height);
        //terrainMapHeightLayers = new TerrainMapHeightLayer[] {
        //    new TerrainMapHeightLayer("Mountain", 0, .8f, 5f),
        //    new TerrainMapHeightLayer("Hill", 0, .6f, 4f),
        //    new TerrainMapHeightLayer("Ground", 0, .4f, 3f),
        //    new TerrainMapHeightLayer("Shallow Water", 0, .2f, 2f),
        //    new TerrainMapHeightLayer("Deep Water", 0, 0, 1f)
        //    };
        //terrainMapHeightLayers = new TerrainMapHeightLayer[] {
        //    new TerrainMapHeightLayer("Mountain", 4     , .8f   , 4f    , new Color(.3f,.3f,.3f)),
        //    new TerrainMapHeightLayer("Hill", 3         , .6f   , 3f    , new Color(.6f,.6f,.6f)),
        //    new TerrainMapHeightLayer("Ground", 2       , .4f   , 2f    , new Color(.4f,.7f,.4f)),
        //    new TerrainMapHeightLayer("Shallow Water", 1, .2f   , 1f    , new Color(.5f,.5f,.8f)),
        //    new TerrainMapHeightLayer("Deep Water", 0   , .0f   , 0f    , new Color(.2f,.2f,.5f))
        //    };
        terrainMapHeightLayers = new TerrainMapHeightLayer[] {
            new TerrainMapHeightLayer("Mountain", 4     , .8f   , 4f    , new Color(.3f,.3f,.3f),1f,4f),
            new TerrainMapHeightLayer("Hill", 3         , .6f   , 3f    , new Color(.6f,.6f,.6f),25f,2f),
            new TerrainMapHeightLayer("Ground", 2       , .2f   , 2f    , new Color(.4f,.7f,.4f),3f,2f),
            new TerrainMapHeightLayer("Shallow Water", 1, .1f   , 1f    , new Color(.5f,.5f,.8f),2f,3f),
            new TerrainMapHeightLayer("Deep Water", 0   , .0f   , 0f    , new Color(.2f,.2f,.5f),1f,4f)
            };

        map.terrainLayer = terrainMapHeightLayers;
        gabrielGraph = new GabrielGraph();
        pointCloud = new MapGenerationPointCloud(terrainMapHeightLayers.Length, .5f);
        //TerrainMapHeightLayer Layer0 = new TerrainMapHeightLayer("Deep Water", 0, .16f);//TerrainMapHeightLayer Layer1 = new TerrainMapHeightLayer("Shallow Water", 0, .32f);//TerrainMapHeightLayer Layer2 = new TerrainMapHeightLayer("Ground", 0, .48f);//TerrainMapHeightLayer Layer3 = new TerrainMapHeightLayer("Hill", 0, .64f);//TerrainMapHeightLayer Layer4 = new TerrainMapHeightLayer("Mountain", 0, .8f);
        GenerateLayerHeightMap(map,terrainMapHeightLayers);
        meshGenerationTest = new GenerateMeshTest();

        perl = new PerlinNoise();
        cell = new CellNoise();



    }

    void Update()
    {
        if (generate && mapDisplay != null)
        {
            map = mapGen.GenerateTerrain(null, width, height);

            //GenerateHeightMap(map);
            //Erode(50000, map.heightMap, Random.Range(0, 50000000));
            //GenerateLayerHeightMap(map, terrainMapHeightLayers);
            mapDisplay.DrawHeightMap(map.heightMap, map.heightMapMaxValue);
            mapDisplay.DrawLayerHeightMap(map.surfaceMap, mapGen.biomeGenerationSettings.layer);

            //mapDisplay.DrawLayerHeightMap(map.layerMap, mapGen.biomeGenerationSettings.layer.Length);

            //mapDisplay.DrawLayerHeightMap(map, terrainMapHeightLayers);
            //borderGenerator.GenerateMeshPoints(Random.Range(0,400000),map, terrainMapHeightLayers, pointCloud);
            colors = new Color[mapGen.biomeGenerationSettings.layer.Length];
            GameObject[] cubes = new GameObject[mapGen.biomeGenerationSettings.layer.Length];

            //Mesh[,] mesohnator = meshGenerationTest.GenerateMesh(map.layerMap, width, height, 8, mapGen.terrainGenerationSettings.layer,map.surfaceMap, mapGen.biomeGenerationSettings.layer);
            Mesh[,] mesohnator = meshGenerationTest.GenerateMesh(map.layerMap, width, height, 8, mapGen.terrainGenerationSettings.layer, map.surfaceMap, mapGen.biomeGenerationSettings.layer,map.heightMap, treeTemplates, grassTemplate);

            if (meshTemplates == null)
            {
                meshTemplates = new GameObject[mesohnator.GetLength(0), mesohnator.GetLength(1)];
                for (int x = 0; x < mesohnator.GetLength(0); x++)
                    for (int y = 0; y < mesohnator.GetLength(1); y++)
                    {
                        meshTemplates[x, y] = GameObject.Instantiate(meshTemplate);
                    }
            }


            generate = false;
            for (int x = 0; x < mesohnator.GetLength(0); x++)
                for (int y = 0; y < mesohnator.GetLength(1); y++)
                { 
                    meshTemplates[x, y].transform.GetComponent<MeshFilter>().mesh = mesohnator[x, y];
                }

            return;
            for (int i = 0; i < mapGen.biomeGenerationSettings.layer.Length; i++)
            {
                if (mapGen.biomeGenerationSettings.layer[i].surfaceName == "Sand")
                {
                    colors[i] = new Color(0.7f, 0.7f, 0.3f);
                    cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i].transform.GetComponent<MeshRenderer>().material.color = colors[i];
                    continue;
                }
                if (mapGen.biomeGenerationSettings.layer[i].surfaceName == "Gravel")
                {
                    colors[i] = new Color(0.7f, 0.7f, 0.7f);
                    cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i].transform.GetComponent<MeshRenderer>().material.color = colors[i];
                    continue;
                }
                if (mapGen.biomeGenerationSettings.layer[i].surfaceName == "Grass")
                {
                    colors[i] = new Color(0.5f, 0.8f, 0.5f);
                    cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i].transform.GetComponent<MeshRenderer>().material.color = colors[i];
                    continue;
                }
                if (mapGen.biomeGenerationSettings.layer[i].surfaceName == "Dirt")
                {
                    colors[i] = new Color(0.3f, 0.3f, 0.25f);
                    cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i].transform.GetComponent<MeshRenderer>().material.color = colors[i];
                    continue;
                }
                if (mapGen.biomeGenerationSettings.layer[i].surfaceName == "Forest")
                {
                    colors[i] = new Color(0.4f, 0.55f, 0.2f);
                    cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i].transform.GetComponent<MeshRenderer>().material.color = colors[i];
                    continue;
                }
                if (mapGen.biomeGenerationSettings.layer[i].surfaceName == "Rock")
                {
                    colors[i] = new Color(0.2f, 0.2f, 0.1f);
                    cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i].transform.GetComponent<MeshRenderer>().material.color = colors[i];
                    continue;
                }

            }
            float drawScale = .05f;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    //Debug.DrawLine(new Vector3(x, mapGen.terrainGenerationSettings.layer[map.layerMap[x, y]].worldHeight * 3, y) * drawScale, new Vector3(x, mapGen.terrainGenerationSettings.layer[map.layerMap[x, y]].worldHeight * 3 + 5, y) * drawScale, colors[map.surfaceMap[x,y]], 30f);
                    GameObject cuby = GameObject.Instantiate(cubes[map.surfaceMap[x, y]]);
                    cuby.transform.position = new Vector3(x, mapGen.terrainGenerationSettings.layer[map.layerMap[x, y]].worldHeight, y);
                }
            generate = false;
            return;

            //Mesh[] mesh = meshGenerationTest.GenerateMesh(pointCloud);
            //Mesh[] mesh = meshGenerationTest.GenerateMesh(map);

            //for (int i = 0; i < mesh.Length; i++)
            //{
            //    GameObject meshHolder = GameObject.CreatePrimitive(PrimitiveType.Quad);
            //    meshHolder.transform.GetComponent<MeshFilter>().mesh = mesh[i];
            //}

            //generate = false;
            //return;

            Thread gabrielThread = new Thread(StartGabrielThread);
            gabrielThread.Start();

            //gabrielGraph.GenerateGraph(pointCloud, 6f);
            c = 0;
            generate = false;
            return;

            for (int i = 0; i < pointCloud.vertices.Count; i++)
            {
                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                for (int n = 0; n < pointCloud.vertices[i].Count; n++)
                    Debug.DrawLine(pointCloud.vertices[i][n], (pointCloud.vertices[i][n]  + Vector3.up), terrainMapHeightLayers[pointCloud.layerIndex[i]].color, 30f);
            }

            generate = false;
        }

        if (gabrielGraph.lines.Count != 0 && c < gabrielGraph.lines.Count)
        {
            Color color = (colors[gabrielGraph.lines[c].layerA] + colors[gabrielGraph.lines[c].layerB]) / 2f;
            Debug.DrawLine(gabrielGraph.lines[c].startPosition, gabrielGraph.lines[c].endPosition, color, 30000000000);
            c++;
        }

    }

    int c = 0;
    public void StartGabrielThread()
    {
        gabrielGraph.GenerateGraph(pointCloud, 9f, width, height);
    }

    public void Erode(int amount, float[,] heightMap, int seed)
    {
        for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
                heightMap[x, y] = heightMap[x, y] * 2f;
        int count = 0;
        float speed = .5f;
        int maxIterations = 2500;
        System.Random randomGen = new System.Random(seed);
        float[,] erosionMap = new float[heightMap.GetLength(0), heightMap.GetLength(1)];
        while (count <= amount)
        {
            float positionX = randomGen.Next(16, heightMap.GetLength(0) -16);
            float positionY = randomGen.Next(16, heightMap.GetLength(1) -16);
            if (heightMap[(int)positionX, (int)positionY] <= .5f)
            {
                count++;
                continue; 
            }
            int offsetX = randomGen.Next(-1, 1);
            int offsetY = randomGen.Next(-1, 1);
            float velocityX = 0;
            float velocityY = 0;
            GridTools.Slope2D normal = GridTools.GetSlope((int)positionX + offsetX, (int)positionY + offsetY, heightMap);
        
            float deposit = 0;
            float erosion = (1f - normal.length) * .01f;
            float sediment = erosion;

            for (int i = 0; i < maxIterations; i++)
            {
                normal = GridTools.GetSlope((int)positionX + offsetX + 2, (int)positionY + offsetY + 2, heightMap);
                if (normal.length <= 0.01f)
                    break;
                float mapValue = heightMap[(int)positionX, (int)positionY];

                deposit = sediment * normal.length * .01f;
                erosion = (1f - normal.length) * .01f;
                heightMap[(int)positionX, (int)positionY] = mapValue -erosion;
                sediment += erosion - deposit;
                velocityX = Mathf.Clamp(velocityX + normal.directionX * speed, -2f,2f);
                velocityY = Mathf.Clamp(velocityY + normal.directionY * speed, -2f, 2f);
                //velocityX = velocityX + normal.directionX * speed;
                //velocityY = velocityY + normal.directionY * speed;
                positionX += velocityX;
                positionY += velocityY;
      

                if (!GridTools.IsInsideArrayBounds2D((int)positionX, (int)positionY, heightMap.GetLength(0), heightMap.GetLength(1)))
                    break;
            }

            count++;
        }
       
    }

   

    public void GenerateHeightMap(TerrainDataMap map)
    {
        float maxValue = 0f;
        float minValue = 50000000f;

        if (scale <= 0f)
            scale = 0.00001f;
        float halfWidht = map.width / 2;
        float halfHeight = map.height / 2;


        float[] octaveOffsetsX = new float[octaves];
        float[] octaveOffsetsY = new float[octaves];
        bool[] octaveAdditive = new bool[octaves];



        for (int i = 0; i < octaves; i++)
        {
            octaveOffsetsX[i] = Random.Range(-10000f, 10000f);
            octaveOffsetsY[i] = Random.Range(-10000f, 10000f);
            if (i % 2 == 0)
                octaveAdditive[i] = true;
            else
                octaveAdditive[i] = false;
        }

        int type = 2;

        for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
            {
                float noiseHeight = 0;
                float frequency = 1f;
                float amplitude = 1f;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (float)(x - halfHeight) / scale * frequency + octaveOffsetsX[o];
                    float sampleY = (float)(y - halfWidht) / scale * frequency + octaveOffsetsY[o];

                    if (type == 0)
                    { 
                        if (octaveAdditive[o])
                            noiseHeight += perl.SimplexNoise(sampleX, sampleY) * amplitude;
                        else
                            noiseHeight -= perl.SimplexNoise(sampleX, sampleY) * amplitude;
                    }

                    if (type == 1)
                    {
                        noiseHeight += cell.VoronoiNoise(sampleX, sampleY, 1f) * amplitude;
                    }

                    if (type == 2)
                    {
                        noiseHeight += perl.SimplexNoise(sampleX , sampleY) * amplitude;
                        noiseHeight += cell.VoronoiNoise(sampleX * 5f, sampleY * 5f, 1f) * amplitude;
                    }

                    frequency *= lacunarity;
                    amplitude *= persistance;
                }

                if (noiseHeight > maxValue)
                    maxValue = noiseHeight;
                if (noiseHeight < minValue)
                    minValue = noiseHeight;
                map.heightMap[x, y] = noiseHeight;
            }
        Debug.Log(maxValue);
        Debug.Log(minValue);
        map.heightMapMaxValue = maxValue;
        map.heightMapMinValue = minValue;
        for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
            {
                map.heightMap[x, y] = Mathf.InverseLerp(minValue, maxValue, map.heightMap[x, y]);
            }
    }

    public void GenerateLayerHeightMap(TerrainDataMap map, TerrainMapHeightLayer[] heightLayer)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                for (int i = 0; i < heightLayer.Length; i++)
                    if (map.heightMap[x, y] >= heightLayer[i].threshold)
                    {
                        map.layerHeightMap[x, y] = heightLayer[i].height;
                        map.layerMap[x,y] = i;
                        break;
                    }



    }


}





    // Update is called once per frame
   














    //float[,] map;

    //map = new float[width, height];

    //float maxValue = 0f;
    //float minValue = 50000000f;

    //if (scale <= 0f)
    //    scale = 0.00001f;
    //float halfWidht = width / 2;
    //float halfHeight = height / 2;


    //float[] octaveOffsetsX = new float[octaves];
    //float[] octaveOffsetsY = new float[octaves];



    //for (int i = 0; i < octaves; i++)
    //{
    //    octaveOffsetsX[i] = Random.Range(-10000f, 10000f);
    //    octaveOffsetsY[i] = Random.Range(-10000f, 10000f);
    //}


    //for (int x = 0; x < width; x++)
    //    for (int y = 0; y < height; y++)
    //    {
    //        float noiseHeight = 0;
    //        float frequency = 1f;
    //        float amplitude = 1f;

    //        for (int o = 0; o < octaves; o++)
    //        {
    //            float sampleX = (float)(x - halfHeight) / scale * frequency + octaveOffsetsX[o];
    //            float sampleY = (float)(y - halfWidht) / scale * frequency + octaveOffsetsY[o];
    //            noiseHeight += Mathf.PerlinNoise(sampleX, sampleY) * amplitude;
    //            frequency *= lacunarity;
    //            amplitude *= persistance;
    //        }

    //        if (noiseHeight > maxValue)
    //            maxValue = noiseHeight;
    //        if (noiseHeight < minValue)
    //            minValue = noiseHeight;
    //        map[x, y] = noiseHeight;
    //    }


    //Debug.Log(maxValue);
    //Debug.Log(minValue);
    //mapDisplay.DrawMap(map, maxValue);
    //generate = false;
