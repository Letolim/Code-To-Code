using System;

public abstract class BaseNeurode
{
    public float Activation { get; set; }
    public float[] Weight;
    public float Bias;

    public BaseNeurode() { }

    public abstract void Forward(BaseNeurode[] neurodes);
    public abstract void Forward(BaseNeurode[][][][] neurodes);
    public abstract void Forward(BaseNeurode[][][] neurodes, int currentLayer);

    public static BaseNeurode InitNeurodeOfType(int type, int parameters, System.Random rnd, int seed)
    {
        if (type == 0) return new NeurodeTanh(parameters, rnd);
        if (type == 1) return new NeurodeTanhRingbuffer(parameters, rnd);
        if (type == 2) return new NeurodeTanhRingbufferLTM(parameters, seed, rnd);
        if (type == 3) return new NeurodeTanhLTM(parameters, rnd);
        if (type == 4) return new NeurodeTanhNBS(parameters, rnd);

        return null;
    }

    public abstract void Reset(System.Random rnd);
    public abstract void ResetActivation();

    public static float ClampActivation(float value, float clampThreshold)
    {
        if (value < clampThreshold && value > -clampThreshold)
            return 0;
        else
            return value;
    }

    public float GetRandomFloat(System.Random rnd)
    {
        return ((float)rnd.NextDouble() - 0.5f) * 1.75f;
    }

    public static float ClampThreshold = 0.01f;
}