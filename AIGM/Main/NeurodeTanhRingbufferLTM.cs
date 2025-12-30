using System;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NeurodeTanhRingbufferLTM : BaseNeurode
{
    private const int BufferSize = 5;
    private readonly float[] activationArray;
    private int currentIndex = 0;

    private readonly int interval = 30;
    private int iteration = 0;

    public NeurodeTanhRingbufferLTM(int upperLayerLength, int seed, System.Random rnd)
    {
        Weight = new float[upperLayerLength];
        for (int i = 0; i < Weight.Length; i++)
            Weight[i] = GetRandomFloat(rnd) * .2f;

        iteration = Math.Abs(HashCode.Combine(upperLayerLength, seed) % interval);

        Bias = 0;

        activationArray = new float[BufferSize];
    }

    public override void Forward(BaseNeurode[] neurodes)//Done
    {
        if (iteration != 0)
        {
            iteration--;
            return;
        }

        Activation = 0f;

        for (int n = 0; n < neurodes.Length; n++)
            Activation += neurodes[n].Activation * Weight[n];

        activationArray[currentIndex] = activationArray[0];
        activationArray[0] = (float)Math.Tanh(Activation + Bias);

        Activation = 0f;

        for (int i = 0; i < activationArray.Length; i++)
            Activation += (activationArray[i] * activationArray[i]) * activationArray[i];

        Activation = Activation / BufferSize;

        Activation = ClampActivation(Activation, BaseNeurode.ClampThreshold);


        currentIndex++;
        if (currentIndex == activationArray.Length)
            currentIndex = 1;

        iteration = interval;
    }

    public override void Forward(BaseNeurode[][][][] neurodes)//Done
    {
        if (iteration != 0)
        {
            iteration--;
            return;
        }

        int weightIndex = 0;

        Activation = 0f;

        for (int sn = 0; sn < neurodes.Length; sn++)
            for (int nt = 0; nt < neurodes[sn].Length; nt++)
                for (int n = 0; n < neurodes[sn][nt][neurodes[sn][nt].Length - 1].Length; n++)
                {           // += each n inside each last layer inside cs(neurodes)
                    Activation += neurodes[sn][nt][neurodes[sn][nt].Length - 1][n].Activation * Weight[weightIndex];
                    weightIndex++;
                }

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);

        activationArray[currentIndex] = activationArray[0];
        activationArray[0] = (float)Math.Tanh(Activation + Bias);

        Activation = 0f;

        for (int i = 0; i < activationArray.Length; i++)
            Activation += (activationArray[i] * activationArray[i]) * activationArray[i];

        Activation = Activation / BufferSize;

        Activation = ClampActivation(Activation, BaseNeurode.ClampThreshold);

        currentIndex++;
        if (currentIndex == activationArray.Length)
            currentIndex = 1;

        iteration = interval;

        weightIndex = 0;

        for (int sn = 0; sn < neurodes.Length; sn++)
            for (int nt = 0; nt < neurodes[sn].Length; nt++)
                for (int n = 0; n < neurodes[sn][nt][neurodes[sn][nt].Length - 1].Length; n++)
                {           // += each n inside each last layer inside cs(neurodes)
                    Activation += neurodes[sn][nt][neurodes[sn][nt].Length - 1][n].Activation * Weight[weightIndex];
                    weightIndex++;
                }

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);

        if (Activation < ClampThreshold && Activation > -ClampThreshold)
            Activation = 0f;
    }

    public override void Forward(BaseNeurode[][][] neurodes, int currentLayer)//Done
    {
        if (iteration != 0)
        {
            iteration--;
            return;
        }

        int weightIndex = 0;

        Activation = 0f;

        for (int nt = 0; nt < neurodes.Length; nt++)
            if (currentLayer < neurodes[nt].Length)
                for (int n = 0; n < neurodes[nt][currentLayer - 1].Length; n++)
                {           // += each n inside each layer -1 in each nt
                    Activation += neurodes[nt][currentLayer - 1][n].Activation * Weight[weightIndex];
                    weightIndex++;
                }

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);

        activationArray[currentIndex] = activationArray[0];
        activationArray[0] = (float)Math.Tanh(Activation + Bias);

        Activation = 0f;

        for (int i = 0; i < activationArray.Length; i++)
            Activation += (activationArray[i] * activationArray[i]) * activationArray[i];

        Activation = Activation / BufferSize;

        Activation = ClampActivation(Activation, BaseNeurode.ClampThreshold);

        currentIndex++;
        if (currentIndex == activationArray.Length)
            currentIndex = 1;

        iteration = interval;

        weightIndex = 0;

        for (int nt = 0; nt < neurodes.Length; nt++)
            if (currentLayer < neurodes[nt].Length)
                for (int n = 0; n < neurodes[nt][currentLayer - 1].Length; n++)
                {           // += each n inside each layer -1 in each nt
                    Activation += neurodes[nt][currentLayer - 1][n].Activation * Weight[weightIndex];
                    weightIndex++;
                }

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);

        if (Activation < ClampThreshold && Activation > -ClampThreshold)
            Activation = 0f;
    }

    public override void Reset(Random rnd)
    {

        for (int i = 0; i < Weight.Length; i++)
            Weight[i] = GetRandomFloat(rnd) * .2f;

        for (int i = 0; i < activationArray.Length; i++)
            activationArray[i] = 0f;
        
        Bias = GetRandomFloat(rnd) * .2f;
        Activation = 0f;
        iteration = 0;
    }

    public override void ResetActivation()//Done
    {
        for (int i = 0; i < activationArray.Length; i++)
            activationArray[i] = 0f;

        Activation = 0f;
    }
}