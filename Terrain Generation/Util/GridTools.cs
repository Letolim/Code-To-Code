using System.Collections;
using System.Collections.Generic;
using System;

public static class GridTools
{
    public static bool IsInsideBounds(int posX,int posY, int minX,int maxX, int minY,int maxY)
    {
        if(posX < minX || posY < minY || posX > maxX || posY > maxY)
            return false;
        else return true;
    }

    public static bool IsInsideArrayBounds2D(int posX, int posY, int length0, int length1)
    {
        if (posX < 0 || posY < 0 || posX >= length0 || posY >= length1)
            return false;
        else return true;
    }

    public static Slope2D GetSlope(int positionX, int positionY, float[,] heightMap)
    {
        Slope2D slope = new Slope2D();

        float sample0 = 0;
        if (IsInsideArrayBounds2D(positionX, positionY + 1, heightMap.GetLength(0), heightMap.GetLength(1)))
            sample0 = heightMap[positionX, positionY + 1];
        
        float sample1 = 0;
        if (IsInsideArrayBounds2D(positionX + 1, positionY, heightMap.GetLength(0), heightMap.GetLength(1)))
            sample1 = heightMap[positionX + 1, positionY];

        float sample2 = 0;
        if (IsInsideArrayBounds2D(positionX, positionY - 1, heightMap.GetLength(0), heightMap.GetLength(1)))
            sample2 = heightMap[positionX, positionY - 1];
        
        float sample3 = 0;
        if (IsInsideArrayBounds2D(positionX - 1, positionY, heightMap.GetLength(0), heightMap.GetLength(1)))
            sample3 = heightMap[positionX - 1, positionY];


        float x = (sample1 - sample3) / 2f;
        float y = (sample0 - sample2) / 2f;

        //rechts+1 links-1 = x = 1;
        //rechts-1 links+1 = x =-1;
        //(rechts+1 + links-1 * -1) / 2 = x = 1;
        //(rechts-1 + links+1 * -1) / 2 = x =-1;

        slope.length = x * x + y * y;
        slope.directionX = x / slope.length;
        slope.directionY = y / slope.length;


        return slope;
    }

    public class Slope2D
    {
        public float directionX;
        public float directionY;
        public float length;
    }

}
