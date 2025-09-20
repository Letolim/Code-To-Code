using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTools
{
    private static System.Random randomGen;

    static MathTools()
    {
        randomGen = new System.Random(System.DateTime.Now.Millisecond);
    }

    public static float RandomDoubleToFloatRange(float min, float max, double randomNumber)
    {
        if (min <= 0)
            return (float)randomNumber * (System.Math.Abs(min) + System.Math.Abs(max)) - System.Math.Abs(min);
        else
            return max - (float)randomNumber * (System.Math.Abs(max) - System.Math.Abs(min));

    }

    public static float RandomRange(float min, float max)
    {
        if(min <= 0)
            return (float)randomGen.NextDouble() * (System.Math.Abs(min) + System.Math.Abs(max)) - System.Math.Abs(min);
        else
            return max - (float)randomGen.NextDouble() * (System.Math.Abs(max) - System.Math.Abs(min));


    }

    public static int RandomRange(int min, int max)
    {
        return randomGen.Next(min,max);
    }
}
