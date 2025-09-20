using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NeuralNetwork 
{
    public Neurode[][] neurodes;//[]Les go
    //public Bool[][] neurodes;//                   [v]Les go

    public System.Random randomGen;
    public float fitness = 0;

    public int shortMemoryCount = 0;
    public int shortMemoryEntraceStartIndex = 0;
    public int shortMemoryExitStartIndex = 0;

    public int seedCount = 0;
    public int seedEntraceStartIndex = 0;
    public int seedExitStartIndex = 0;
    private int[] bin;
    public bool clampValues = false;


    public int generation = 0;

    private float alternationValue = .2f;
    private bool peaked = false;

    public NeuralNetwork(int[] networkLayout,int seed)
    {
        randomGen = new System.Random(seed);
        neurodes = new Neurode[networkLayout.Length][];

        for (int i = 0; i < networkLayout.Length; i++)
            neurodes[i] = new Neurode[networkLayout[i]];

        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
                if (i == 0)
                {
                    neurodes[i][n] = new ReLuNeurode(i, 0, randomGen, Neurode.NeurodeType.ReLuNeurode);
                }
                else
                    neurodes[i][n] = new ReLuNeurode(i, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.ReLuNeurode);


        


        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
            {
                




            }




                for (int i = 1; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
            {

            }
    }

    public void SaveNetwork(string name)
    {
        List<float> networkData = new List<float>();
        NeuralNetworkSaveContainer saveContainer = new NeuralNetworkSaveContainer();

        saveContainer.bias = new float[neurodes.Length][][];
        saveContainer.weight = new float[neurodes.Length][][];
        saveContainer.layer = new int[neurodes.Length][][];
        saveContainer.type = new Neurode.NeurodeType[neurodes.Length][][];
        saveContainer.shortMemoryCount = this.shortMemoryCount;
        saveContainer.shortMemoryEntraceStartIndex = this.shortMemoryEntraceStartIndex;
        saveContainer.shortMemoryExitStartIndex = this.shortMemoryExitStartIndex;

        for (int i = 0; i < neurodes.Length; i++)
        {
            saveContainer.bias[i] = new float[neurodes[i].Length][];
            saveContainer.weight[i] = new float[neurodes[i].Length][];
            saveContainer.layer[i] = new int[neurodes[i].Length][];
            saveContainer.type[i] = new Neurode.NeurodeType[neurodes[i].Length][];


            for (int n = 0; n < neurodes[i].Length; n++)
            {
                saveContainer.bias[i][n] = new float[neurodes[i][n].Bias.Length];
                saveContainer.weight[i][n] = new float[neurodes[i][n].Bias.Length];
                saveContainer.type[i][n] = new Neurode.NeurodeType[neurodes[i][n].Bias.Length];
                saveContainer.layer[i][n] = new int[neurodes[i][n].Bias.Length];

            }
        }

        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
                for (int o = 0; o < neurodes[i][n].Weight.Length; o++)
                {
                    saveContainer.bias[i][n][o] = neurodes[i][n].Bias[o];
                    saveContainer.weight[i][n][o] = neurodes[i][n].Weight[o];
                    saveContainer.layer[i][n][o] = neurodes[i][n].Layer;
                    saveContainer.type[i][n][o] = neurodes[i][n].Type;
                  }
        //networkData.Add(0);//layout


        Debug.Log(name);
        XmlTools.Serialize(saveContainer, @"D:\NeuralNetworkSaves\" + name + ".xml");
    }

    public void LoadNetwork(string name)
    {
        //int cutoff = 0;

        //for (int i = 0; i < name.Length; i++)
        //    if (name[i] == '_')
        //    { 
        //        cutoff = i;
        //        break;
        //    }
        //name = name.Substring(cutoff, name.Length - 1);




        Debug.Log(name);
        NeuralNetworkSaveContainer template = XmlTools.Deserialize<NeuralNetworkSaveContainer>(@"D:\NeuralNetworkSaves\" + name + ".xml");

        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
            {
                for (int o = 0; o < neurodes[i][n].Weight.Length; o++)
                {
                    neurodes[i][n].Bias[o] = template.bias[i][n][o];
                    neurodes[i][n].Weight[o] = template.weight[i][n][o];
                    neurodes[i][n].Layer = template.layer[i][n][o];
                    neurodes[i][n].Type = template.type[i][n][o];
                }
            }

    }


    public void ResetDelta()
    {
        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
                neurodes[i][n].Delta = 0;
    }

    public void Respawn()
    {
        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
                neurodes[i][n].Respawn(randomGen);
    }

    public void MergeInToThis(NeuralNetwork partnerNetwork, Neurode.MergeType mergeType)
    {
        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
                neurodes[i][n].MergeInToThis(partnerNetwork.neurodes[i][n], mergeType);
    }

    public void AlterInToThis(NeuralNetwork partnerNetwork)
    {
        if (!peaked && alternationValue < 1f)
        { 
            alternationValue += .1f;
            if (alternationValue > 1f)
                peaked = true;
        }
        else
        {
            alternationValue -= .1f;
            if (alternationValue < .3f)
                peaked = false;
        }

        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
                neurodes[i][n].AlterNeurode(partnerNetwork.neurodes[i][n], alternationValue, randomGen);
    }

    public NeuralNetwork(NetworkLayout[] networkLayout,int seed)
    {
        randomGen = new System.Random(seed);

        for (int i = 0; i < networkLayout.Length; i++)
            if (networkLayout[i].type == Neurode.NeurodeType.ShortMemoryNeurode)
                shortMemoryCount += networkLayout[i].count;

        for (int i = 0; i < networkLayout.Length; i++)
            if (networkLayout[i].type == Neurode.NeurodeType.Seed)
                seedCount += networkLayout[i].count;

        if (seedCount != 0)
        {
            if (seedCount <= 3)
                seedCount = 3;

            bin = new int[seedCount];

            bin[0] = 0;
            bin[1] = 1;
            bin[2] = 2;
        }
        for (int i = 3; i < seedCount; i ++)
        {
            bin[i] = bin[i-1] * 2;
        }

        int length = 0;

        for (int i = 0; i < networkLayout.Length; i++)//highest layer count from the sets
        {
            if (length < networkLayout[i].layout.Length)
                length = networkLayout[i].layout.Length;
        }

        neurodes = new Neurode[length][];

        for (int i = 0; i < length; i++)//calulate the length of each layer
        {
            int layerLength = 0;

            for (int n = 0; n < networkLayout.Length; n++)
            {
                if (networkLayout[n].layout.Length <= i)
                    continue;
                layerLength += networkLayout[n].layout[i];

            }

            if (i == 0 || i == length - 1)// loop input and out put layer for memory
            {
                if (i == 0)
                    shortMemoryEntraceStartIndex = layerLength;
                if (i == length - 1)
                    shortMemoryExitStartIndex = layerLength;

                layerLength += shortMemoryCount;

                if (i == 0)
                    seedEntraceStartIndex = layerLength;
                if (i == length - 1)
                    seedExitStartIndex = layerLength;

                layerLength += seedCount;

            }

            neurodes[i] = new Neurode[layerLength];
        }
        for (int i = 0; i < length; i++)//initialise layers
        {
            int d = 0;

            for (int n = 0; n < networkLayout.Length; n++)
            {
                if (networkLayout[n].layout.Length <= i)
                    continue;

                for (int k = 0; k < networkLayout[n].layout[i]; k++)
                {
                    if (i == 0)
                    {
                        if (networkLayout[n].type == Neurode.NeurodeType.ReLuNeurode)
                            neurodes[0][d] = new ReLuNeurode(d, 0, randomGen, Neurode.NeurodeType.ReLuNeurode);
                        if (networkLayout[n].type == Neurode.NeurodeType.TanNeurode)
                            neurodes[0][d] = new TanNeurode(d, 0, randomGen, Neurode.NeurodeType.TanNeurode);
                        if (networkLayout[n].type == Neurode.NeurodeType.MemoryNeurode)
                            neurodes[0][d] = new MemoryNeurode(d, 0, randomGen, Neurode.NeurodeType.MemoryNeurode);
                        if (networkLayout[n].type == Neurode.NeurodeType.SigmoidNeurode)
                            neurodes[0][d] = new SigmoidNeurode(d, 0, randomGen, Neurode.NeurodeType.SigmoidNeurode);
                    }
                    else
                    {
                        if (networkLayout[n].type == Neurode.NeurodeType.ReLuNeurode)
                            neurodes[i][d] = new ReLuNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.ReLuNeurode);
                        if (networkLayout[n].type == Neurode.NeurodeType.TanNeurode)
                            neurodes[i][d] = new TanNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.TanNeurode);
                        if (networkLayout[n].type == Neurode.NeurodeType.MemoryNeurode)
                            neurodes[i][d] = new MemoryNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.MemoryNeurode);
                        if (networkLayout[n].type == Neurode.NeurodeType.SigmoidNeurode)
                            neurodes[i][d] = new SigmoidNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.SigmoidNeurode);
                    }
                    d++;
                }
            }
            //-----------------------------------------------------------------------------------------------------------------------------!!!!!!
            if (i == 0 || i == length - 1)
            {
                for (int k = 0; k < shortMemoryCount; k++)
                {
                    if (i == 0)
                        neurodes[i][d] = new TanNeurode(d, 0, randomGen, Neurode.NeurodeType.TanNeurode);
                    else
                        neurodes[i][d] = new TanNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.TanNeurode);
                    d++;
                }

                for (int k = 0; k < seedCount; k++)
                {
                    if (i == 0)
                        neurodes[i][d] = new TanNeurode(d, 0, randomGen, Neurode.NeurodeType.ReLuNeurode);
                    else
                        neurodes[i][d] = new TanNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.ReLuNeurode);
                    d++;
                }

            }
            //-------------------------------------------------------------------------------------------------------------------------------------
        }



        //for (int i = 0; i < length; i++)//initialise layers
        //{
        //    int d = 0;

        //    //-----------------------------------------------------------------------------------------------------------------------------!!!!!! 
        //    if (i == 0 || i == length - 1)
        //    { // ####### > ###
        //        for (int k = 0; k < shortMemoryCount; k++)
        //        {
        //            if (i == 0)
        //                neurodes[i][d] = new TanNeurode(d, 0, randomGen, Neurode.NeurodeType.TanNeurode);//
        //            else
        //                neurodes[i][d] = new TanNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.TanNeurode);
        //            d++;
        //        }

        //        for (int k = 0; k < seedCount; k++) //Seed is a seed .... >_> rnd............................................................
        //        {
        //            if (i == 0)
        //                neurodes[i][d] = new TanNeurode(d, 0, randomGen, Neurode.NeurodeType.ReLuNeurode);
        //            else
        //                neurodes[i][d] = new TanNeurode(d, neurodes[i - 1].Length, randomGen, Neurode.NeurodeType.ReLuNeurode);
        //            d++;
        //        }

        //    }
        //    //-------------------------------------------------------------------------------------------------------------------------------------

        //    for (int n = 0; n < networkLayout.Length; n++)
        //    {
        //        if (networkLayout[n].layout.Length <= i)
        //            continue; // #######

        //        for (int k = 0; k < networkLayout[n].layout[i]; k++)
        //        {
        //            if (i == 0)//input layer >_>
        //                neurodes[0][d] = new CNeurode(randomGen, networkLayout[n].type, 16, false, 0); //  memory?
        //            else// output.....
        //                neurodes[i][d] = new CNeurode(randomGen, networkLayout[n].type, 16, false, neurodes[i - 1].Length); //  memory?
        //            d++;
        //        }
        //    }

        //}
    }

    public NeuralNetwork()
    {

    }

    public void RunForward(float[] input)
    {
        for (int n = 0; n < neurodes[0].Length - shortMemoryCount; n++) //(####) ################
        {
            neurodes[0][n].Delta = input[n];// Random.Range(-1,2);
        }

        for (int i = 0; i < shortMemoryCount; i++)
            neurodes[0][shortMemoryEntraceStartIndex + i].Delta = neurodes[neurodes.Length - 1][shortMemoryExitStartIndex + i].Delta;

        for (int i = 1; i < neurodes.Length; i++)
        {
            for (int n = 0; n < neurodes[i].Length; n++)
                neurodes[i][n].RunForward(neurodes[i - 1]);
        }

        if (clampValues)//.......... for input / output       
        {
            float min = 5000;
            float max = -5000;

            for (int n = 0; n < neurodes[neurodes.Length - 1].Length; n++)
            {
                if (neurodes[neurodes.Length - 1][n].Delta < min)
                    min = neurodes[neurodes.Length - 1][n].Delta;
                if (neurodes[neurodes.Length - 1][n].Delta > max)
                    max = neurodes[neurodes.Length - 1][n].Delta;
            }

            if(min < 0)
            {
                if (min > -.45)
                    min -= .2f;
            }
            else
            {
                if (min > .05)
                    min -= .2f;
            }

            for (int n = 0; n < neurodes[neurodes.Length - 1].Length; n++)
                neurodes[neurodes.Length - 1][n].Delta = Mathf.InverseLerp(min, max, neurodes[neurodes.Length - 1][n].Delta);//normalize....
        }
    }



    float maxValueLastDrawStage = 1;
    int drawStepCount = 0;
    float distanceX = 5f;
    float distanceZ = 1f;

    public void DrawConnections()
    {
        for (int i = 0; i < neurodes.Length; i++)
            for (int n = 0; n < neurodes[i].Length; n++)
            {
                Vector3 position = new Vector3(i * distanceX, 0, n * distanceZ - (neurodes[i].Length / 2 * distanceZ));
                Vector3 positionUp = new Vector3(position.x, neurodes[i][n].Delta * 3f, position.z);
                Debug.DrawLine(position, positionUp, Color.blue);
                
                if (i == 0)
                    continue;

                for (int k = 0; k < neurodes[i - 1].Length; k++)
                {
                    float c = neurodes[i][n].Weight[k] * neurodes[i - 1][k].Delta;
                    if (c <= 0)
                        c *= -1;
                    if (c > 1)
                        c = 1;
              
                    if (maxValueLastDrawStage < c)
                        maxValueLastDrawStage = c;

                    c /= maxValueLastDrawStage;

                    Vector3 positionB = new Vector3((i - 1) * distanceX, 0, k * distanceZ - (neurodes[i - 1].Length / 2 * distanceZ));
                    Debug.DrawLine(position, positionB,new Color(c,c,c));
                }
            }

        for (int i = 0; i < shortMemoryCount; i++)
        {
            Vector3 position = new Vector3(0 * distanceX, 0, (shortMemoryEntraceStartIndex + i) * distanceZ - (neurodes[0].Length / 2 * distanceZ));
            Vector3 positionB = new Vector3((neurodes.Length - 1) * distanceX, 0, (shortMemoryExitStartIndex + i) * distanceZ - (neurodes[neurodes.Length - 1].Length / 2 * distanceZ));
            Debug.DrawLine(position, positionB, Color.green);
        }

        if (drawStepCount == 300)
        {
            drawStepCount = 0;
            maxValueLastDrawStage = 1;
        }

        drawStepCount++;
    }


    public class NeuralNetworkSaveContainer
    {
        [SerializeField] public float[][][] bias;
        [SerializeField] public float[][][] weight;
        [SerializeField] public Neurode.NeurodeType[][][] type;
        [SerializeField] public int[][][] layer;
        [SerializeField] public int[] layout;

        [SerializeField] public int shortMemoryCount = 0;
        [SerializeField] public int shortMemoryEntraceStartIndex = 0;
        [SerializeField] public int shortMemoryExitStartIndex = 0;
    }
}
