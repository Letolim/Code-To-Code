using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class BiomeGenerationData
{
    public TerrainDataGenerator.NoiseType[] noiseType;
    public TerrainDataGenerator.NoiseAplicationType[] noiseApplicationType;

    public float[] scale;
    public int[] octaves;
    public float[] lacunarity;
    public float[] persistance;
    public float[] magnitude;

    public SurfaceLayer[] layer;

    public BiomeGenerationData(){ }

    public BiomeGenerationData(TerrainDataGenerator.NoiseType[] noiseType, float[] scale, int[] octaves, float[] lacunarity, float[] persistance, float[] magnitude, SurfaceLayer[] layer)
    {
        this.noiseType = noiseType;
        this.scale = scale;
        this.octaves = octaves;
        this.lacunarity = lacunarity;
        this.persistance = persistance;
        this.magnitude = magnitude;
        this.layer = layer;
    }
}
