using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StructureGen : MonoBehaviour
{
    private List<Vector4> _walls;
    private float _chunkSize = 1;


    private class neurode
    {
        public int connections = 0;
        public int ID;
        public float[] Bias;
        public UnityEngine.Vector3 Position;


        public Vector3 Weight;
        public float Value;

        public List<neurode> Neurodes;
        public float Input;
        private List<neurode> connectedNeurones;
        public float Activation;
        private float[] _weights; //total.noramlized < 1
        private float[] _input;
        public Color Color;

        public float State = 0;

        public neurode()
        {
            Activation = 0;
            Neurodes = new List<neurode>();

            Color = UnityEngine.Random.ColorHSV();
            Color.a = .1f;
        }

        public void SetGetDistance(int index)
        {
            int distance = 0;

            foreach (neurode n in Neurodes)
                SearchIndex(n , index, ref distance);
        }

        public bool SearchIndex(neurode neurode, int index, ref int distance)
        {
            if (distance != 0)
                distance++;

            foreach (neurode n in Neurodes)
            {
                if (n.ID == index)
                {
                    distance++;
                    return true; 
                }
            }

            foreach (neurode n in Neurodes)
                n.SearchIndex(neurode, index, ref distance);

            return false;
        }

        public void ForwardA()
        {
            for (int i = 0; i < Bias.Length; i++)
                Input += Neurodes[i].Activation * Bias[i];


        }

        public void ForwardB()
        {

            Activation = (float)Math.Tanh(Input);

            if (Mathf.Abs(Activation) < Value)
                Activation = 0;
            else
                State = Activation;

            Input = 0;
        }


        public void Draw()
        {
            //for (int i = 0; i < Neurodes.Count; i++)
                //Debug.DrawLine(Position, Neurodes[i].Position, new Color(Color.r, Color.g, Color.b, .1f), 600f);

            //Debug.DrawLine(Position, Position + Vector3.up * Value, new Color(Color.r, Color.g, Color.b, .1f), 30f);

            //for (int i = 0; i < Neurodes.Count; i++)
                //Debug.DrawLine((Position + Neurodes[i].Position) / 2f, (Position + Neurodes[i].Position) / 2f + Vector3.up * Bias[i], new Color(Color.r, Color.g, Color.b, .1f), 600f);


        }

        public Vector3 LesGoFancyInit;

        public void Update(float t) 
        { 
        
            for (int i = 0; i < Neurodes.Count; i++)
            {  
                Vector3 position = Vector3.Lerp(Position + new Vector3((UnityEngine.Random.value - .5f) * Neurodes[i].Activation * .25f, (UnityEngine.Random.value - .5f) * Neurodes[i].Activation * .25f, (UnityEngine.Random.value - .5f) * Neurodes[i].Activation * .25f), Neurodes[i].Position, t);
                //Debug.DrawLine(position, position + LesGoFancyDir[0] * Math.Min(Bias[1], Vector3.Distance(position, Neurodes[1].Position)) , new Color(1f, 1f, 1f, .1f), 30f);

                Debug.DrawLine(position , position + (Neurodes[i].Position - Position).normalized  * Bias[i] * Neurodes[i].Activation, new Color(Color.r, Color.g, Color.b,.1f), 5f);
            }
        }
    }





    private int _neurodeCount = 512;

    private float _maxSearchDistance = 25f;
    private int _minConnectionCount = 1;
    private int _maxConnectionCount = 4;


    private float _frequenz = 15;
    private int _amplitude = 3;
    private float _gain = 2f;
    private int _wavelength = 3;
    int[,] perm;
    neurode[] neurodes;

    void Start()//prodecudral nuculi
    {
        System.Random rnd = new System.Random(110);
        neurodes = new neurode[_neurodeCount];
        Vector3[] noise = new Vector3[3];
        Vector3[] initPositions = new Vector3[_neurodeCount];
        perm = new int[noise.Length, _amplitude];

        for (int x = 0; x < noise.Length; x++)
            for (int y = 0; y < _amplitude; y++)
                perm[x, y] = rnd.Next(0, noise.Length);

        for (int i = 0; i < neurodes.Length; i++)
        {
            neurodes[i] = new neurode();
            neurodes[i].ID = i;
        }

        for (int i = 0; i < noise.Length; i++)
            noise[i] = new Vector3((float)(rnd.NextDouble() - .5), (float)(rnd.NextDouble() - .5), (float)(rnd.NextDouble() - .5));

        for (int i = 0; i < initPositions.Length; i++)
        {
            initPositions[i] = new Vector3((float)(rnd.NextDouble() - .5), (float)(rnd.NextDouble() - .5), (float)(rnd.NextDouble() - .5));

            Vector3 initPosition = initPositions[i];

            for (int a = 0; a < _amplitude; a++)
                for (int n = 0; n < noise.Length; n++)
                {//pi pi || tangent > dot?
                    Vector3 sum = ((noise[n] - noise[perm[n, a]]).normalized - initPosition);
                    Vector3 sumb = Vector3.zero;

                    for (int j = 0; j < _wavelength; j++)
                        sum = new Vector3(sum.x * sum.magnitude, sum.y * sum.magnitude, sum.z * sum.magnitude);

                    for (int o = 0; o < noise.Length; o++)
                        sumb += ((noise[n] - noise[perm[n, a]]).normalized - noise[perm[o, a]]);

                    initPositions[i] += (sum.normalized - sumb.normalized) * _gain;

                    //sum = new Vector3(0, 0, 0);
                    //for (int k = 0; k < initPositions.Length; k++)
                    //    sum += (noise[i] - initPosition).normalized * _amplitude;
                    //initPositions[i] += initPositions[i] + sum;
                }

            neurodes[i].Weight = Vector3.Cross(initPosition, initPositions[i]);
        }

        Vector3 mid = Vector3.zero;

        for (int i = 0; i < initPositions.Length; i++)
            mid += initPositions[i];

        mid /= (float)initPositions.Length;

        Debug.DrawLine(mid, mid + Vector3.up, Color.white, 30f);

        float maxDistance = 0;

        for (int i = 0; i < neurodes.Length; i++)
        {
            //int offset = rnd.Next() * _neurodeCount;
            neurodes[i].Position = initPositions[i];
            neurodes[i].Value = Vector3.Distance(initPositions[i],mid);

            if (neurodes[i].Value > maxDistance)
                maxDistance = neurodes[i].Value;

            //density = function ?
            for (int o = 0; o < _neurodeCount; o++)
            {
                float minDistance = float.PositiveInfinity;
                int partnerNodeID = -1;

                for (int n = 0; n < _neurodeCount; n++)
                {
                    if (i == n || neurodes[n].Neurodes.Count > _maxConnectionCount)
                        continue;

                    float distance = Vector3.Distance(initPositions[i], initPositions[n]);// * ((initPositions[i] - initPositions[n]).normalized - neurodes[i].Weight).sqrMagnitude; 

                    if (distance < _maxSearchDistance)
                    {
                        for (int k = 0; k < neurodes[i].Neurodes.Count; k++)
                            if (neurodes[i].Neurodes[k].ID == neurodes[n].ID)
                            {
                                distance = float.PositiveInfinity;
                                break;
                            }

                        for (int k = 0; k < neurodes[n].Neurodes.Count; k++)
                            if (neurodes[n].Neurodes[k].ID == neurodes[i].ID)
                            {
                                distance = float.PositiveInfinity;
                                break;
                            }


                        if (distance < minDistance)
                            {
                                minDistance = distance;
                                partnerNodeID = neurodes[n].ID;
                            }
                    }
                }

                if (partnerNodeID != -1)
                {
                    neurodes[i].Neurodes.Add(neurodes[partnerNodeID]);
                    neurodes[i].connections++;
                }

                if (neurodes[i].connections > _maxConnectionCount)
                    break;
            }

            while (neurodes[i].connections < _minConnectionCount)
            {
                int partnerNodeID = rnd.Next(0, neurodes.Length);

                for (int k = 0; k < neurodes[i].Neurodes.Count; k++)
                {
                    if (neurodes[i].Neurodes[k].ID == neurodes[partnerNodeID].ID)
                    {
                        partnerNodeID = -1;
                        break;
                    }
                }

                if (partnerNodeID != -1)
                {
                    neurodes[i].Neurodes.Add(neurodes[partnerNodeID]);
                    neurodes[i].connections++;
                }
            }
        }

        for (int i = 0; i < neurodes.Length; i++)
        {
            neurodes[i].Bias = new float[neurodes[i].connections];
            for (int n = 0; n < neurodes[i].connections; n++)
                neurodes[i].Bias[n] = (1f - Vector3.Dot(neurodes[i].Weight, (neurodes[i].Position - neurodes[i].Neurodes[n].Position).normalized)) * 0.5f;
        }

        for (int i = 0; i < neurodes.Length; i++)
            neurodes[i].Value = neurodes[i].Value / maxDistance;


        for (int i = 0; i < neurodes.Length; i++)
            neurodes[i].Position = (neurodes[i].Position - mid) * 5f;

        for (int i = 0; i < neurodes.Length; i++)
            neurodes[i].Draw();

    }

    float t = 0;
    public bool SetInput = false;
    float a = 0;

    void Update()
    {
        for (int i = 0; i < neurodes.Length; i++)
            neurodes[i].Update(t);

        if (t == 1f)
            t = 0;
        else
            t += Time.deltaTime * 5f;

        if (t >= 1f)
        {
            t = 1f;
            for (int i = 0; i < neurodes.Length; i++)
                neurodes[i].ForwardA();
            for (int i = 0; i < neurodes.Length; i++)
                neurodes[i].ForwardB();
            if (SetInput)
            {
                neurodes[UnityEngine.Random.Range(0, neurodes.Length)].Activation = UnityEngine.Random.Range(-5, 5);

                //neurodes[10].Activation = -5f;
                a += 15f;
            }

   

        }
        //for (int i = 0; i < _walls.Count; i++)
        //Debug.DrawLine(new Vector3(_walls[i].x, 0, _walls[i].y), new Vector3(_walls[i].z, 0, _walls[i].w));


    }

}
