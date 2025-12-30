using System;
using System.Collections.Generic;
using System.IO;
using static Helper;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

public class NetworkManager
{
    public int TotalNetworkcount { get { return _totalNetworkcount; } private set { _totalNetworkcount = value; } }

    private int _totalNetworkcount;
    private OuterParameters[][] _parameters;
    private System.Random rnd;
    private float _alternationValue = .5f;

    public NetworkManager(OuterParameters[][] parameters, int seed)
    {
        _parameters = parameters;
        rnd = new System.Random(seed);

        for (int i = 0; i < _parameters.Length; i++)
            for (int n = 0; n < _parameters[i].Length; n++)
                _totalNetworkcount += _parameters[i][n].NetworkObjects.Count;
    }

    public NetworkManager(int networkCount, int seed)
    {
        rnd = new System.Random(seed);
        _totalNetworkcount = networkCount;
    }


    public static int[] ComputeTopNetworks(List<NeuralNetwork> networks, int topNetworkCount)//done
    {
        int[] topNetworks = new int[topNetworkCount];

        float[] bestFitness = new float[topNetworks.Length];

        for (int i = 0; i < topNetworks.Length; i++)
            bestFitness[i] = float.NegativeInfinity;

        for (int k = 0; k < networks.Count; k++)
        {
            float fitness = networks[k].Fitness;

            if (fitness <= bestFitness[topNetworks.Length - 1])
                continue;

            int insertPos = topNetworks.Length - 1;
            while (insertPos > 0 && fitness > bestFitness[insertPos - 1])
                insertPos--;

            for (int j = topNetworks.Length - 1; j > insertPos; j--)
            {
                bestFitness[j] = bestFitness[j - 1];
                topNetworks[j] = topNetworks[j - 1];
            }

            bestFitness[insertPos] = fitness;
            topNetworks[insertPos] = k;
        }

        return topNetworks;
    }

    public static IntVector3[] ComputeTopNetworks(OuterParameters[][] parameters, int topNetworkCount)//done
    {
        IntVector3[] topNetworks = new IntVector3[topNetworkCount];

        float[] bestFitness = new float[topNetworks.Length];

        for (int i = 0; i < topNetworks.Length; i++)
            bestFitness[i] = float.NegativeInfinity;

        for (int i = 0; i < parameters.Length; i++)
            for (int n = 0; n < parameters[i].Length; n++)
                for (int k = 0; k < parameters[i][n].NetworkObjects.Count; k++)
                {
                    float fitness = parameters[i][n].NetworkObjects[k].NetworkWorker.Fitness;

                    if (fitness <= bestFitness[topNetworks.Length - 1])
                        continue;

                    int insertPos = topNetworks.Length - 1;
                    while (insertPos > 0 && fitness > bestFitness[insertPos - 1])
                        insertPos--;

                    for (int j = topNetworks.Length - 1; j > insertPos; j--)
                    {
                        bestFitness[j] = bestFitness[j - 1];
                        topNetworks[j] = topNetworks[j - 1];
                    }

                    bestFitness[insertPos] = fitness;
                    topNetworks[insertPos] = new IntVector3(i, n, k);
                }
        return topNetworks;
    }


    public void CombineNetworks(List<NeuralNetwork> networks, int topNetworkCount)//done
    {
        int[] topNetworks = ComputeTopNetworks(networks, topNetworkCount);
        int[] lookup = Helper.CreateRandomIntArray(topNetworkCount, rnd);

        int combineTypes = 17;
        int combineType = rnd.Next(0, combineTypes);

        int topNetworkIndex = rnd.Next(-1, topNetworkCount -2);
        bool isTopNetwork = false;

        for (int k = 0; k < networks.Count; k++)
        {
            topNetworkIndex++;

            if (topNetworkIndex == topNetworkCount)
            {
                lookup = Helper.CreateRandomIntArray(topNetworkCount, rnd);
                topNetworkIndex = 0;
            }

            for (int i = 0; i < topNetworkCount; i++)
                if (k == topNetworks[i])
                {
                    isTopNetwork = true;
                    break;
                }

            if (isTopNetwork)
            {
                isTopNetwork = false;
                continue;
            }

            switch (combineType)
            {
                case 0:
                    MergeWithMean(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 1:
                    SchuffleEachValue(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 2:
                    ReplaceRandomCluster(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 3:
                    ReplaceRandomSubcluster(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 4:
                    ReplaceRandomNetwork(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 5:
                    ReplaceRandomLayer(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 6:
                    ReplaceRandomNeurode(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]]);
                    break;
                case 7:
                    AlterEachValue(networks[k], networks[topNetworks[topNetworkIndex]], _alternationValue);
                    break;
                case 8:
                    AlterEachValue(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]], _alternationValue);
                    break;
                case 9:
                    AlterEachValueAdditive(networks[k], networks[topNetworks[topNetworkIndex]], _alternationValue);
                    break;
                case 10:
                    AlterEachValueAdditive(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]], _alternationValue);
                    break;
                case 11:
                    AlterEachValueMultiplicative(networks[k], networks[topNetworks[topNetworkIndex]], _alternationValue);
                    break;
                case 12:
                    AlterEachValueMultiplicative(networks[k], networks[topNetworks[topNetworkIndex]], networks[topNetworks[lookup[topNetworkIndex]]], _alternationValue);
                    break;
                case 13:
                    AlterOneValue(networks[k], networks[topNetworks[topNetworkIndex]], _alternationValue);
                    break;
                case 14:
                    AlterOneValueAdditive(networks[k], networks[topNetworks[topNetworkIndex]], _alternationValue);
                    break;
                case 15:
                    AlterOneValueMultiplicative(networks[k], networks[topNetworks[topNetworkIndex]], _alternationValue);
                    break;
                case 16:
                    networks[k].Reset(rnd);
                break;
            }

            combineType++;

            if (combineType == combineTypes)
                combineType = 0;

            _alternationValue -= .01f;
            if (_alternationValue < 0)
                _alternationValue = .5f;
        }
    }

    public void CombineNetworks(OuterParameters[][] parameters, int topNetworkCount)//done
    {
        IntVector3[] topNetworks = ComputeTopNetworks(parameters, topNetworkCount);

        int[] lookup = Helper.CreateRandomIntArray(topNetworkCount, rnd);
        var topSet = new HashSet<(int x, int y, int z)>();
        foreach (var v in topNetworks) 
            topSet.Add((v.X, v.Y, v.Z));

        int combineTypes = 17;
        int combineType = rnd.Next(0, combineTypes);

        int topNetworkIndex = rnd.Next(-1, topNetworkCount - 2);
        bool isTopNetwork = false;

        for (int i = 0; i < parameters.Length; i++)
            for (int n = 0; n < parameters[i].Length; n++)
                for (int k = 0; k < parameters[i][n].NetworkObjects.Count; k++)
                {
                    topNetworkIndex++;

                    if (topNetworkIndex == topNetworkCount)
                    {
                        lookup = Helper.CreateRandomIntArray(topSet.Count, rnd);
                        topNetworkIndex = 0;
                    }

                    for (int o = 0; o < topNetworkCount; o++)
                        if (topNetworks[o].X == i && topNetworks[o].Y == n && topNetworks[o].Z == k)
                        {
                            isTopNetwork = true;
                            break;
                        }

                    if (isTopNetwork)
                    {
                        isTopNetwork = false;
                        continue;
                    }

                    switch (combineType)
                    {
                        case 0:
                            MergeWithMean(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 1:
                            SchuffleEachValue(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 2:
                            ReplaceRandomCluster(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 3:
                            ReplaceRandomSubcluster(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 4:
                            ReplaceRandomNetwork(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 5:
                            ReplaceRandomLayer(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 6:
                            ReplaceRandomNeurode(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network);
                            break;
                        case 7:
                            AlterEachValue(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 8:
                            AlterEachValue(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 9:
                            AlterEachValueAdditive(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 10:
                            AlterEachValueAdditive(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 11:
                            AlterEachValueMultiplicative(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 12:
                            AlterEachValueMultiplicative(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, parameters[topNetworks[lookup[topNetworkIndex]].X][topNetworks[lookup[topNetworkIndex]].Y].NetworkObjects[topNetworks[lookup[topNetworkIndex]].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 13:
                            AlterOneValue(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 14:
                            AlterOneValueAdditive(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 15:
                            AlterOneValueMultiplicative(parameters[i][n].NetworkObjects[k].NetworkWorker.Network, parameters[topNetworks[topNetworkIndex].X][topNetworks[topNetworkIndex].Y].NetworkObjects[topNetworks[topNetworkIndex].Z].NetworkWorker.Network, _alternationValue);
                            break;
                        case 16:
                            parameters[i][n].NetworkObjects[k].NetworkWorker.Network.Reset(rnd);
                            break;
                    }

                    combineType++;

                    if (combineType == combineTypes)
                        combineType = 0;

                    _alternationValue -= .01f;
                    if (_alternationValue < 0)
                        _alternationValue = .5f;
                }
    }

    public void MergeWithMean(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = MergeValues(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias);
                            
                            for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = MergeValues(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w]);
                        }
    }
    
    public void SchuffleEachValue(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (rnd.NextDouble() > .5)
                            { 
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;

                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                            else
                                {
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;

                                    for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                        network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                                }
    }


    public void ReplaceRandomCluster(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        int csIndex = rnd.Next(0, network.NetworkCluster.Count);

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            if (cs != csIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                        }
    }

    public void ReplaceRandomSubcluster(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        int csIndex = rnd.Next(0, network.NetworkCluster.Count);
        int snIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes.Length);

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            if (cs != csIndex && sn != snIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                        }
    }

    public void ReplaceRandomNetwork(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        int csIndex = rnd.Next(0, network.NetworkCluster.Count);
        int snIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes.Length);
        int ntIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes[snIndex].Length);

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            if (cs != csIndex && sn != snIndex && nt != ntIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                        }
    }

    public void ReplaceRandomLayer(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        int csIndex = rnd.Next(0, network.NetworkCluster.Count);
        int snIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes.Length);
        int ntIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes[snIndex].Length);
        int lyIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes[snIndex][ntIndex].Length);

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            if (cs != csIndex && sn != snIndex && nt != ntIndex && ly != lyIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                        }
    }

    public void ReplaceRandomNeurode(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB)//done
    {
        int csIndex = rnd.Next(0, network.NetworkCluster.Count);
        int snIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes.Length);
        int ntIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes[snIndex].Length);
        int lyIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes[snIndex][ntIndex].Length);
        int nIndex = rnd.Next(0, network.NetworkCluster[csIndex].Neurodes[snIndex][ntIndex][lyIndex].Length);

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            if (cs != csIndex && sn != snIndex && nt != ntIndex && ly != lyIndex && n != nIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];
                            }
                        }
    }


    public void AlterEachValue(NeuralNetwork network, NeuralNetwork baseNetwork, float alternation)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValue(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                            for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValue(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                        }
    }

    public void AlterEachValue(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB, float alternation)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (rnd.NextDouble() > .5)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValue(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValue(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                            }
                            else
                                {
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValue(baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                                    for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                        network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValue(baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                                }
    }

    public void AlterEachValueAdditive(NeuralNetwork network, NeuralNetwork baseNetwork, float alternation)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueAdditive(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                            for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueAdditive(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                        }
    }

    public void AlterEachValueAdditive(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB, float alternation)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (rnd.NextDouble() > .5)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueAdditive(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueAdditive(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueAdditive(baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueAdditive(baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                            }
    }

    public void AlterEachValueMultiplicative(NeuralNetwork network, NeuralNetwork baseNetwork, float alternation)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                        {
                            network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueMultiplicative(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                            for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueMultiplicative(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                        }
    }

    public void AlterEachValueMultiplicative(NeuralNetwork network, NeuralNetwork baseNetworkA, NeuralNetwork baseNetworkB, float alternation)//done
    {
        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (rnd.NextDouble() > .5)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueMultiplicative(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueMultiplicative(baseNetworkA.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueMultiplicative(baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);

                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueMultiplicative(baseNetworkB.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                            }
    }


    public void AlterOneValue(NeuralNetwork network, NeuralNetwork baseNetwork, float alternation)//done
    {
        int index = 0;
        bool alteredValue = false;

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (index == network.NeurodeSwapIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValue(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValue(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);

                                alteredValue = true;
                                network.NeurodeSwapIndex++;
                            }
                            else
                                {
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                    for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                        network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];

                                    index++;
                                }
        if (!alteredValue)
            network.NeurodeSwapIndex = 0;
    }

    public void AlterOneValueAdditive(NeuralNetwork network, NeuralNetwork baseNetwork, float alternation)//done
    {
        int index = 0;
        bool alteredValue = false;

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (index == network.NeurodeSwapIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueAdditive(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueAdditive(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                                
                                alteredValue = true;
                                network.NeurodeSwapIndex++;
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];

                                index++;
                            }
        if (!alteredValue)
            network.NeurodeSwapIndex = 0;
    }

    public void AlterOneValueMultiplicative(NeuralNetwork network, NeuralNetwork baseNetwork, float alternation)//done
    {
        int index = 0;
        bool alteredValue = false;

        for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
            for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            if (index == network.NeurodeSwapIndex)
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = AlterValueMultiplicative(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias, rnd, alternation);
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = AlterValueMultiplicative(baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w], rnd, alternation);
                                
                                alteredValue = true;
                                network.NeurodeSwapIndex++;
                            }
                            else
                            {
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias;
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = baseNetwork.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w];

                                index++;
                            }
        if (!alteredValue)
            network.NeurodeSwapIndex = 0;
    }


    private float MergeValues(float valueA, float ValueB)//done
    {
        return (valueA + ValueB) / 2f;
    }

    private float AlterValue(float valueA, System.Random rnd, float alternation)//done
    {
        int alternationMode = rnd.Next(0, 2);

        if (alternationMode == 0)
            return valueA + (.5f - (float)rnd.NextDouble()) * alternation;

        if (alternationMode == 1)
            return valueA * (float)rnd.NextDouble() * (1f - alternation);

        return -1;
    }

    private float AlterValueAdditive(float valueA, System.Random rnd, float alternation)//done
    {
        return valueA + (.5f - (float)rnd.NextDouble()) * alternation;
    }

    private float AlterValueMultiplicative(float valueA, System.Random rnd, float alternation)//done
    { 
        return valueA * (float)rnd.NextDouble() * (1f - alternation);
    }

    public void ResetNetworks()//done
    {
        for (int i = 0; i < _parameters.Length; i++)
            for (int n = 0; n < _parameters[i].Length; n++)
                for (int k = 0; k < _parameters[i][n].NetworkObjects.Count; k++)
                {
                    _parameters[i][n].NetworkObjects[k].NetworkWorker.Network.Reset(rnd);
                }
    }

    public static class NeuralNetworkIO
    {
        public class NetworkContainer
        {
            public float[] NodeWeights;
            public float[] NodeBias;
            public float Fitness;

            public List<IntVector2[][][]> layout;
        }

        public static void SaveToXml(int count, OuterParameters[][] parameters, string filePath, List<IntVector2[][][]> layout = null)
        {
            IntVector3[] topNetwork = ComputeTopNetworks(parameters, count);
            NetworkContainer container = new NetworkContainer();

            List<float> weights = new List<float>();
            List<float> bias = new List<float>();

            for (int i = 0; i < count; i++)
            {
                for (int cs = 0; cs <  parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster.Count; cs++)
                    for (int sn = 0; sn < parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes.Length; sn++)
                        for (int nt = 0; nt < parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                            for (int ly = 0; ly < parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                                for (int n = 0; n < parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                                {
                                    for (int w = 0; w < parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                        weights.Add(parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w]);

                                    bias.Add(parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias);
                                }

                container.NodeWeights = weights.ToArray();
                container.NodeBias = bias.ToArray();
                container.Fitness = parameters[topNetwork[i].X][topNetwork[i].Y].NetworkObjects[topNetwork[i].Z].NetworkWorker.Network.Fitness;
                container.layout = layout;

                XmlSerializer serializer = new XmlSerializer(typeof(NetworkContainer));

                var stream = new FileStream(filePath + "_number_" + i + ".xml", FileMode.Create, FileAccess.Write);

                serializer.Serialize(stream, container);

                stream.Dispose();
            }
        }

        public static void SaveToXml(int count, List<NeuralNetwork> networks, string filePath, List<IntVector2[][][]> layout = null)
        {
            int[] topNetwork = ComputeTopNetworks(networks, count);
            NetworkContainer container = new NetworkContainer();

            List<float> weights = new List<float>();
            List<float> bias = new List<float>();

            for (int i = 0; i < count; i++)
            {
                for (int cs = 0; cs < networks[topNetwork[i]].NetworkCluster.Count; cs++)
                    for (int sn = 0; sn < networks[topNetwork[i]].NetworkCluster[cs].Neurodes.Length; sn++)
                        for (int nt = 0; nt < networks[topNetwork[i]].NetworkCluster[cs].Neurodes[sn].Length; nt++)
                            for (int ly = 0; ly < networks[topNetwork[i]].NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                                for (int n = 0; n < networks[topNetwork[i]].NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                                {
                                    for (int w = 0; w < networks[topNetwork[i]].NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                        weights.Add(networks[topNetwork[i]].NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w]);

                                    bias.Add(networks[topNetwork[i]].NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias);
                                }

                container.NodeWeights = weights.ToArray();
                container.NodeBias = bias.ToArray();
                container.Fitness = networks[topNetwork[i]].Fitness;
                container.layout = layout;

                XmlSerializer serializer = new XmlSerializer(typeof(NetworkContainer));

                var stream = new FileStream(filePath + "_number_" + i + ".xml", FileMode.Create, FileAccess.Write);

                serializer.Serialize(stream, container);

                stream.Dispose();
            }
        }

        public static NeuralNetwork LoadFromXml(List<IntVector2[][][]> layout, string filePath, System.Random rnd)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkContainer));

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            NetworkContainer container = serializer.Deserialize(stream) as NetworkContainer;

            stream.Dispose();

            NeuralNetwork network = new NeuralNetwork(layout, rnd);

            int weightIndex = 0;
            int biasIndex = 0;

            for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
                for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                    for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                        for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                            for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            {
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                {
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = container.NodeWeights[weightIndex];
                                    weightIndex++;
                                }
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = container.NodeBias[biasIndex];
                                biasIndex++;
                            }

            network.Fitness = container.Fitness;

            return network;
        }

        public static NeuralNetwork LoadFromXml(string filePath, System.Random rnd)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkContainer));

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            NetworkContainer container = serializer.Deserialize(stream) as NetworkContainer;

            stream.Dispose();

            NeuralNetwork network = new NeuralNetwork(container.layout, rnd);

            int weightIndex = 0;
            int biasIndex = 0;

            for (int cs = 0; cs < network.NetworkCluster.Count; cs++)
                for (int sn = 0; sn < network.NetworkCluster[cs].Neurodes.Length; sn++)
                    for (int nt = 0; nt < network.NetworkCluster[cs].Neurodes[sn].Length; nt++)
                        for (int ly = 0; ly < network.NetworkCluster[cs].Neurodes[sn][nt].Length; ly++)
                            for (int n = 0; n < network.NetworkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            {
                                for (int w = 0; w < network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight.Length; w++)
                                {
                                    network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Weight[w] = container.NodeWeights[weightIndex];
                                    weightIndex++;
                                }
                                network.NetworkCluster[cs].Neurodes[sn][nt][ly][n].Bias = container.NodeBias[biasIndex];
                                biasIndex++;
                            }

            network.Fitness = container.Fitness;

            return network;
        }
    }

}