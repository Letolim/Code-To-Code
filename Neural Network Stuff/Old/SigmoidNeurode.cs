using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmoidNeurode : Neurode
{
    private int layer;
    private float delta;
    private float[] bias;
    private float[] weight;
    private NeurodeType type;

    public SigmoidNeurode(int layer, float delta, float[] bias, float[] weight)
    {
        Layer = layer;
        Delta = delta;
        Bias = bias;
        Weight = weight;
    }

    public SigmoidNeurode(int layer, int parentLayerSize, System.Random randomGen, NeurodeType type)
    {
        this.layer = layer;
        this.delta = 0;
        this.bias = new float[parentLayerSize];
        this.weight = new float[parentLayerSize];
        this.type = type;

        for (int i = 0; i < parentLayerSize; i++)
        {
            bias[i] = (float)randomGen.NextDouble() * .5f;
            weight[i] = (float)randomGen.NextDouble() * .5f;
        }
    }

    public override int Layer { get { return layer; } set { layer = value; } }
    public override float Delta { get { return delta; } set { delta = value; } }
    public override float[] Bias { get { return bias; } set { bias = value; } }
    public override float[] Weight { get { return weight; } set { weight = value; } }
    public override NeurodeType Type { get { return type; } set { type = value; } }

    public override void RunForward(Neurode[] parentLayer)
    {
        float activationValue = 0;

        for (int i = 0; i < parentLayer.Length; i++)
            activationValue += parentLayer[i].Delta * Weight[i] * bias[i];

        activationValue = (float)System.Math.Exp(activationValue);
        delta = activationValue / (1f + activationValue);
    }

    public override void RunForward(Neurode[][] network)
    {
        float activationValue = 0;

        for (int i = 0; i < network[layer - 1].Length; i++)
            activationValue += network[layer - 1][i].Delta * Weight[i] * bias[i];

        //delta = (float)System.Math.Tanh((double)activationValue);
        delta = activationValue / (1f + activationValue);

    }

    public override Neurode Merge(Neurode neurodeA, Neurode neurodeB, MergeType type)
    {
        throw new System.NotImplementedException();
    }

    public override void MergeInToThis(Neurode partnerNeurode, MergeType type)
    {
        delta = 0;

        if (type == MergeType.Merge)
            for (int i = 0; i < partnerNeurode.Weight.Length; i++)
            {
                bias[i] = (partnerNeurode.Bias[i] + bias[i]) / 2f;
                weight[i] = (partnerNeurode.Weight[i] + weight[i]) / 2f;

                if (bias[i] > 1f)
                    bias[i] = 1f;
                else if (bias[i] < 0f)
                    bias[i] = 0f;

                if (weight[i] > 1f)
                    weight[i] = 1f;
                else if (weight[i] < 0f)
                    weight[i] = 0f;

            }

        if (type == MergeType.Schuffle)
            for (int i = 0; i < partnerNeurode.Weight.Length; i++)
                if (i % 2 == 0)                //dont always schuffle the same
                {
                    bias[i] = (partnerNeurode.Bias[i] + bias[i]) / 2f;
                    weight[i] = (partnerNeurode.Weight[i] + weight[i]) / 2f;

                    if (bias[i] > 1f)
                        bias[i] = 1f;
                    else if (bias[i] < 0f)
                        bias[i] = 0f;

                    if (weight[i] > 1f)
                        weight[i] = 1f;
                    else if (weight[i] < 0f)
                        weight[i] = 0f;
                }

        if (type == MergeType.Lerp)
        {
            float t = .75f;
            for (int i = 0; i < partnerNeurode.Weight.Length; i++)
            {

                bias[i] = bias[i] + t * (partnerNeurode.Bias[i] - bias[i]);
                weight[i] = weight[i] + t * (partnerNeurode.Weight[i] - weight[i]);

                if (bias[i] > 1f)
                    bias[i] = 1f;
                else if (bias[i] < 0f)
                    bias[i] = 0f;

                if (weight[i] > 1f)
                    weight[i] = 1f;
                else if (weight[i] < 0f)
                    weight[i] = 0f;
            }
        }

        

    }

    public override void Respawn(System.Random randomGen)
    {
        for (int i = 0; i < bias.Length; i++)
        {
            bias[i] = (float)randomGen.NextDouble() * .5f;
            weight[i] = (float)randomGen.NextDouble() * .5f;
        }

    }

    public override void AlterNeurode(Neurode baseNeurode, float alternation, System.Random randomGen)
    {
        float magnitude = (float)(.2d + randomGen.NextDouble()) * alternation;
        for (int i = 0; i < bias.Length; i++)
        {
            if (randomGen.NextDouble() > alternation)
            {
                bias[i] = baseNeurode.Bias[i];
                weight[i] = baseNeurode.Weight[i];
                if (bias[i] > 1f)
                    bias[i] = 1f;
                else if (bias[i] < 0f)
                    bias[i] = 0f;

                if (weight[i] > 1f)
                    weight[i] = 1f;
                else if (weight[i] < 0f)
                    weight[i] = 0f;
                continue;
            }
            bias[i] = baseNeurode.Bias[i] + (.5f - (float)randomGen.NextDouble()) * magnitude;
            weight[i] = baseNeurode.Weight[i] + (.5f - (float)randomGen.NextDouble()) * magnitude;
            if (bias[i] > 1f)
                bias[i] = 1f;
            else if (bias[i] < 0f)
                bias[i] = 0f;

            if (weight[i] > 1f)
                weight[i] = 1f;
            else if (weight[i] < 0f)
                weight[i] = 0f;
        }
    }
}