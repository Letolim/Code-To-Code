using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDataGenerator
{
    public enum NoiseType { Worley, Cell, Simplex};
    public enum NoiseAplicationType { Additive, Subtractive, Multiplicative};

    public TerrainGenerationData terrainGenerationSettings;
    public BiomeGenerationData biomeGenerationSettings;


    PerlinNoise perl;
    CellNoise cell;
    // Start is called before the first frame update
    public TerrainDataGenerator()
    {
        terrainGenerationSettings = new TerrainGenerationData();
        terrainGenerationSettings.noiseType = NoiseType.Simplex;
        terrainGenerationSettings.scale = 40;
        terrainGenerationSettings.octaves = 2;
        terrainGenerationSettings.lacunarity = 2.1f;
        terrainGenerationSettings.persistance = 0.55f;
        terrainGenerationSettings.worleyBias = 1f;
        terrainGenerationSettings.simplexBias = 1f;
        terrainGenerationSettings.fadeToCenter = false;         // check
        terrainGenerationSettings.fadeFromCenter = false;         // check
        terrainGenerationSettings.fadeToRandomPoint = false;         // check
        terrainGenerationSettings.fadeFromRandomPoint = false;         // check
        terrainGenerationSettings.fadeFromTop = false;
        terrainGenerationSettings.fadeToTop = false;
        terrainGenerationSettings.fadeFromBottom = false;
        terrainGenerationSettings.fadeToBottom = false;
        terrainGenerationSettings.fadeFromLeft = false;
        terrainGenerationSettings.fadeToLeft = false;
        terrainGenerationSettings.fadeFromRight = false;
        terrainGenerationSettings.fadeToRight = false;
        terrainGenerationSettings.layer = new TerrainLayer[] { new TerrainLayer("Mountain",2,30,12,.2f,3f), new TerrainLayer("Hill", 1, 10, 6, .2f, 1f), new TerrainLayer("Ground", 0, 60, 0, .2f, .1f), new TerrainLayer("Shallow Water", -1, 20, -2, .2f, .15f), new TerrainLayer("Deep Water", -2, 10, -4, .2f, .5f) };
        terrainGenerationSettings.layer = new TerrainLayer[] { new TerrainLayer("Mountain", 2, 10, 5, .2f, 2f), new TerrainLayer("Hill", 1, 40, 3, .2f, 1.5f), new TerrainLayer("Ground", 0, 90, 0, .2f, .1f), new TerrainLayer("Shallow Water", -1, 10, -2, .2f, .15f), new TerrainLayer("Deep Water", -2, 5, -4, .2f, .5f) };

        biomeGenerationSettings = new BiomeGenerationData();



        biomeGenerationSettings.noiseType = new NoiseType[]{ NoiseType.Cell, NoiseType.Simplex};
        biomeGenerationSettings.scale = new float[]{ 10, 120};
        biomeGenerationSettings.octaves = new int[]{ 1, 3};
        biomeGenerationSettings.lacunarity = new float[]{ 2, 2.5f};
        biomeGenerationSettings.persistance = new float[]{ .5f, .75f};
        biomeGenerationSettings.magnitude = new float[]{ .3f, .7f};
        biomeGenerationSettings.noiseApplicationType = new NoiseAplicationType[]{ NoiseAplicationType.Subtractive, NoiseAplicationType.Multiplicative};
        biomeGenerationSettings.layer = new SurfaceLayer[] {    new SurfaceLayer("Sand", new int[] { -2, -1}, new float[]{ 30, 70},null),
                                                                new SurfaceLayer("Gravel", new int[] { -2, -1 , 1, 0}, new float[] { 70, 30 , 50, 25}, null),
                                                                new SurfaceLayer("Grass", new int[] {0}, new float[]{25}, null),
                                                                new SurfaceLayer("Dirt", new int[] {0}, new float[]{25}, null),
                                                                new SurfaceLayer("Forest", new int[] {0}, new float[]{25}, null),
                                                                new SurfaceLayer("Rock", new int[] {1,2}, new float[]{50,100}, null)};

        biomeGenerationSettings.noiseType = new NoiseType[] { NoiseType.Simplex, NoiseType.Worley };
        biomeGenerationSettings.noiseApplicationType = new NoiseAplicationType[] { NoiseAplicationType.Additive, NoiseAplicationType.Subtractive};
        biomeGenerationSettings.scale = new float[] { 60, 5 };
        biomeGenerationSettings.octaves = new int[] { 3, 1 };
        biomeGenerationSettings.lacunarity = new float[] { 2.5f, 2 };
        biomeGenerationSettings.persistance = new float[] { .75f, .5f };
        biomeGenerationSettings.magnitude = new float[] { 2f, 2f };
        biomeGenerationSettings.layer = new SurfaceLayer[] {    new SurfaceLayer("Sand", new int[] { -2, -1}, new float[]{ 40, 50},null),
                                                                new SurfaceLayer("Gravel", new int[] { -2, -1 , 1, 0}, new float[] { 60, 50 , 50, 10}, null),
                                                                new SurfaceLayer("Grass", new int[] {0}, new float[]{100}, null),
                                                                new SurfaceLayer("Dirt", new int[] {0}, new float[]{10}, null),
                                                                new SurfaceLayer("Forest", new int[] {0}, new float[]{60}, null),
                                                                new SurfaceLayer("Rock", new int[] {1,2}, new float[]{50,100}, null)};


        perl = new PerlinNoise();
        cell = new CellNoise();
        //new TerrainMapHeightLayer("Mountain", 4, .8f, 4f, new Color(.3f, .3f, .3f), 1f, 4f),
        //    new TerrainMapHeightLayer("Hill", 3, .6f, 3f, new Color(.6f, .6f, .6f), 25f, 2f),
        //    new TerrainMapHeightLayer("Ground", 2, .2f, 2f, new Color(.4f, .7f, .4f), 3f, 2f),
        //    new TerrainMapHeightLayer("Shallow Water", 1, .1f, 1f, new Color(.5f, .5f, .8f), 2f, 3f),
        //    new TerrainMapHeightLayer("Deep Water", 0, .0f, 0f, new Color(.2f, .2f, .5f), 1f, 4f)

    }


    public TerrainDataMap GenerateTerrain(TerrainGenerationData generationData, int mapWidth, int mapHeight)
    {
        generationData = terrainGenerationSettings;
        float[,] heightMap = GenerateHeightMap(mapWidth, mapHeight, generationData.scale, generationData.octaves, generationData.lacunarity, generationData.persistance, generationData.noiseType,1f,1f);
        int[,] layerMap = GenerateLayerMap(heightMap, generationData.layer);
        float[,] layerHeightMap = GenerateLayerHeightMap(layerMap, generationData.layer);
        List<Vector3Int> entryPoints = GenerateEntryPoints(layerMap);
        //float[,] heightMapb = GenerateHeightMap(mapWidth, mapHeight, biomeGenerationSettings.scale[0], biomeGenerationSettings.octaves[0], biomeGenerationSettings.lacunarity[0], biomeGenerationSettings.persistance[0], biomeGenerationSettings.noiseType[0], 1, 1);
        float[,] heightMapb = GenerateHeightMap(mapWidth, mapHeight, biomeGenerationSettings.scale, biomeGenerationSettings.octaves, biomeGenerationSettings.lacunarity, biomeGenerationSettings.persistance , biomeGenerationSettings.magnitude, biomeGenerationSettings.noiseType, biomeGenerationSettings.noiseApplicationType);


        int[,] layerMapb = GenerateSurfaceLayer(heightMapb, layerMap, biomeGenerationSettings.layer, terrainGenerationSettings.layer);
        //
        TerrainDataMap dataMap = new TerrainDataMap(mapWidth, mapHeight);
        dataMap.heightMap = heightMap;
        dataMap.layerMap = layerMap;
        dataMap.surfaceMap = layerMapb;

        dataMap.layerHeightMap = layerHeightMap;
        //

        return dataMap;
    }

    public float[,] GenerateHeightMap(int width, int height, float[] scale, int[] octaves, float[] lacunarity, float[] persistance, float[] magnitude, NoiseType[] noiseType, NoiseAplicationType[] applicationType)
    {
        float maxValue = -50000000f;
        float minValue = 50000000f;

        int randomPointX = Random.Range(0, width);
        int randomPointY = Random.Range(0, width);

        float halfWidht = width / 2;
        float halfHeight = height / 2;

        float maxDistancePointX = System.Math.Max(width - randomPointX,randomPointX);
        float maxDistancepointY = System.Math.Max(height - randomPointY,randomPointY);
        float maxDistanceToRandomPoint = (float)System.Math.Sqrt((maxDistancePointX - randomPointX) * (maxDistancePointX - randomPointX) + (maxDistancepointY - randomPointY) * (maxDistancepointY - randomPointY));

        float maxDistanceToMid = (float)System.Math.Sqrt((halfWidht) * (halfWidht) + (halfHeight) * (halfHeight));
        
        bool fadeToCenter = false;
        bool fadeFromCenter = false;
        bool fadeToRandomPoint = false;
        bool fadeFromRandomPoint = false;
        bool fadeToTop = false;



        float[,] heightMap = new float[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                heightMap[x,y] = 1f + Random.Range(0f,1f);

        float[][] octaveOffsetsX = new float[octaves.Length][];
        float[][] octaveOffsetsY = new float[octaves.Length][];

        for (int i = 0; i < octaves.Length; i++)
        {
            octaveOffsetsX[i] = new float[octaves[i]];
            octaveOffsetsY[i] = new float[octaves[i]];

            for (int o = 0; o < octaveOffsetsX[i].Length; o++)
            {
                octaveOffsetsX[i][o] = Random.Range(-10000f, 10000f);
                octaveOffsetsY[i][o] = Random.Range(-10000f, 10000f);
            }
        }

        for (int i = 0; i < noiseType.Length; i++)
        {
            if (scale[i] < 0f)
                scale[i] = 1f;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    float noiseHeight = 0;
                    float frequency = 1f;
                    float amplitude = 1f;

                    for (int o = 0; o < octaveOffsetsX[i].Length; o++)
                    {
                        float sampleX = (float)(x - halfHeight) / scale[i] * frequency + octaveOffsetsX[i][o];
                        float sampleY = (float)(y - halfWidht) / scale[i] * frequency + octaveOffsetsY[i][o];

                        if (noiseType[i] == NoiseType.Simplex)
                            noiseHeight += perl.SimplexNoise(sampleX, sampleY) * amplitude;

                        if (noiseType[i] == NoiseType.Worley)
                            noiseHeight += cell.VoronoiNoise(sampleX, sampleY, .75f) * amplitude;
                        
                        if (noiseType[i] == NoiseType.Cell)
                            noiseHeight += cell.VoronoiCellNoise(sampleX, sampleY, .75f) * amplitude;
    
                        frequency *= lacunarity[i];
                        amplitude *= persistance[i];
                    }

                    noiseHeight *= magnitude[i];

                    if (applicationType[i] == NoiseAplicationType.Additive)
                        heightMap[x, y] += noiseHeight;

                    if(applicationType[i] == NoiseAplicationType.Subtractive)
                        heightMap[x, y] -= noiseHeight;

                    if(applicationType[i] == NoiseAplicationType.Multiplicative)
                        heightMap[x, y] *= noiseHeight;

                    if (fadeToRandomPoint)
                    {
                        float distance = (float)System.Math.Sqrt((x - randomPointX) * (x - randomPointX) + (y - randomPointY) * (y - randomPointY));
                        noiseHeight *= (distance / maxDistanceToRandomPoint);
                    }
                    if (fadeFromRandomPoint)
                    {
                        float distance = (float)System.Math.Sqrt((x - randomPointX) * (x - randomPointX) + (y - randomPointY) * (y - randomPointY));
                        noiseHeight *= 1f - (distance / maxDistanceToRandomPoint);
                    }
                    if (fadeToCenter)
                    {
                        float distance = (float)System.Math.Sqrt((x - halfWidht) * (x - halfWidht) + (y - halfHeight) * (y - halfHeight));
                        heightMap[x, y] *= 1f - (distance / maxDistanceToMid);
                    }
                    if (fadeFromCenter)
                    {
                        float distance = (float)System.Math.Sqrt((x - halfWidht) * (x - halfWidht) + (y - halfHeight) * (y - halfHeight));
                        noiseHeight *= (distance / maxDistanceToMid);
                    }
                    if (fadeToTop)
                    {
                        float distance = 0;// System.Math.Abs()
                        noiseHeight *= (distance / maxDistanceToRandomPoint);
                    }
                    //heightMap[x, y] = (1f + heightMap[x, y]) * (1f + heightMap[x, y]);
                    //heightMap[x, y] =heightMap[x, y] * heightMap[x, y];
                    if (heightMap[x, y] > maxValue)
                        maxValue = heightMap[x, y];
                    if (heightMap[x, y] < minValue)
                        minValue = heightMap[x, y];
                }
        }
        Debug.Log(minValue + " min");
        Debug.Log(maxValue + " max");

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                heightMap[x, y] = (heightMap[x, y] - minValue) / (maxValue - minValue);
            

        return heightMap;
    }


    public float[,] GenerateHeightMap(int width, int height, float scale, int octaves, float lacunarity, float persistance, NoiseType noiseType, float biasWorley, float biasSimplex)
    {
        float maxValue = -50000000f;
        float minValue = 50000000f;

        if (scale < 1f)
            scale = 1f;

        float halfWidht = width / 2;
        float halfHeight = height / 2;

        float[,] heightMap = new float[width, height];
        float[] octaveOffsetsX = new float[octaves];
        float[] octaveOffsetsY = new float[octaves];

        for (int i = 0; i < octaves; i++)
        {
            octaveOffsetsX[i] = Random.Range(-10000f, 10000f);
            octaveOffsetsY[i] = Random.Range(-10000f, 10000f);
        }

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                float noiseHeight = 0;
                float frequency = 1f;
                float amplitude = 1f;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (float)(x - halfHeight) / scale * frequency + octaveOffsetsX[o];
                    float sampleY = (float)(y - halfWidht) / scale * frequency + octaveOffsetsY[o];
           
                    if (noiseType == NoiseType.Simplex)
                    {
                        noiseHeight += perl.SimplexNoise(sampleX, sampleY) * amplitude;
                    }

                    if (noiseType == NoiseType.Worley)
                    {
                        noiseHeight += cell.VoronoiNoise(sampleX, sampleY, 1f) * amplitude;
                    }


                    if (noiseType == NoiseType.Cell)
                    {
                        noiseHeight = (cell.VoronoiNoise(sampleX, sampleY, 1f) * amplitude) * biasWorley + (perl.SimplexNoise(sampleX, sampleY) * amplitude) * biasSimplex;
                    }

                    frequency *= lacunarity;
                    amplitude *= persistance;
                }

                if (noiseHeight > maxValue)
                    maxValue = noiseHeight;
                if (noiseHeight < minValue)
                    minValue = noiseHeight;

                heightMap[x, y] = noiseHeight;
            }


        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                heightMap[x, y] = (heightMap[x, y] - minValue) / (maxValue - minValue);

        return heightMap;
    }

    public int[,] GenerateLayerMap(float[,] heightMap, TerrainLayer[] layer,int c)
    {
        int[,] layerMap = new int[heightMap.GetLength(0), heightMap.GetLength(1)];


        float sum = 0;

        for (int i = 0; i < layer.Length; i++)
            sum += layer[i].bias;

        for (int i = 0; i < layer.Length; i++)
            layer[i].bias = layer[i].bias / sum;

        for (int i = layer.Length; i > 1; i--)
            for (int n = 0; n < i - 1; n++)
                if (layer[n].bias > layer[n + 1].bias)
                {
                    float tmp = layer[n].bias;
                    layer[n].bias = layer[n + 1].bias;
                    layer[n + 1].bias = tmp;
                }


        for (int x = 0; x < heightMap.GetLength(0); x++)
            for (int y = 0; y < heightMap.GetLength(1); y++)
                for (int i = 0; i < layer.Length; i++)
                    if (heightMap[x, y] >= layer[i].bias)
                    {
                        layerMap[x, y] = layer[i].layerIndex;
                        break;
                    }

        return layerMap;
    }

    public float[,] GenerateLayerHeightMap(int[,] layerMap, TerrainLayer[] layer)
    {
        float[,] layerHeightMap = new float[layerMap.GetLength(0), layerMap.GetLength(1)];

        for (int x = 0; x < layerMap.GetLength(0); x++)
            for (int y = 0; y < layerMap.GetLength(1); y++)
                for (int i = 0; i < layer.Length; i++)
                    if (layerMap[x, y] == layer[i].layerIndex)
                    {
                        layerHeightMap[x, y] = layer[i].worldHeight;
                        break;
                    }

        return layerHeightMap;
    }

    public List<Vector3Int> GenerateEntryPoints(int[,] layerMap)
    {
        bool[,] checkMap = new bool[layerMap.GetLength(0), layerMap.GetLength(1)];
        List<Vector3Int> entryPoints = new List<Vector3Int>();


        for (int x = 0; x < layerMap.GetLength(0); x++)
            for (int y = 0; y < layerMap.GetLength(1); y++)
            {
                int currentLayer = -1;

                if (layerMap[x, y] != currentLayer && !checkMap[x, y])
                {
                    currentLayer = layerMap[x, y];
                    entryPoints.Add(new Vector3Int(x, y, currentLayer));
                    List<Vector2Int> openNodes = new List<Vector2Int>();
                    checkMap[x, y] = true;
                    openNodes.Add(new Vector2Int(x, y));

                    while (openNodes.Count != 0)
                    {
                        int initialPosX = openNodes[0].x;
                        int initialPosY = openNodes[0].y;

                        int posX = initialPosX;
                        int posY = initialPosY + 1;

                        if (posY < layerMap.GetLength(1))
                            if (checkMap[posX, posY] == false && layerMap[posX, posY] == currentLayer)
                            {
                                openNodes.Add(new Vector2Int(posX, posY));
                                checkMap[posX, posY] = true;
                            }

                        posX = initialPosX + 1;
                        posY = initialPosY;
                        if (posX < layerMap.GetLength(0))
                            if (checkMap[posX, posY] == false && layerMap[posX, posY] == currentLayer)
                            {
                                openNodes.Add(new Vector2Int(posX, posY));
                                checkMap[posX, posY] = true;
                            }

                        posX = initialPosX;
                        posY = initialPosY - 1;
                        if (posY >= 0)
                            if (checkMap[posX, posY] == false && layerMap[posX, posY] == currentLayer)
                            {
                                openNodes.Add(new Vector2Int(posX, posY));
                                checkMap[posX, posY] = true;
                            }

                        posX = initialPosX - 1;
                        posY = initialPosY;
                        if (posX > 0)
                            if (checkMap[posX, posY] == false && layerMap[posX, posY] == currentLayer)
                            {
                                openNodes.Add(new Vector2Int(posX, posY));
                                checkMap[posX, posY] = true;
                            }
                        openNodes.RemoveAt(0);
                    }
                }
            }

        return entryPoints;
    }

    public int[,] GenerateSurfaceLayer(float[,] heightMap, int[,] layerMap, SurfaceLayer[] surfaces, TerrainLayer[] terrainLayer)
    {
        int[,] surfaceMap = new int[heightMap.GetLength(0), heightMap.GetLength(1)];
        float[] sum = new float[terrainLayer.Length];
        
        List<float> biases = new List<float>();
        List<Vector2Int> indexes = new List<Vector2Int>();
        List<int> drawOrder = new List<int>();


        //Normalize--------------------------------------------------------------------------
        for (int t = 0; t < terrainLayer.Length; t++)
        {
            sum[t] = 0;
            for (int s = 0; s < surfaces.Length; s++)
            {
                surfaces[s].layerIndex = s;
                for (int b = 0; b < surfaces[s].bias.Length; b++)
                    if (terrainLayer[t].drawOrder == surfaces[s].terrainDrawLayer[b])
                        sum[t] += surfaces[s].bias[b];
            }
        }

        for (int t = 0; t < terrainLayer.Length; t++)
            for (int s = 0; s < surfaces.Length; s++)
                for (int b = 0; b < surfaces[s].bias.Length; b++)
                    if (terrainLayer[t].drawOrder == surfaces[s].terrainDrawLayer[b])
                        surfaces[s].bias[b] = surfaces[s].bias[b] / sum[t];
        //-----------------------------------------------------------------------------------
        
        //Sort-------------------------------------------------------------------------------
        int indexCount = 0;
        for (int s = 0; s < surfaces.Length; s++)
            for (int b = 0; b < surfaces[s].bias.Length; b++)
            {
                biases.Add(surfaces[s].bias[b]);
                indexes.Add(new Vector2Int(s, b));
                drawOrder.Add(surfaces[s].terrainDrawLayer[b]);
                indexCount++;

            }

        for (int t = 0; t < terrainLayer.Length; t++)
            for (int i = indexCount; i > 1; i--)
                for (int n = 0; n < i - 1; n++)
                {
                    if (drawOrder[n] != terrainLayer[t].drawOrder)
                        continue;
                    
                    int o;
                    bool match = false;

                    for (o = n + 1; o < i - 1; o++)
                        if (drawOrder[n] == drawOrder[o])
                        {
                            match = true;
                            break;
                        }

                    if (!match)
                        continue;

                    if (biases[n] < biases[o])
                    {
                        float tmp0 = biases[n];
                        biases[n] = biases[o];
                        biases[o] = tmp0;
                        Vector2Int tmp1 = indexes[n];
                        indexes[n] = indexes[o];
                        indexes[o] = tmp1;
                    }
                }
        //-----------------------------------------------------------------------------------

        //Apply to terrain-------------------------------------------------------------------
        for (int x = 0; x < heightMap.GetLength(0); x++)
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                float threshold = 0;
                int i = 0;
       
                for (int s = 0; s < surfaces.Length; s++)
                    for (int b = 0; b < surfaces[s].bias.Length; b++)
                    {
                        if (terrainLayer[layerMap[x, y]].drawOrder != drawOrder[i])
                        {
                            i++;
                            continue;
                        }
                        threshold += surfaces[indexes[i].x].bias[indexes[i].y];
              
                        if (threshold >= heightMap[x, y])
                        {
                            surfaceMap[x, y] = surfaces[indexes[i].x].layerIndex;
                            i += surfaces[s].bias.Length - b;
                            s = 10;
                            break;
                        }
                        else
                            i++;
                        
                    }
            }
        //-----------------------------------------------------------------------------------
        return surfaceMap;
    }

    public int[,] GenerateLayerMap(float[,] heightMap, TerrainLayer[] layer)
    {
        int[,] layerMap = new int[heightMap.GetLength(0), heightMap.GetLength(1)];
        int[] layerOrder = new int[layer.Length];
        int[] sortingLayer = new int[layer.Length];

        float sum = 0;

        for (int i = 0; i < layer.Length; i++)
        {
            layerOrder[i] = i;
            sortingLayer[i] = layer[i].drawOrder;
            sum += layer[i].bias;
        }

        for (int i = 0; i < layer.Length; i++)
            layer[i].bias = layer[i].bias / sum;


        for (int i = layer.Length; i > 1; i--)
            for (int n = 0; n < i - 1; n++)
                if (sortingLayer[n] > sortingLayer[n + 1])
                {
                    int tmp0 = sortingLayer[n];
                    sortingLayer[n] = sortingLayer[n + 1];
                    sortingLayer[n + 1] = tmp0;
                    int tmp1 = layerOrder[n];
                    layerOrder[n] = layerOrder[n + 1];
                    layerOrder[n + 1] = tmp1;
                }


        for (int x = 0; x < heightMap.GetLength(0); x++)
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                float threshold = 0;

                for (int i = 0; i < layer.Length; i++)
                {
                    threshold += layer[layerOrder[i]].bias;
                    if (heightMap[x, y] <= threshold)
                    {
                        layerMap[x, y] = layerOrder[i];
                        break;
                    }
                }
            }

        return layerMap;
    }

}
