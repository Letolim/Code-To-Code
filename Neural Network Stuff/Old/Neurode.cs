using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Neurode
{
    public enum MergeType { Merge, Schuffle, Lerp, Alter}
    public enum NeurodeType { ReLuNeurode, TanNeurode, ShortMemoryNeurode, MemoryNeurode, SigmoidNeurode, Seed };

    public abstract NeurodeType Type { get; set; }
    public abstract int Layer { get; set; }
    public abstract float Delta { get; set; }
    public abstract float[] Bias { get; set; }
    public abstract float[] Weight { get; set; }
    //public abstract NetworkLayout.NeurodeType type;

    public abstract void RunForward(Neurode[] parentLayer);

    public abstract void RunForward(Neurode[][] network);


    public abstract void AlterNeurode(Neurode baseNeurode, float alternation, System.Random randomGen);
 
    
    public abstract Neurode Merge(Neurode neurodeA, Neurode neurodeB, MergeType type);
    public abstract void MergeInToThis(Neurode partnerNeurode, MergeType type);
    public abstract void Respawn(System.Random randomGen);

}
