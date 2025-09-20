using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabrielGraph
{
    public List<Line> lines;

    public GabrielGraph()
    {
        lines = new List<Line>();
    }
    float scale = 0.125f;
    public void GenerateGraph(MapGenerationPointCloud pointCloud,float maxPointDistance,int mapWidth,int mapHeight)
    {
        int chunkCountX = (int)((float)mapWidth * scale);//0.0625f);0.03125 0.015625
        int chunkCountY = (int)((float)mapHeight * scale);
        List<GabrielPoint>[,] pointMatrix = new List<GabrielPoint>[chunkCountX,chunkCountY];
        lines = new List<Line>();

        for (int x = 0; x < chunkCountX; x ++)
            for (int y = 0; y < chunkCountY; y++)
                pointMatrix[x,y] = new List<GabrielPoint>();
        int index = 0;
        for (int i = 0; i < pointCloud.vertices.Count; i++)
            for (int n = 0; n < pointCloud.vertices[i].Count; n++)
            {
                pointMatrix[(int)(pointCloud.vertices[i][n].x * scale), (int)(pointCloud.vertices[i][n].z * scale)].Add(new GabrielPoint(pointCloud.vertices[i][n], pointCloud.layerIndex[i], index));
                index++;
            }


        for (int x = 0; x < chunkCountX; x++)
            for (int y = 0; y < chunkCountY; y++)
            {

                for (int i = 0; i < pointMatrix[x, y].Count; i++)
                {
                    int posX = x;
                    int posY = y;
                    int innerPosX = x;
                    int innerPosY = y;
                    if(GridTools.IsInsideArrayBounds2D(posX,posY,chunkCountX,chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x;
                    posY = y + 1;
                    innerPosX = x;
                    innerPosY = y + 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x + 1;
                    posY = y + 1;
                    innerPosX = x + 1;
                    innerPosY = y + 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x + 1;
                    posY = y;
                    innerPosX = x + 1;
                    innerPosY = y;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x + 1;
                    posY = y - 1;
                    innerPosX = x + 1;
                    innerPosY = y - 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x;
                    posY = y - 1;
                    innerPosX = x;
                    innerPosY = y - 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x - 1;
                    posY = y - 1;
                    innerPosX = x - 1;
                    innerPosY = y - 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x - 1;
                    posY = y;
                    innerPosX = x - 1;
                    innerPosY = y;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    posX = x - 1;
                    posY = y + 1;
                    innerPosX = x - 1;
                    innerPosY = y + 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, chunkCountX, chunkCountY))
                        CheckNext(posX, posY, x, y, innerPosX, innerPosY, pointMatrix, maxPointDistance, i);

                    continue;

                    for (int n = 0; n < pointMatrix[posX, posY].Count; n++)
                    {
                        if (n == i)
                            continue;

                        float distance = Vector3.Distance(pointMatrix[x, y][i].position, pointMatrix[posX, posY][n].position);
                        if (distance > maxPointDistance || pointMatrix[x, y][i].GotNeighbour(pointMatrix[posX, posY][n].index))
                            continue;

                        float halfDistance = distance / 2f;
                        Vector3 midPoint = Vector3.Lerp(pointMatrix[x, y][i].position, pointMatrix[posX, posY][n].position, 0.5f);

                        bool pointsAreGabriel = true;

                        for (int c = 0; c < pointMatrix[innerPosX,innerPosY].Count; c++)
                        {
                            if (c == i || c == n)
                                continue;

                            if (Vector3.Distance(midPoint, pointMatrix[innerPosX, innerPosY][c].position) < halfDistance)
                            {
                                pointsAreGabriel = false;
                                break;
                            }
                            //if distance < not gabriel
                        }

                        if (pointsAreGabriel)
                        {
                            pointMatrix[x, y][i].neighbourPoints.Add(n);
                            pointMatrix[posX, posY][n].neighbourPoints.Add(i);
                            lines.Add(new Line(pointMatrix[x, y][i].position, pointMatrix[posX, posY][n].position, pointMatrix[x, y][i].layer, pointMatrix[posX, posY][n].layer));
                            //Debug.DrawLine(gabrielPoints[i].position, gabrielPoints[n].position, Color.white, 30);
                        }

                    }
                }
            }
    }

    private void CheckNext(int posX, int posY, int x, int y, int innerPosX, int innerPosY, List<GabrielPoint>[,] pointMatrix, float maxPointDistance, int i)
    {
        for (int n = 0; n < pointMatrix[posX, posY].Count; n++)
        {
       
            float distance = Vector3.Distance(pointMatrix[x, y][i].position, pointMatrix[posX, posY][n].position);
            if (distance > maxPointDistance || pointMatrix[x, y][i].GotNeighbour(pointMatrix[posX, posY][n].index))
                continue;

            float halfDistance = distance / 2.1f;
            Vector3 midPoint = Vector3.Lerp(pointMatrix[x, y][i].position, pointMatrix[posX, posY][n].position, 0.5f);
            bool pointIsGabriel = true;

            for (int c = 0; c < pointMatrix[innerPosX, innerPosY].Count; c++)
            {
                if (c == n)
                    continue;

                if (Vector3.Distance(midPoint, pointMatrix[innerPosX, innerPosY][c].position) < halfDistance)
                {
                    pointIsGabriel = false;
                    break;
                }
                //if distance < not gabriel
            }

            if(pointIsGabriel)
                for (int c = 0; c < pointMatrix[x, y].Count; c++)
                {
                    if (c == i)
                        continue;

                    if (Vector3.Distance(midPoint, pointMatrix[x, y][c].position) < halfDistance)
                    {
                        pointIsGabriel = false;
                        break;
                    }
                    //if distance < not gabriel
                }

            if (pointIsGabriel)
            {
                pointMatrix[x, y][i].neighbourPoints.Add(pointMatrix[posX, posY][n].index);
                pointMatrix[posX, posY][n].neighbourPoints.Add(pointMatrix[x, y][i].index);
                lines.Add(new Line(pointMatrix[x, y][i].position, pointMatrix[posX, posY][n].position, pointMatrix[x, y][i].layer, pointMatrix[posX, posY][n].layer));
                //Debug.DrawLine(gabrielPoints[i].position, gabrielPoints[n].position, Color.white, 30);
            }
        }
    }
    public class Line
    {
        public Vector3 startPosition;
        public int layerA;
        public Vector3 endPosition;
        public int layerB;

        public Line(Vector3 startPosition, Vector3 endPosition, int layerA, int layerB)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.layerA = layerA;
            this.layerB = layerB;

        }
    }

    private class GabrielPoint
    {
        public Vector3 position;
        public List<int> neighbourPoints;
        public int layer;
        public int index;

        public GabrielPoint(Vector3 position) 
        {
            this.position = position;
            neighbourPoints = new List<int>();
        }
        public GabrielPoint(Vector3 position, int layer)
        {
            this.position = position;
            neighbourPoints = new List<int>();
            this.layer = layer;
        }
        public GabrielPoint(Vector3 position, int layer, int index)
        {
            this.index = index;
            this.position = position;
            neighbourPoints = new List<int>();
            this.layer = layer;
        }
        public bool GotNeighbour(int index)
        {
            for (int i = 0; i < neighbourPoints.Count; i++)
                if (neighbourPoints[i] == index)
                    return true;

            return false;
        }
    }

}
