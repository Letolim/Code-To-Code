//Pseudocode; Also not sure about the rules and if the task completion time of this "algorithm" would be bounded above by a polynomial function.
//https://en.wikipedia.org/wiki/P_versus_NP_problem

public int worldWidth;
public int worldHeight;

public Cube[] cubes;
public Token movableToken;
public Token staticToken;

public int currentStep;


public void Update()
{
	if(tokensAtSamePosition)
	return;	

	if(currentStep == initiationStep)
	{
		tokenA.Initiate(direction);
	}
	
	tokenA.Update();
	
	if(tokenA.x == tokenB.x && tokenA.y == tokenB.y)
	{
		tokensAtSamePosition = true;
		return;
	}
	
	for(int i = 0; i < cubes.Length; i ++)
		cubes[i].UpdateDirections();
	
	for(int i = 0; i < cubes.Length; i ++)
		cubes[i].Update();
} 


public class Token
{
	int X;
	int Y;
	int Direction;
	int stopped = 0;	

	public Token(int x, int y, int direction, int initiationStep)
	{
		X = x;
		Y = y;
		Direction = direction;
	}
	
	public void Update(int step, Cube[] cubes)
	{
		if(stopped != 0)
			return;

		if(step >= intiationStep)
		{
			if(Direction == 0)
				Y++;
			if(Direction == 1)
				X++;
			if(Direction == 2)
				Y--;
			if(Direction == 3)
				X--;
		}

		for(int i = 0; i < cubes.Length; i ++)
			if(cubes[i].X == X && && cubes[i].Y == Y)
				stopped ++;
	}

	public void PushByCube(int direction)
	{
		if(Direction == 0 && tokenA.Y == Y + 1)
			Y++;
		if(Direction == 1 && tokenA.X == X + 1)
			X++;
		if(Direction == 2 && tokenA.Y == Y - 1)
			Y--;
		if(Direction == 3 && tokenA.X == X - 1)
			X--;

		if(Y == worldHeight + 1)
			Y = 0;

		if(X == worldWidth + 1)
			X = 0;

		if(Y == -1)
			Y = worldHeight;
		
		if(X == -1)//You could avoid going -1 by adding a switch that stores the state of wrapping to the other side while Direction == Direction
			X = worldWidth;
	}
}

public class Cube
{
	int X;
	int Y;
	int Direction; // 0 up 1 right 2 down 3 left

	public Cube(int x, int y, int direction)
	{
		X = x;
		Y = y;
		Direction = direction;
	}

	public void UpdateDirections(int index, Cube[] cubes)
	{
			for(int i = 0; i < cubes.Length; i ++)
			{
				if(i == index)
					continue;

				if(Direction == 0 && cubes[i].Direction == 2 && cubes[i].Y == Y + 2)
				{
					Direction = 3;
				}
				
				if(Direction == 1 && cubes[i].Direction == 3 && cubes[i].X == X + 2)
				{
					Direction = 0;
				}

				if(Direction == 2 && cubes[i].Direction == 0 && cubes[i].Y == Y - 2)
				{
					Direction = 1;
				}

				if(Direction == 3 && cubes[i].Direction == 1 && cubes[i].X == X - 2)
				{
					Direction = 2;
				}
			}
	}
	

	public void Update(int index, Cube[] cubes, int worldWidth, int worldHeight, int tokenA)
	{

		if(Direction == 0 && tokenA.Y == Y + 1)
			tokenA.PushByCube(Direction);
				
		if(Direction == 1 && tokenA.Y == X + 1)
			tokenA.PushByCube(Direction);


		if(Direction == 2 && tokenA.Y == Y - 1)
			tokenA.PushByCube(Direction);


		if(Direction == 3 && tokenA.Y == X - 1)
			tokenA.PushByCube(Direction);

	
		if(Direction == 0)
			Y++;
		if(Direction == 1)
			X++;
		if(Direction == 2)
			Y--;
		if(Direction == 3)
			X--;
	
		if(Y == worldHeight + 1)
			Y = 0;

		if(X == worldWidth + 1)
			X = 0;

		if(Y == -1)
			Y = worldHeight;
		
		if(X == -1)
	}
}