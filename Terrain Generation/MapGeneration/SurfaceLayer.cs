using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class SurfaceLayer
{
    public string surfaceName;
    public int layerIndex;
    //public float bias;
    public float[] bias;
    public int[] terrainDrawLayer;
    public string[] texturePath;
   

    public SurfaceLayer(string surfaceName, int[] terrainDrawLayer, float[] bias, string[] texturePath)
    {
        this.surfaceName = surfaceName;
        this.bias = bias;
        this.terrainDrawLayer = terrainDrawLayer;
        this.texturePath = texturePath;
    }
}
