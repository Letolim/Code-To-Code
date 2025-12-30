using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Helper;
using static UnityEditor.Rendering.CameraUI;

[Serializable]
public class NeuralNetwork
{
    public List<NetworkCluster> NetworkCluster { get { return _networkCluster; } set { _networkCluster = value; } }
    public int NeurodeSwapIndex { get { return _neurodeSwapIndex; } set { _neurodeSwapIndex = value; } }
    public float[] Output { get { return _outPut; } set { _outPut = value; } }
    public float Fitness { get { return _fitness; } set { _fitness = value; } }

    private List<NetworkCluster> _networkCluster;
    private int _neurodeSwapIndex = 0;
    public float _fitness = 0;
    private float[] _outPut;

    public NeuralNetwork(){}
   
    public NeuralNetwork(List<IntVector2[][][]> layout,System.Random rnd)//Done
    {
        rnd = new System.Random();

        _networkCluster = new List<NetworkCluster>() { new NetworkCluster(layout[0], rnd)};

        for (int i = 1; i < layout.Count; i++) 
            _networkCluster.Add(new NetworkCluster(_networkCluster[i -1].OutPutCount, layout[i], rnd));

        _outPut = new float[_networkCluster[_networkCluster.Count -1].OutPutCount];
    }

    public float[] Forward(float[][] input)//Done
    {
        _networkCluster[0].Forward(input);

        for (int i = 1; i < _networkCluster.Count - 1; i++)
            _networkCluster[i].Forward(_networkCluster[i - 1]);

        for (int sn = 0; sn < _networkCluster[_networkCluster.Count - 1].Neurodes.Length; sn++)
            for (int nt = 0; nt < _networkCluster[_networkCluster.Count - 1].Neurodes[sn].Length; nt++)
                for (int n = 0; n < _networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt][_networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt].Length - 1].Length; n++)
                    _outPut[n] = _networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt][_networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt].Length - 1][n].Activation;

        return _outPut;
    }

    public float[] Forward(float[] input)//Done
    {
        _networkCluster[0].Forward(input);

        for (int i = 1; i < _networkCluster.Count; i++)
            _networkCluster[i].Forward(_networkCluster[i - 1]);

        for (int sn = 0; sn < _networkCluster[_networkCluster.Count - 1].Neurodes.Length; sn++)
          for (int nt = 0; nt < _networkCluster[_networkCluster.Count - 1].Neurodes[sn].Length; nt++)
                for (int n = 0; n < _networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt][_networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt].Length -1].Length; n++)
                    _outPut[n] = _networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt][_networkCluster[_networkCluster.Count - 1].Neurodes[sn][nt].Length - 1][n].Activation;

        return _outPut;
    }

    public void Reset(System.Random rnd)//Done
    {
        for (int cs = 0; cs < _networkCluster.Count; cs++)
            for (int sn = 0; sn < _networkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < _networkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < _networkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < _networkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            _networkCluster[cs].Neurodes[sn][nt][ly][n].Reset(rnd);
    }

    public void ResetActivations()//Done
    {
        for (int cs = 0; cs < _networkCluster.Count; cs++)
            for (int sn = 0; sn < _networkCluster[cs].Neurodes.Length; sn++)
                for (int nt = 0; nt < _networkCluster[cs].Neurodes[sn].Length; nt++)
                    for (int ly = 0; ly < _networkCluster[cs].Neurodes[sn][nt].Length; ly++)
                        for (int n = 0; n < _networkCluster[cs].Neurodes[sn][nt][ly].Length; n++)
                            _networkCluster[cs].Neurodes[sn][nt][ly][n].ResetActivation();
    }

}