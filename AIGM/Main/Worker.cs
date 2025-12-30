using System.Collections.Generic;
using static Helper;

public class Worker
{
    public NeuralNetwork Network { get { return _neuralNetwork; } private set { _neuralNetwork = value; } }
    public float Fitness { get { return _fitness; } set { _fitness = value; } }

    private NeuralNetwork _neuralNetwork;
    private float _fitness;

    public Worker(List<IntVector2[][][]> networkLayout, System.Random rnd)
    {
        _neuralNetwork = new NeuralNetwork(networkLayout, rnd);
    }

    public float[] Forward(float[][] input) { return _neuralNetwork.Forward(input); }
    public float[] Forward(float[] input) { return _neuralNetwork.Forward(input); }
}