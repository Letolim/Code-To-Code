using System;

public class NeurodeTanh : BaseNeurode
{
    public NeurodeTanh(int upperLayerLength, System.Random rnd)
    {
        Weight = new float[upperLayerLength];

        for (int i = 0; i < Weight.Length; i++)
            Weight[i] = GetRandomFloat(rnd);

        Bias = 0;
    }

    public override void Forward(BaseNeurode[] neurodes)//Done
    {
        Activation = 0f;

            for (int n = 0; n < neurodes.Length; n++)
                Activation += neurodes[n].Activation * Weight[n];

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);
    }

    public override void Forward(BaseNeurode[][][][] neurodes)//Done
    {
        int weightIndex = 0;

        for (int sn = 0; sn < neurodes.Length; sn++)
            for (int nt = 0; nt < neurodes[sn].Length; nt++)
                    for (int n = 0; n < neurodes[sn][nt][neurodes[sn][nt].Length - 1].Length; n++)
                    {           // += each n inside each last layer inside cs(neurodes)
                        Activation += neurodes[sn][nt]  [neurodes[sn][nt].Length - 1]  [n].Activation * Weight[weightIndex];
                        weightIndex++;
                    }

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);
    }

    public override void Forward(BaseNeurode[][][] neurodes, int currentLayer)//Done
    {
        int weightIndex = 0;

        for (int nt = 0; nt < neurodes.Length; nt++)
            if (currentLayer < neurodes[nt].Length)
                for (int n = 0; n < neurodes[nt][currentLayer - 1].Length; n++)
                {           // += each n inside each layer -1 in each nt
                    Activation += neurodes[nt][currentLayer - 1][n].Activation * Weight[weightIndex];
                    weightIndex++;
                }
        
        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);
    }

    public override void Reset(Random rnd)//Done
    {
        for (int i = 0; i < Weight.Length; i++)
            Weight[i] = GetRandomFloat(rnd);

        Bias = GetRandomFloat(rnd);
        Activation = 0f;
    }

    public override void ResetActivation()//Done
    {
        Activation = 0f;
    }
}