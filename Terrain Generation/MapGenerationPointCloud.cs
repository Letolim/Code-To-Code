using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationPointCloud
{
    public float avaragePointDistance = .5f;
    public List<List<Vector2>> borderPoints;
    public List<List<Vector3>> vertices;

    public List<int> layerIndex;

    public MapGenerationPointCloud(int layerCount, float maxPointDistanceFromCenter) 
    {
        this.avaragePointDistance = maxPointDistanceFromCenter;

        borderPoints = new List<List<Vector2>>();
        vertices = new List<List<Vector3>>();
        layerIndex = new List<int>();

        //for (int i = 0; i < layerCount; i++)
        //{
            //borderPoints[i] = new List<Vector2>();
            //vertices[i] = new List<Vector3>();
        //}

    }


}
