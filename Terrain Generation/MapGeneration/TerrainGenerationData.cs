using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TerrainGenerationData 
{
    public TerrainDataGenerator.NoiseType noiseType;
    public float scale;
    public int octaves;
    public float lacunarity;
    public float persistance;
    public float worleyBias;
    public float simplexBias;

    public bool fadeToCenter;
    public bool fadeFromCenter;
    public bool fadeToRandomPoint;
    public bool fadeFromRandomPoint;

    public bool fadeFromTop;
    public bool fadeToTop;
    public bool fadeFromBottom;
    public bool fadeToBottom;

    public bool fadeFromLeft;
    public bool fadeToLeft;
    public bool fadeFromRight;
    public bool fadeToRight;

    public TerrainLayer[] layer;

    public TerrainGenerationData()
    { }

    public TerrainGenerationData(TerrainDataGenerator.NoiseType noiseType, float scale, int octaves, float lacunarity, float persistance, float worleyBias, float simplexBias, bool fadeToCenter, bool fadeFromCenter, bool fadeToRandomPoint, bool fadeFromRandomPoint, bool fadeFromTop, bool fadeToTop, bool fadeFromBottom, bool fadeToBottom, bool fadeFromLeft, bool fadeToLeft, bool fadeFromRight, bool fadeToRight, TerrainLayer[] layer)
    {
        this.noiseType = noiseType;
        this.scale = scale;
        this.octaves = octaves;
        this.lacunarity = lacunarity;
        this.persistance = persistance;
        this.worleyBias = worleyBias;
        this.simplexBias = simplexBias;
        this.fadeToCenter = fadeToCenter;
        this.fadeFromCenter = fadeFromCenter;
        this.fadeToRandomPoint = fadeToRandomPoint;
        this.fadeFromRandomPoint = fadeFromRandomPoint;
        this.fadeFromTop = fadeFromTop;
        this.fadeToTop = fadeToTop;
        this.fadeFromBottom = fadeFromBottom;
        this.fadeToBottom = fadeToBottom;
        this.fadeFromLeft = fadeFromLeft;
        this.fadeToLeft = fadeToLeft;
        this.fadeFromRight = fadeFromRight;
        this.fadeToRight = fadeToRight;
        this.layer = layer;
    }
}
