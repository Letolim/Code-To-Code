Mostly working multi dimensional nested ffn implementation for unity3d runs on separate threads so performance shouldn't be a issue for learning/training.
(Also added a leapfrog ffn based on a function that generates somewhat nucleoli like structures and weights them based one a pseudo(spherical surface flow) and distance to center Structuregen.cs)




        NetworkWorker = new Worker(new List<Helper.IntVector2[][][]>        {
            new Helper.IntVector2[][][]//[cluster]
            {
                new Helper.IntVector2[][]
                {
                    new Helper.IntVector2[]// Two layers input with 64 neurodes and a hidden one with 255 linking to the next cluster 
                        {new Helper.IntVector2(64,0), new Helper.IntVector2(255,0)}
                },
            },
            new Helper.IntVector2[][][]//"Two layers" 132 and 132
            {
                new Helper.IntVector2[][]
                {
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(38,0), new Helper.IntVector2(38,0) },
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(38,0), new Helper.IntVector2(38,0) },
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(14,0), new Helper.IntVector2(14,1) },
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(14,0), new Helper.IntVector2(14,2) },
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(14,0), new Helper.IntVector2(14,3) },
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(14,0), new Helper.IntVector2(14,3) }
                },
            },
            new Helper.IntVector2[][][]//Seven layers 140  64 16 16 16 16 8 > output is the sum of each last layer 140
            {
                new Helper.IntVector2[][]
                {
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(76,0)}
                },
                new Helper.IntVector2[][]//sn
                {
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(24,0), new Helper.IntVector2(24,0)},
                    new Helper.IntVector2[]                                                                                       
                        { new Helper.IntVector2( 8,0), new Helper.IntVector2( 8,0), new Helper.IntVector2( 8,0),new Helper.IntVector2(8,0),new Helper.IntVector2(8,0),new Helper.IntVector2(8,0)},//[layer] 
                },
                new Helper.IntVector2[][]
                {
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(24,0), new Helper.IntVector2(24,0)},
                    new Helper.IntVector2[]
                        { new Helper.IntVector2( 8,0), new Helper.IntVector2( 8,0), new Helper.IntVector2( 8,0),new Helper.IntVector2(8,0),new Helper.IntVector2(8,0), new Helper.IntVector2(8,0), new Helper.IntVector2(8,0)},
                },
            },
            new Helper.IntVector2[][][]
            {
                new Helper.IntVector2[][]
                {
                    new Helper.IntVector2[] 
                        { new Helper.IntVector2(60, 0)},
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(8,1), new Helper.IntVector2(8,0)},
                    new Helper.IntVector2[]
                        { new Helper.IntVector2(8,3), new Helper.IntVector2(8,0)}
                }
            }
            ,
            new Helper.IntVector2[][][]//Out = Result of Forward = 255
            {
                new Helper.IntVector2[][]
                {
                    new Helper.IntVector2[] {new Helper.IntVector2(255, 0)}
                },
            }
        }, new System.Random());
    }