//https://en.wikipedia.org/wiki/P_versus_NP_problem

public int CurrentStep;
public Token TokenA;
public Token TokenB;
public Vector[] Vectors;

public void Main(int startStep, int direction)
{
    Token TokenA = new Token(new int[] { 0, 0 }, 1, new int[] { 0, 0 }, new int[] { 4, 4 }, 4);
    Token TokenB = new Token(new int[] { 0, 0 }, 0, new int[] { 0, 0 }, new int[] { 4, 4 }, null);
	
    Vectors = new Vector[3];
    Vectors[0] = new Vector(new int[] { 2, 3 }, 1, new int[] { 0, 0 }, new int[] { 4, 4 });
    Vectors[1] = new Vector(new int[] { 3, 2 }, 0, new int[] { 0, 0 }, new int[] { 4, 4 });
    Vectors[2] = new Vector(new int[] { 2, 3, 6, 1, 6, 8, 9 }, 8, new int[] { 0, 0, 0, 0, 0, 0, 0 }, new int[] { 16, 16, 16, 16, 16, 16, 16 });
}

public void Update()
{
	bool tokenAequalsTokenB = true;
	
	if(TokenA.Dimensions.Length < TokenB.Dimensions.Length)
	{
		for (int n = 0; n < TokenA.Dimensions.Length; n++)
		{
			if(TokenA.Dimensions[n] != TokenB.Dimensions[n])
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
				if(TokenB.Dimensions[n] != TokenA.Dimensions[n])
				{	
					tokenAequalsTokenB = false;
					break;
				}
			}
		}

	if(tokenAequalsTokenB)
		return;// TokenA == TokenB.
	
    TokenA.Update(CurrentStep, Vectors);
    TokenA.Update(CurrentStep, Vectors);

    for (int i = 0; i < Vectors.Length; i++)
        Vectors[i].Update(i, Vectors);

    CurrentStep++;
} 

public class Token : DimensionalObject
{
	public int InitiationStep;

    public Token(int[] dimensions, int direction, int[] min, int[] max, int initiationStep = -1)
    {
        this.Dimensions = dimensions;
        this.Direction = direction;
        this.Dimension = DirectionToDimension(Direction);
        this.min = min;
        this.max = max;
		this.InitiationStep = initiationStep;
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

                            if (Dimensions[n] == max[n])
                                Dimensions[n] = min[n];

                            break;
                        }

                        if (vectors[i].Direction == n + 1)
                        {
                            Dimensions[n]--;

                            if (Dimensions[n] == min[n])
                                Dimensions[n] = max[n];

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
                return;
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
        this.min = min;
		this.max = max;
	}

	public int Reflect(int direction, int dimensions)
	{

        for (int i = 0; i < dimensions; i ++)
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
                for (int i = 0; i < Dimensions.Length; i ++)
                {
					if (Direction == i)
					{
						for (int n = 0; n < vectors.Length; n ++)
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
                        for (int n = 0; n < vectors.Length; n ++)
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
    public int[] min;
    public int[] max;

	public void Move()
	{
		
        for (int i = 0; i < Dimensions.Length; i++)
        {
            if (Direction == i)
            {
                Dimensions[i]++;

                if (Dimensions[i] == max[i])
                    Dimensions[i] = min[i];

                return;
            }

            if (Direction == i + 1)
            {
                Dimensions[i]--;

                if (Dimensions[i] == min[i])
                    Dimensions[i] = max[i];

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
