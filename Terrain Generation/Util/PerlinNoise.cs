using System.Collections;
using System.Collections.Generic;
using System;

public class PerlinNoise
{
    private Random randomGen;

    private float scale = 60f;
    private float persistance = 0.5f;
    private float lacunarity = 2f;
    private int octaves = 2;

    private float[] octaveOffsetsX;
    private float[] octaveOffsetsY;


    private Vector2I[] gradientsSimplex2D = new Vector2I[] { new Vector2I(0, 1), new Vector2I(1, 1), new Vector2I(1, 0), new Vector2I(1, -1), new Vector2I(0, -1), new Vector2I(-1, -1), new Vector2I(-1, 0), new Vector2I(-1, 1) };
    public int[] permutatonTable = new int[] {151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33, 88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
                                77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244, 102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196, 135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123, 5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
                                223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9, 129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228, 251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107, 49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33, 88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
                                77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244, 102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196, 135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123, 5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
                                223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9, 129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228, 251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107, 49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180 };

    private readonly float consA = 0.366025403784439f;


    private readonly float consB = 0.211324865405187f;
    // A step of (1,0) in (i,j) means a step of (1-consB,-consB) in (x,y), and
    // a step of (0,1) in (i,j) means a step of (-consB,1-consB) in (x,y), where
    // consB = (3f - (float)Math.Sqrt(3)) / 6f
    private readonly float doubleConsB = 0.422649730810374f;

    public float SimplexNoise(float positionX, float positionY)
    {
        float skewValue = (positionX + positionY) * consA;//satic (0.5f * ((float)Math.Sqrt(3.0f) - 1.0f) consA
        int cellPositionX = (int)Math.Floor(positionX + skewValue);
        int cellPositionY = (int)Math.Floor(positionY + skewValue);
       
        float unskewValue = (cellPositionX + cellPositionY) * consB;// static (3f - (float)Math.Sqrt(3)) / 6f consB
        float cellOriginX = cellPositionX - unskewValue;
        float cellOriginY = cellPositionY - unskewValue;
        float offset0X = positionX - cellOriginX;
        float offset0Y = positionY - cellOriginY;

        int simplexCorner0X;
        int simplexCorner0Y;

        if (offset0X > offset0Y)
        {
            simplexCorner0X = 1;
            simplexCorner0Y = 0;
        }
        else
        {
            simplexCorner0X = 0;
            simplexCorner0Y = 1;
        }

        float offset1X = offset0X - (float)simplexCorner0X + consB;
        float offset1Y = offset0Y - (float)simplexCorner0Y + consB;
        float offset2X = offset0X - 1.0f + doubleConsB;
        float offset2Y = offset0Y - 1.0f + doubleConsB;

        int permIndexA = cellPositionX & 255;
        int permIndexB = cellPositionY & 255;

        int gradient0 = permutatonTable[permIndexA + permutatonTable[permIndexB]] % gradientsSimplex2D.Length;
        int gradient1 = permutatonTable[permIndexA + simplexCorner0X + permutatonTable[permIndexB + simplexCorner0Y]] % gradientsSimplex2D.Length;
        int gradient2 = permutatonTable[permIndexA + 1 + permutatonTable[permIndexB + 1]] % gradientsSimplex2D.Length;

        float value0, value1, value2;

        float temp = 0.5f - offset0X * offset0X - offset0Y * offset0Y;
        if (temp < 0)
            value0 = 0;
        else
        {
            temp = temp * temp;
            value0 = temp * temp * dot(gradientsSimplex2D[gradient0], offset0X, offset0Y);
        }

        temp = 0.5f - offset1X * offset1X - offset1Y * offset1Y;
        if (temp < 0)
            value1 = 0;
        else
        {
            temp = temp * temp;
            value1 = temp * temp * dot(gradientsSimplex2D[gradient1], offset1X, offset1Y);
        }

        temp = 0.5f - offset2X * offset2X - offset2Y * offset2Y;
        if (temp < 0)
            value2 = 0;
        else
        {
            temp = temp * temp;
            value2 = temp * temp * dot(gradientsSimplex2D[gradient2], offset2X, offset2Y);
        }

        return 70f * (value0 + value1 + value2);
    }


    private float dot(Vector2I gradient,float posX, float posY) 
    {
        return gradient.x * posX + gradient.y * posY;
    }



    public PerlinNoise()
    {
        randomGen = new Random(0);

        float[] octaveOffsetsX = new float[octaves];
        float[] octaveOffsetsY = new float[octaves];
        for (int i = 0; i < octaves; i++)
        {
            octaveOffsetsX[i] = (float)(randomGen.NextDouble() - .5) * 999999f;
            octaveOffsetsY[i] = (float)(randomGen.NextDouble() - .5) * 999999f;
        }


    }





    public float[,] GenerateNoiseMap(int width,int height)
    {
        float[,] map;
        float maxValue = 0f;
        float minValue = 50000000f;

        //if (scale <= 0f)
        //    scale = 0.00001f;
        //float halfWidht = map.width / 2;
        //float halfHeight = map.height / 2;

        //for (int x = 0; x < width; x++)
        //    for (int y = 0; y < height; y++)
        //    {
        //        float noiseHeight = 0;
        //        float frequency = 1f;
        //        float amplitude = 1f;

        //        for (int o = 0; o < octaves; o++)
        //        {
        //            float noisePosX = (float)(x - halfHeight) / scale * frequency + octaveOffsetsX[o];
        //            float noisePosY = (float)(y - halfWidht) / scale * frequency + octaveOffsetsY[o];
                    

        //            frequency *= lacunarity;
        //            amplitude *= persistance;
        //        }

        //        if (noiseHeight > maxValue)
        //            maxValue = noiseHeight;
        //        if (noiseHeight < minValue)
        //            minValue = noiseHeight;
        //        map.heightMap[x, y] = noiseHeight;
        //    }

        return null;
    }



    private class Vector2I 
    {
        public int x;
        public int y;

        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
