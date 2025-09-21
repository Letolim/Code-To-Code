using System;

public class LayerCube
{
    public static readonly int[,] primenumberGrid = new int[,] { { 2, 5 }, { 3, 7 } };
    public int Vector { get { return vector; } set { vector = value; } }//Out for the activation value of a NeuralNetwork
    public bool IsRunning { get; set; }

    private int vector = 0;
    private int divident = 2;

    private int count = 0;
    private int positionA_x = 0;
    private int positionA_y = 0;
    private bool stateA = true;

    private int countB = 0;
    private int positionB_x = 0;
    private int positionB_y = 0;
    private bool stateB = false;

    public void Reset(float magnitude)//intial value from the upper layer ""(int)(neurodes[n].Delta * weightMatrix[0][v] + biasVector[0][v])  * 100;""
    {
        positionA_x = 0;
        positionA_y = 0;

        positionB_x = 0;
        positionB_y = 0;

        for (int i = 0; i < (int)(magnitude * 100f); i++)
            Update();
    }

    public void Update()
    {
        UpdatePosition(ref positionA_x, ref positionA_y, stateA);
        UpdatePosition(ref positionB_x, ref positionB_y, stateB);

        if ((positionA_x == positionB_x) && (positionA_y == positionB_y))
        {
            vector = (count + countB) / divident;

            count = vector;
            countB = vector;

            vector++;
        }

        count++;
        countB++;
    }

    public void UpdatePosition(ref int x, ref int y, bool reverse)
    {
        if (reverse)
        {
            if (x == 0 && y == 0)
                y = 1;

            if (x == 0 && y == 1)
                x = 1;

            if (x == 1 && y == 1)
                y = 0;

            if (x == 1 && y == 0)
                x = 0;
        }
        else
        {
            if (x == 0 && y == 0)
                x = 1;

            if (x == 1 && y == 0)
                y = 1;

            if (x == 0 && y == 1)
                x = 0;

            if (x == 1 && y == 1)
                y = 0;

        }

        if (count == primenumberGrid[positionA_x, positionA_y])
            count = 0;

        if (countB == primenumberGrid[positionA_x, positionA_y])
            countB = 0;
    }
}

//
//                                                                                                                                  -------------
//           11-----13                                                                                                              *------------
//           /      /|                                                       "maxed," then motion slows                             ------------*           ------------*
//          3------7 |- out = ?
//          |      | |
// in = 47 -| time | 21       
//          |      |/ 
//          2------5        //3D max space if same distance move slower?
//
//




//if((positionA_x == positionB_x) && (positionA_y == positionB_y))
//magnitude += (delta_A + delta_B);
//Hmmm no big spoilers pls chatGPT




//                      
//                      *------------   ------------*
//
//                      ------*------   ------*------
//                      
//                      ------------*   *------------
//
//
//