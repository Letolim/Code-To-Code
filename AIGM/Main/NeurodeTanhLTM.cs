using System;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NeurodeTanhLTM : BaseNeurode
{
    private float _activationValue = 0f;

    public NeurodeTanhLTM(int upperLayerLength, System.Random rnd)
    {
        Weight = new float[upperLayerLength * 2];

        for (int i = 0; i < Weight.Length; i++)
            Weight[i] = GetRandomFloat(rnd);

        Bias = 0;
    }

    public override void Forward(BaseNeurode[] neurodes)//Done
    {
        int weightIndex = 0;

        for (int n = 0; n < neurodes.Length; n++)
        {
            Activation += neurodes[n].Activation * Weight[weightIndex];
            weightIndex++;
        }

        Activation = ClampActivation((float)Math.Tanh(Activation), BaseNeurode.ClampThreshold);

        if (Math.Abs(Activation) > BaseNeurode.ClampThreshold)
        {
            for (int n = neurodes.Length; n < neurodes.Length; n++)
            {
                _activationValue += neurodes[n].Activation * Weight[weightIndex];
                weightIndex++;
            }

            _activationValue = ClampActivation((float)Math.Tanh(_activationValue), BaseNeurode.ClampThreshold);
        }

        Activation = _activationValue;
    }

    public override void Forward(BaseNeurode[][][][] neurodes)//Done
    {
        int weightIndex = 0;

        for (int sn = 0; sn < neurodes.Length; sn++)
            for (int nt = 0; nt < neurodes[sn].Length; nt++)
                for (int n = 0; n < neurodes[sn][nt][neurodes[sn][nt].Length - 1].Length; n++)
                {           // += each n inside each last layer inside cs(neurodes)
                    Activation += neurodes[sn][nt][neurodes[sn][nt].Length - 1][n].Activation * Weight[weightIndex];
                    weightIndex++;
                }

        Activation = ClampActivation((float)Math.Tanh(Activation + Bias), BaseNeurode.ClampThreshold);

        if (Math.Abs(Activation) > BaseNeurode.ClampThreshold)
        {
            for (int sn = 0; sn < neurodes.Length; sn++)
                for (int nt = 0; nt < neurodes[sn].Length; nt++)
                    for (int n = 0; n < neurodes[sn][nt][neurodes[sn][nt].Length - 1].Length; n++)
                    {           // += each n inside each last layer inside cs(neurodes)
                        _activationValue += neurodes[sn][nt][neurodes[sn][nt].Length - 1][n].Activation * Weight[weightIndex];
                        weightIndex++;
                    }

            _activationValue = ClampActivation((float)Math.Tanh(_activationValue + Bias), BaseNeurode.ClampThreshold); ;
        }

        Activation = _activationValue;
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

        if (Math.Abs(Activation) > BaseNeurode.ClampThreshold)
        {
            for (int nt = 0; nt < neurodes.Length; nt++)
                if (currentLayer < neurodes[nt].Length)
                    for (int n = 0; n < neurodes[nt][currentLayer - 1].Length; n++)
                    {           // += each n inside each layer -1 in each nt
                        _activationValue += neurodes[nt][currentLayer - 1][n].Activation * Weight[weightIndex];
                        weightIndex++;
                    }

            _activationValue = ClampActivation((float)Math.Tanh(_activationValue + Bias), BaseNeurode.ClampThreshold); ;
        }

        Activation = _activationValue;
    }

    public override void Reset(Random rnd)
    {
        for (int i = 0; i < Weight.Length; i++)
            Weight[i] = GetRandomFloat(rnd);

        Bias = GetRandomFloat(rnd);

        _activationValue = 0f;
        Activation = 0f;
    }

    public override void ResetActivation()//Done
    {
        Activation = 0f;
    }
}
