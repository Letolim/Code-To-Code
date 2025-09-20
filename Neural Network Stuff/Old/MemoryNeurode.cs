using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryNeurode : Neurode
{
    private int layer;
    private float delta;
    private float[] bias;
    private float[] weight;
    private int startIndexSecondGate;
    private NeurodeType type;


    public MemoryNeurode(int layer, float delta, float[] bias, float[] weight)
    {
        Layer = layer;
        Delta = delta;
        Bias = bias;
        Weight = weight;
    }

    public MemoryNeurode(int layer, int parentLayerSize, System.Random randomGen, NeurodeType type)
    {
        this.startIndexSecondGate = parentLayerSize;
        parentLayerSize = parentLayerSize * 2;
        this.layer = layer;
        this.delta = 0;
        this.bias = new float[parentLayerSize];
        this.weight = new float[parentLayerSize];
        this.type = type;

        for (int i = 0; i < parentLayerSize; i++)
        {
            bias[i] = ((float)randomGen.NextDouble() - .5f) * 2f;
            weight[i] = ((float)randomGen.NextDouble() - .5f) *2f;
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

        for (int i = 0; i < startIndexSecondGate; i++)
                    activationValue += parentLayer[i].Delta * weight[i] * bias[i];

        if (System.Math.Tanh(activationValue) > 0)
        {
            activationValue = 0;

            for (int i = 0; i < startIndexSecondGate; i++)
            {
                int index = i + startIndexSecondGate;
                activationValue += parentLayer[i].Delta * weight[index] * bias[index];

            }
            delta = (float)System.Math.Tanh(activationValue);

        }

    }

    public override void RunForward(Neurode[][] network)
    {
        float activationValue = 0;

        for (int i = 0; i < network[layer - 1].Length; i++)
            activationValue += network[layer - 1][i].Delta * weight[i] * bias[i];

        //delta = (float)System.Math.Tanh((double)activationValue);
        delta = System.Math.Max(0, activationValue);
    }

    public override Neurode Merge(Neurode neurodeA, Neurode neurodeB, MergeType type)
    {
        throw new System.NotImplementedException();
    }

    public override void MergeInToThis(Neurode partnerNeurode, MergeType type)
    {
        delta = 0;

        if(type == MergeType.Merge)
            for (int i = 0; i < partnerNeurode.Weight.Length; i++)
            {
                bias[i] = (partnerNeurode.Bias[i] + bias[i]) / 2f;
                weight[i] = (partnerNeurode.Weight[i] + weight[i]) / 2f;
                if (bias[i] > 1f)
                    bias[i] = 1f;
                else if (bias[i] < -1f)
                    bias[i] = -1f;

                if (weight[i] > 1f)
                    weight[i] = 1f;
                else if (weight[i] < -1f)
                    weight[i] = -1f;
            }

        if (type == MergeType.Schuffle)
            for (int i = 0; i < partnerNeurode.Weight.Length; i++)
                if (i % 2 == 0)                //dont always schuffle the same
                {
                    bias[i] = (partnerNeurode.Bias[i] + bias[i]) / 2f;
                    weight[i] = (partnerNeurode.Weight[i] + weight[i]) / 2f;
                    if (bias[i] > 1f)
                        bias[i] = 1f;
                    else if (bias[i] < -1f)
                        bias[i] = -1f;

                    if (weight[i] > 1f)
                        weight[i] = 1f;
                    else if (weight[i] < -1f)
                        weight[i] = -1f;
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
                else if (bias[i] < -1f)
                    bias[i] = -1f;

                if (weight[i] > 1f)
                    weight[i] = 1f;
                else if (weight[i] < -1f)
                    weight[i] = -1f;
            }
        }



    }

    public override void Respawn(System.Random randomGen)
    {
        for (int i = 0; i < bias.Length; i++)
        {
            bias[i] = (float)randomGen.NextDouble() - .5f;
            weight[i] = (float)randomGen.NextDouble() - .5f;
        }

    }

    public override void AlterNeurode(Neurode baseNeurode, float alternation, System.Random randomGen)
    {
        float magnitude = (float)(.2d + randomGen.NextDouble()) * alternation;
        for(int i = 0; i < bias.Length; i ++)
        {
            if (randomGen.NextDouble() > alternation)
            {
                bias[i] = baseNeurode.Bias[i];
                weight[i] = baseNeurode.Weight[i];
                if (bias[i] > 1f)
                    bias[i] = 1f;
                else if (bias[i] < -1f)
                    bias[i] = -1f;

                if (weight[i] > 1f)
                    weight[i] = 1f;
                else if (weight[i] < -1f)
                    weight[i] = -1f;
                continue;
            }
            bias[i] = baseNeurode.Bias[i] + (.5f - (float)randomGen.NextDouble()) * magnitude;
            weight[i] = baseNeurode.Weight[i] + (.5f - (float)randomGen.NextDouble()) * magnitude;
            if (bias[i] > 1f)
                bias[i] = 1f;
            else if (bias[i] < -1f)
                bias[i] = -1f;

            if (weight[i] > 1f)
                weight[i] = 1f;
            else if (weight[i] < -1f)
                weight[i] = -1f;
        }    
    }
}