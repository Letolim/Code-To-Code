using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TerrainLayer
{
    public string layerName;
    public int layerIndex;
    public int drawOrder;
    public float bias;
    public float worldHeight;

    public float meshDistortion;
    public float heightDistortion;

    public TerrainLayer(string layerName, int drawOrder, float bias, float worldHeight,float meshDistortion, float heightDistortion)
    {
        this.layerName = layerName;
        this.drawOrder = drawOrder;
        this.bias = bias;
        this.meshDistortion = meshDistortion;
        this.heightDistortion = heightDistortion;
        this.worldHeight = worldHeight;

    }



}
