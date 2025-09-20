using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDataMap 
{
    public int width;
    public int height;

    public float[,] heightMap;
    public float heightMapMaxValue;
    public float heightMapMinValue;


    public float[,] layerHeightMap;
    public int[,] layerMap; 
    public int[,] surfaceMap;

    public List<bool[,]> layers;

    public TerrainMapHeightLayer[] terrainLayer;
    public SurfaceLayer[] surfaceLayer;

    public TerrainDataMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        layers = new List<bool[,]>();
        heightMap = new float[width, height];
        layerHeightMap = new float[width, height];
        layerMap = new int[width, height];
    }
}
