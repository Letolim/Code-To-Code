using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLayout
{
    public List<NetworkLayoutField> setup;
    public int feedForwardMemoryCount = 0;


        public Neurode.NeurodeType type;
        public int[] layout;
        public int count;

        public NetworkLayout(Neurode.NeurodeType type, int[] layout)
        {
            this.type = type;
            this.layout = layout;
        }

    public NetworkLayout(Neurode.NeurodeType type, int count)
    {
        this.type = type;
        this.count = count;
        layout = new int[] { };

    }



    public class NetworkLayoutField
    {
        int layer;
        int count;
        CNeurode.NeurodeType type;
    }

    //public Neurode.NeurodeType type;
    //public bool usesMemory;



    //public int[] layout;
    //public int count;

    //public List<int[]> chromosomeCount;
    //public List<bool[]> isUsingMemory;


    //public NetworkLayout(Neurode.NeurodeType type, int[] layout)
    //{
    //    chromosomeCount = new List<int[]>();

    //    for (int i = 0; i < layout.Length; i++)
    //    {
    //        //chromosomeCount.Add(layout[i]);                 //c count = layoutheight chromosomeCount.Add(layout[i]);        |   c count = layoutLength    chromosomeCount.Add(new int[layout[i].Length]);   
    //        chromosomeCount.Add(new int[layout[i].Length]);
    //        isUsingMemory.Add(new bool[layout[i].Length]);

    //    }

    //    for (int i = 0; i < layout.Length; i++)
    //        for (int n = 0; n < chromosomeCount[i].Length; n++)
    //            chromosomeCount[i][n] = layout.Length;

    //    for (int i = 0; i < layout.Length; i++)
    //        for (int n = 0; n < chromosomeCount[i].Length; n++)
    //            isUsingMemory[i][n] = true;

    //    //isUsingMemory


    //    Debug.Log(layout.length + " c count");

    //    this.type = type;
    //    //uses memory
    //    //chromosomeCount
    //    this.layout = layout;
    //}

    //public NetworkLayout(Neurode.NeurodeType type, int count)
    //{
       

    //    this.type = type;
    //    this.count = count;
    //    layout = new int[] {};

    //}



    //public NetworkLayout(Neurode.NeurodeType type, int count)
    //{
    //    LayoutNode node = new LayoutNode(6, true, Neurode.NeurodeType.Tanh);        8   32  32  8           80     

    //    this.type = type;
    //    this.count = count;
    //    layout = new int[] { };

    //}





    ////networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0, 0, 0, 0, 0 });        8   32  32  8           80
    ////networkLayout[1] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 9, 0, 0, 0, 5 });
    ////networkLayout[2] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0, 6, 6, 6, 0 });


    ////networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0, {         }, 0{}, 0{}, 0{}, 0{}});
    ////networkLayout[1] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0{}, 0{}, 0{}, 0{}, 0{}});
    ////networkLayout[2] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0{}, 0{}, 0{}, 0{}, 0{}});

    ////networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0, {         }, 0{}, 0{}, 0{}, 0{}});
    ////networkLayout[1] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0{}, 0{}, 0{}, 0{}, 0{}});
    ////networkLayout[2] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0{}, 0{}, 0{}, 0{}, 0{}});


    //class LayoutNode
    //{
    //    public int ChromosomeCount;
    //    public bool IsUsingMemory;
    //    public Neurode.NeurodeType Type;

    //    public LayoutNode(int chromosomeCount, bool isUsingMemory, Neurode.NeurodeType type)
    //    {
    //        ChromosomeCount = chromosomeCount;
    //        IsUsingMemory = isUsingMemory;
    //        Type = type;
    //    }
    //}

}
