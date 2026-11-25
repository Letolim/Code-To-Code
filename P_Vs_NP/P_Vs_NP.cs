//https://en.wikipedia.org/wiki/P_versus_NP_problem

public class Main
{
    public int CurrentStep;
    public Token TokenA;
    public Token TokenB;
    public Vector[] Vectors;
    public string States[];

    public void Main(int startStep, int direction)
    {

        Token TokenA = new Token(new int[] { 0, 0 }, direction, new int[] { 0, 0 }, new int[] { 4, 4 }, startStep);
        Token TokenB = new Token(new int[] { 0, 0 }, 0, new int[] { 0, 0 }, new int[] { 4, 4 }, null);

        Vectors = new Vector[3];
        Vectors[0] = new Vector(new int[] { 2, 3 }, 1, new int[] { 0, 0 }, new int[] { 4, 4 });
        Vectors[1] = new Vector(new int[] { 3, 2 }, 0, new int[] { 0, 0 }, new int[] { 4, 4 });
        Vectors[2] = new Vector(new int[] { 2, 3, 6, 1, 6, 8, 9 }, 8, new int[] { 0, 0, 0, 0, 0, 0, 0 }, new int[] { 16, 16, 16, 16, 16, 16, 16 });

        int initialStateArrayLength = 0;

        for (int i = 0; i < Vectors.Length; i++)
            for (int n = 0; n < Vectors[i].Max.Length; n++)
                initialStateArrayLength += Vectors[i].Max[n] * 2;

        for (int i = 0; i < TokenA.Max.Length; i++)
            initialStateArrayLength += TokenA.Max[i] * 2;

        for (int i = 0; i < TokenB.Max.Length; i++)
            initialStateArrayLength += TokenB.Max[i] * 2;

        States = new string[initialStateArrayLength];

        CurrentStep = 0;


        while (true)
        {
            int state = Update();

            if (state == 1)
                break;// tokenA equals tokenB;

            if (state == 2)
                break;//tokenA cant equal tokenB or there is a more efficient solution
        }
    }

    public int Update()
    {

        bool tokenAequalsTokenB = true;

        if (TokenA.Dimensions.Length < TokenB.Dimensions.Length)
        {
            for (int n = 0; n < TokenA.Dimensions.Length; n++)
            {
                if (TokenA.Dimensions[n] != TokenB.Dimensions[n])
                {
                    tokenAequalsTokenB = false;
                    break;
                }
            }
        }
        else
        {
            for (int n = 0; n <= TokenB.Dimensions.Length; n++)
            {
                if (TokenB.Dimensions[n] != TokenA.Dimensions[n])
                {
                    tokenAequalsTokenB = false;
                    break;
                }
            }
        }

        if (tokenAequalsTokenB)
            return 1;

        TokenA.Update(CurrentStep, Vectors);
        TokenB.Update(CurrentStep, Vectors);

        for (int i = 0; i < Vectors.Length; i++)
            Vectors[i].Update(i, Vectors);

        string state = GetState(TokenA, TokenB, Vectors);

        if (ContainsState(state))
            return 2;

        SaveState(state);

        CurrentStep++;

        return 0;
    }

    public bool ContainsState(string state)
    {
        for (int i = 0; i < States.Length; i++)
            if (States[i] == null && States[i] == state)
                return true;

        return false;
    }

    public string GetState(DimensionalObject tokenA, DimensionalObject tokenB, DimensionalObject vectors)
    {
        string state = "";

        for (int i = 0; i < tokenA.Dimensions; i++)
            state += tokenA.Dimensions[i];

        state += tokenA.Direction;

        for (int i = 0; i < tokenB.Dimensions; i++)
            state += tokenB.Dimensions[i];

        state += tokenB.Direction;


        for (int i = 0; i < vectors.Length; i++)
        {
            for (int n = 0; n < vectors[i].Dimensions; n++)
                state += vectors[i].Dimensions[n];

            state += vectors[i].Direction;
        }

        return state;
    }

    public void SaveState(string state)
    {
        if (States[States.Length - 1] != null && States[States.Length - 1] != "")
        {
            System.Array.Resize(ref States, States.Length + 100;
            States[Sates.Length - 101] = state;

            return;
        }
        else
        {
            for (int i = 0; i < States.Length; i++)
                if (States[i] == null || States[i] == "")
                    States[i] = state;

            return;
        }
    }

    //public string SaveState(DimensionalObject[][] objects)
    //{

    //}

    public class Token : DimensionalObject
    {

        public int InitiationStep;

        public Token(int[] dimensions, int direction, int[] min, int[] max, int initiationStep = -1)
        {
            this.Dimensions = dimensions;
            this.Direction = direction;
            this.Dimension = DirectionToDimension(Direction);
            this.Min = min;
            this.Max = max;
            this.InitiationStep = initiationStep;
            this.States = new string[Min * 2 + Max * 2];
        }

        public void Update(int step, Vector[] vectors)
        {

            int[] dimensionsBeforeMoving = new int[Dimension.Length];

            for (int i = 0; i < Dimensions.Length; i++)
                dimensionsBeforeMoving[i] = Dimension[i];

            if (step >= intiationStep)
                Move();

            int[] dimensionsBeforeVectorInteraction = new int[Dimension.Length];

            for (int i = 0; i < Dimensions.Length; i++)
                dimensionsBeforeVectorInteraction[i] = Dimension[i];

            while (true)
            {
                bool gotMovedByAVector = false;

                for (int i = 0; i < vectors.Length; i++)
                {
                    bool dimensionsEqualThisDimensions = true; // FYI can only equal one vector since the Vectors them self cant equal each other...

                    for (int n = 0; n < vectors[i].Dimensions.Length; n++)
                        for (int o = 0; o < Dimensions.Length; o++)
                        {
                            if (vectors[i].Dimensions[n] != Dimensions[o])
                                dimensionsEqualThisDimensions = false;
                        }

                    if (dimensionsEqualThisDimensions)
                    {

                        for (int n = 0; n < Dimensions.Length; n++)
                        {
                            if (vectors[i].Direction == n)
                            {
                                Dimensions[n]++;

                                if (Dimensions[n] == Max[n])
                                    Dimensions[n] = Min[n];

                                break;
                            }

                            if (vectors[i].Direction == n + 1)
                            {
                                Dimensions[n]--;

                                if (Dimensions[n] == Min[n])
                                    Dimensions[n] = Max[n];

                                break;
                            }
                        }

                        gotMovedByAVector = true;

                        break;
                    }
                }

                bool isLooping = true;

                for (int i = 0; i < Dimensions.Length; i++)
                    if (dimensionsBeforeVectorInteraction[i] != Dimension[i])
                        isLooping = false;

                if (isLooping)
                {
                    for (int i = 0; i < Dimensions.Length; i++)
                        Dimensions[i] = dimensionsBeforeMoving[i];
                }

                if (!gotMovedByAVector)
                    break;
            }
        }
    }

    public class Vector : DimensionalObject
    {

        public Vector(int dimensions[], int direction, int[] min, int[] max)
        {
            this.Dimensions = dimensions;
            this.Direction = direction;
            this.Dimension = DirectionToDimension(Direction);
            this.Min = min;
            this.Max = max;
        }

        public int Reflect(int direction, int dimensions)
        {

            for (int i = 0; i < dimensions; i++)
            {
                if (Direction == i)
                {
                    Direction += 2;

                    if (Direction >= dimensions * 2)
                    {
                        Direction = 1;
                        break;
                    }
                }

                if (Direction == i + 1)
                {
                    Direction += 2;

                    if (Direction >= dimensions * 2)
                    {
                        Direction = 0;
                        break;
                    }
                }
            }
        }

        public void Update(int index, Vector[] vectors)
        {

            for (int i = 0; i < vectors.Length; i++)
            {
                if (i == index)
                    continue;

                if (Dimension != vectors[i].Dimension || Direction == vectors[i].Direction)
                    continue;

                if (IsOnSameComplexPlane(Dimension, vectors[i].Dimension))
                {
                    for (int i = 0; i < Dimensions.Length; i++)
                    {
                        if (Direction == i)
                        {
                            for (int n = 0; n < vectors.Length; n++)
                                if (vectors[n].Direction == n + 1)
                                {
                                    if (Dimensions[Dimension] + 1 == vectors[n].Dimensions[vectors[i].Dimension] - 1)
                                    {
                                        Direction = Reflect(Direction, dimensions.Length);
                                        Dimension = DirectionToDimension(Direction);
                                        vectors[n].Direction = Reflect(worldObjects[n].Direction, vectors[n].dimensions.Length);
                                        vectors[n].Dimension = DirectionToDimension(vectors[n].Direction);
                                        return;
                                    }
                                }
                        }

                        if (Direction == i + 1)
                        {
                            for (int n = 0; n < vectors.Length; n++)
                                if (vectors[n].Direction == i)
                                {
                                    if (Dimensions[Dimension] - 1 == vectors[n].Dimensions[vectors[n].Dimension] + 1)
                                    {
                                        Direction = Reflect(Direction, dimensions.Length);
                                        Dimension = DirectionToDimension(Direction);
                                        vectors[n].Direction = Reflect(vectors[n].Direction, vectors[n].dimensions.Length);
                                        vectors[n].Dimension = DirectionToDimension(vectors[n].Direction);
                                        return;
                                    }
                                }
                        }
                    }
                }

                Move();
            }
        }
    }

    public abstract class DimensionalObject
    {
        public int[] Dimensions;
        public int Direction;
        public int Dimension;
        public int[] Min;
        public int[] Max;

        public void Move()
        {

            for (int i = 0; i < Dimensions.Length; i++)
            {
                if (Direction == i)
                {
                    Dimensions[i]++;

                    if (Dimensions[i] == Max[i])
                        Dimensions[i] = Min[i];

                    return;
                }

                if (Direction == i + 1)
                {
                    Dimensions[i]--;

                    if (Dimensions[i] == Min[i])
                        Dimensions[i] = Max[i];

                    return;
                }
            }
        }

        public int DirectionToDimension()
        {

            for (int i = 0; i < Dimensions; i++)
            {
                if (Direction == i)
                    return i;

                if (Direction == i + 1)
                    return i;
            }
        }

        public bool IsOnSameComplexPlane(int excludedDimensionA, int excludedDimensionB, DimensionalObject worldObject)
        {

            if (excludedDimensionA != excludedDimensionB)
                return false;

            if (Dimension != worldObject.Dimension)
                return false;

            for (int i = 0; i < Dimensions.Length; i++)
            {
                if (i == worldObject.Dimensions.Length)
                    return true;

                if (i == excludedDimensionA)
                    continue;

                for (int n = 0; n < worldObject.Dimensions.Length; n++)
                {
                    if (Dimensions[i] != worldObject.Dimensions[n])
                        return false;

                    if (n == excludedDimensionB)
                        continue;
                }
            }

            return true;
        }
    }
}

Copyright (c) 25/11/2026 Nikita Knöfel.
This work is licensed under a Creative Commons Attribution 4.0 International License (CC BY 4.0). You are
free to share — copy and redistribute the material in any medium or format — and adapt — remix,
transform, and build upon the material for any purpose, even commercially — under the following terms:
Attribution — You must give appropriate credit, provide a link to the license, and indicate if changes were
made. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you
or your use. No additional restrictions — You may not apply legal terms or technological measures that
legally restrict others from doing anything the license permits.
For the full license text, see: https://creativecommons.org/licenses/by/4.0
