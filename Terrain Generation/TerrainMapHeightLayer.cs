using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainMapHeightLayer 
{
    public string layerName;
    public int layerIndex;
    public float threshold;
    public float height;
    public Color color;

    public float pointCloudProbabillity;
    public float pointCloudNoiseHeight;


    public TerrainMapHeightLayer(string layerName, int layerIndex, float threshold, float height)
    {
        this.layerName = layerName;
        this.layerIndex = layerIndex;
        this.threshold = threshold;
        this.height = height;
    }

    public TerrainMapHeightLayer(string layerName, int layerIndex, float threshold, float height,Color color)
    {
        this.layerName = layerName;
        this.layerIndex = layerIndex;
        this.threshold = threshold;
        this.height = height;
        this.color = color;
    }
    public TerrainMapHeightLayer(string layerName, int layerIndex, float threshold, float height, Color color, float pointCloudProbabillity, float pointCloudNoiseHeight)
    {
        this.layerName = layerName;
        this.layerIndex = layerIndex;
        this.threshold = threshold;
        this.height = height;
        this.color = color;
        this.pointCloudProbabillity = pointCloudProbabillity;
        this.pointCloudNoiseHeight = pointCloudNoiseHeight;
    }
}
