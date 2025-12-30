using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static Helper;
using static UnityEditor.Rendering.CameraUI;


public class NetworkCluster
{
    public BaseNeurode[][][][] Neurodes { get { return _neurodes; } set { _neurodes = value; } }
    public int OutPutCount { get { return _outPutCount; } set { _outPutCount = value; } }

    private BaseNeurode[][][][] _neurodes;
    private int _outPutCount;

    public NetworkCluster() {}

    public NetworkCluster(int inputCount, IntVector2[][][] layout, System.Random rnd)//Done
    {
        _neurodes = new BaseNeurode[layout.Length][][][];

        for (int i = 0; i < layout.Length; i++)
        {
            _neurodes[i] = new BaseNeurode[layout[i].Length][][];

            for (int sn = 0; sn < layout[i].Length; sn++)
            {
                _neurodes[i][sn] = new BaseNeurode[layout[i][sn].Length][];

                for (int ly = 0; ly < layout[i][sn].Length; ly++)
                {
                    _neurodes[i][sn][ly] = new BaseNeurode[layout[i][sn][ly].X];

                    if (ly == layout[i][sn].Length - 1)
                        _outPutCount += layout[i][sn][ly].X;
                }
            }
        }

        for (int sn = 0; sn < layout.Length; sn++)
            for (int nt = 0; nt < layout[sn].Length; nt++)
                for (int ly = 0; ly < layout[sn][nt].Length; ly++)
                    for (int n = 0; n < layout[sn][nt][ly].X; n++)
                        if (ly != 0)
                        {
                            int inputNeurodes = 0;

                            for (int iint = 0; iint < _neurodes[sn].Length; iint++)
                                if (_neurodes[sn][iint].Length > ly)
                                    inputNeurodes += layout[sn][iint][ly - 1].X;

                            _neurodes[sn][nt][ly][n] = BaseNeurode.InitNeurodeOfType(layout[sn][nt][ly].Y, inputNeurodes, rnd, n); 
                        }
                        else
                            _neurodes[sn][nt][ly][n] = BaseNeurode.InitNeurodeOfType(layout[sn][nt][ly].Y, inputCount, rnd, n);
    }

    public NetworkCluster(IntVector2[][][] layout, System.Random rnd)//Done
    {
        _neurodes = new BaseNeurode[layout.Length][][][];

        for (int sn = 0; sn < layout.Length; sn++)
        {
            _neurodes[sn] = new BaseNeurode[layout[sn].Length][][];

            for (int nt = 0; nt < layout[sn].Length; nt++)
            {
                _neurodes[sn][nt] = new BaseNeurode[layout[sn][nt].Length][];

                for (int ly = 0; ly < layout[sn][nt].Length; ly++)
                {
                    _neurodes[sn][nt][ly] = new BaseNeurode[layout[sn][nt][ly].X];

                    if (ly == layout[sn][nt].Length - 1)
                        _outPutCount += layout[sn][nt][ly].X;
                }
            }
        }

        for (int sn = 0; sn < layout.Length; sn++)
            for (int nt = 0; nt < layout[sn].Length; nt++)
                for (int ly = 0; ly < layout[sn][nt].Length; ly++)
                    for (int n = 0; n < layout[sn][nt][ly].X; n++)
                        if (ly != 0)
                        {
                            int inputNeurodes = 0;

                            for (int iint = 0; iint < _neurodes[sn].Length; iint++)
                                if (_neurodes[sn][iint].Length > ly)
                                    inputNeurodes += layout[sn][iint][ly - 1].X;

                            _neurodes[sn][nt][ly][n] = BaseNeurode.InitNeurodeOfType(layout[sn][nt][ly].Y, inputNeurodes, rnd, n);
                        }
                        else
                            _neurodes[sn][nt][ly][n] = BaseNeurode.InitNeurodeOfType(layout[sn][nt][ly].Y, 0, rnd, n);
    }

    public void Forward(NetworkCluster input)//Done
    {
        for (int sn = 0; sn < _neurodes.Length; sn++)
            for (int nt = 0; nt < _neurodes[sn].Length; nt++)
                for (int ly = 0; ly < _neurodes[sn][nt].Length; ly++)
                    for (int n = 0; n < _neurodes[sn][nt][ly].Length; n++)
                    {
                        if (ly != 0)
                            _neurodes[sn][nt][ly][n].Forward(_neurodes[sn], ly);
                        else
                            _neurodes[sn][nt][ly][n].Forward(input.Neurodes);
                    }
    }

    public void Forward(float[] input)//Done
    {
        int invalues = 0;

        for (int sn = 0; sn < _neurodes.Length; sn++)
            for (int nt = 0; nt < _neurodes.Length; nt++)
            {
                for (int n = 0; n < _neurodes[sn][nt][0].Length; n++)
                    _neurodes[sn][nt][0][n].Activation = input[n + invalues];

                invalues += _neurodes[sn][nt][0].Length;
            }

        for (int sn = 0; sn < _neurodes.Length; sn++)
            for (int nt = 0; nt < _neurodes[sn].Length; nt++)
                for (int ly = 0; ly < _neurodes[sn][nt].Length; ly++)
                    for (int n = 0; n < _neurodes[sn][nt][ly].Length; n++)
                        if (ly != 0)
                            _neurodes[sn][nt][ly][n].Forward(_neurodes[sn], ly);
    }

    public void Forward(float[][] input)//Done
    {
        for (int sn = 0; sn < _neurodes.Length; sn++)
            for (int nt = 0; nt < _neurodes[sn].Length; nt++)
                for (int n = 0; n < _neurodes[0][nt][0].Length; n++)
                    _neurodes[sn][nt][0][n].Activation = input[sn][n];

        for (int sn = 0; sn < _neurodes.Length; sn++)
            for (int nt = 0; nt < _neurodes[sn].Length; nt++)
                for (int ly = 0; ly < _neurodes[sn][nt].Length; ly++)
                    for (int n = 0; n < _neurodes[sn][nt][ly].Length; n++)
                        if (ly != 0)
                            _neurodes[sn][nt][ly][n].Forward(_neurodes[sn], ly);
    }

}