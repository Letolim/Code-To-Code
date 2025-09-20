using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationBorderGenerator
{
    System.Random randomGen;

    public void GenerateMeshPoints(int seed, TerrainDataMap map, TerrainMapHeightLayer[] heightLayer, MapGenerationPointCloud pointCloud)
    {
        randomGen = new System.Random(seed);

        List<Vector2Int> startNodes = new List<Vector2Int>();
        bool[,] checkMap = new bool[map.width, map.height];
        Debug.Log("Start");
        Debug.Log(checkMap[0, 0]);
        //find starter node
        Debug.Log("Start");


        for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
            {
                if (checkMap[x, y] == false)
                {
                    int posX = x;
                    int posY = y + 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, map.width, map.height))
                        if (map.layerMap[x, y] != map.layerMap[posX, posY])//Border tile found
                        {
                            pointCloud.layerIndex.Add(map.layerMap[x, y]);
                            pointCloud.borderPoints.Add(new List<Vector2>());
                            pointCloud.vertices.Add(new List<Vector3>());
                            map.layers.Add(new bool[map.width, map.height]);

                            StartBorderAnt(x, y, map, pointCloud);
                            FillCheckMap(x, y, checkMap, map.layerMap, pointCloud, map.heightMap, map.layerHeightMap, heightLayer, map);
                            continue;
                        }

                    posX = x + 1;
                    posY = y;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, map.width, map.height))
                        if (map.layerMap[x, y] != map.layerMap[posX, posY])//Border tile found
                        {
                            pointCloud.layerIndex.Add(map.layerMap[x, y]);
                            pointCloud.borderPoints.Add(new List<Vector2>());
                            pointCloud.vertices.Add(new List<Vector3>());
                            map.layers.Add(new bool[map.width, map.height]);

                            StartBorderAnt(x, y, map, pointCloud);
                            FillCheckMap(x, y, checkMap, map.layerMap, pointCloud, map.heightMap, map.layerHeightMap, heightLayer, map);
                            continue;
                        }

                    posX = x;
                    posY = y - 1;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, map.width, map.height))
                        if (map.layerMap[x, y] != map.layerMap[posX, posY])//Border tile found
                        {
                            pointCloud.layerIndex.Add(map.layerMap[x, y]);
                            pointCloud.borderPoints.Add(new List<Vector2>());
                            pointCloud.vertices.Add(new List<Vector3>());
                            map.layers.Add(new bool[map.width, map.height]);

                            StartBorderAnt(x, y, map, pointCloud);
                            FillCheckMap(x, y, checkMap, map.layerMap, pointCloud, map.heightMap, map.layerHeightMap, heightLayer, map);
                            continue;
                        }

                    posX = x - 1;
                    posY = y;
                    if (GridTools.IsInsideArrayBounds2D(posX, posY, map.width, map.height))
                        if (map.layerMap[x, y] != map.layerMap[posX, posY])//Border tile found
                        {
                            pointCloud.layerIndex.Add(map.layerMap[x, y]);
                            pointCloud.borderPoints.Add(new List<Vector2>());
                            pointCloud.vertices.Add(new List<Vector3>());
                            map.layers.Add(new bool[map.width, map.height]);

                            StartBorderAnt(x, y, map, pointCloud);
                            FillCheckMap(x, y, checkMap, map.layerMap, pointCloud, map.heightMap, map.layerHeightMap, heightLayer, map);
                            continue;
                        }
                }
            }
    }


    public bool IsNextBorderNode(int nodePositionX, int nodePositionY, int checkedNodePositionX, int checkedNodePositionY, TerrainDataMap map)
    {
        bool isInside = GridTools.IsInsideArrayBounds2D(checkedNodePositionX, checkedNodePositionY, map.width, map.height);
        if (isInside && map.layerMap[nodePositionX, nodePositionY] != map.layerMap[checkedNodePositionX, checkedNodePositionY] || !isInside)
            return true;

        return false;
    }


    public void FillCheckMap(int startPosX, int startPosY, bool[,] map, int[,] layerMap, MapGenerationPointCloud pointCloud, float[,] heightMap, float[,] layerHeightMap, TerrainMapHeightLayer[] heightLayer, TerrainDataMap dataMap)
    {
        // bool openNodes = true;
        List<Vector2Int> openNodes = new List<Vector2Int>();
        int layer = layerMap[startPosX, startPosY];
        map[startPosX, startPosY] = true;
        openNodes.Add(new Vector2Int(startPosX, startPosY));

        while (openNodes.Count != 0)
        {
            int initialPosX = openNodes[0].x;
            int initialPosY = openNodes[0].y;

            int posX = initialPosX;
            int posY = initialPosY + 1;
            float pointSpawnProbabillity = heightLayer[layerMap[initialPosX, initialPosY]].pointCloudProbabillity;
            float pointNoiseMul = heightLayer[layerMap[initialPosX, initialPosY]].pointCloudNoiseHeight;

            if (posY < layerMap.GetLength(1))
                if (map[posX, posY] == false && layerMap[posX, posY] == layer)
                {
                    dataMap.layers[dataMap.layers.Count - 1][posX, posY] = true;
                    openNodes.Add(new Vector2Int(posX, posY));
                    if(randomGen.NextDouble() <= pointSpawnProbabillity)
                        pointCloud.vertices[pointCloud.vertices.Count -1].Add(new Vector3((float)posX + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance, layerHeightMap[posX,posY] * 3f + heightMap[posX, posY] * pointNoiseMul, (float)posY + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance));
                    map[posX, posY] = true;
                }

            posX = initialPosX + 1;
            posY = initialPosY;
            if (posX < layerMap.GetLength(0))
                if (map[posX, posY] == false && layerMap[posX, posY] == layer)
                {
                    dataMap.layers[dataMap.layers.Count - 1][posX, posY] = true;
                    openNodes.Add(new Vector2Int(posX, posY));
                    if (randomGen.NextDouble() <= pointSpawnProbabillity)
                        pointCloud.vertices[pointCloud.vertices.Count - 1].Add(new Vector3((float)posX + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance, layerHeightMap[posX, posY] * 3f + heightMap[posX, posY] * pointNoiseMul, (float)posY + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance));
                    map[posX, posY] = true;
                }

            posX = initialPosX;
            posY = initialPosY - 1;
            if (posY >= 0)
                if (map[posX, posY] == false && layerMap[posX, posY] == layer)
                {
                    dataMap.layers[dataMap.layers.Count - 1][posX, posY] = true;
                    openNodes.Add(new Vector2Int(posX, posY));
                    if (randomGen.NextDouble() <= pointSpawnProbabillity)
                        pointCloud.vertices[pointCloud.vertices.Count - 1].Add(new Vector3((float)posX + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance, layerHeightMap[posX, posY] * 3f + heightMap[posX, posY] * pointNoiseMul, (float)posY + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance));
                    map[posX, posY] = true;
                }

            posX = initialPosX - 1;
            posY = initialPosY;
            if (posX > 0)
                if (map[posX, posY] == false && layerMap[posX, posY] == layer)
                {
                    dataMap.layers[dataMap.layers.Count - 1][posX, posY] = true;
                    openNodes.Add(new Vector2Int(posX, posY));
                    if (randomGen.NextDouble() <= pointSpawnProbabillity)
                        pointCloud.vertices[pointCloud.vertices.Count - 1].Add(new Vector3((float)posX + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance, layerHeightMap[posX, posY] * 3f + heightMap[posX, posY] * pointNoiseMul, (float)posY + (float)randomGen.NextDouble() * pointCloud.avaragePointDistance));
                    map[posX, posY] = true;
                }
            openNodes.RemoveAt(0);
        }
    }

    private int antStartPositionX;
    private int antStartPositionY;
    private int antPositionX;
    private int antPositionY;
    private int antDirection;
    private float borderDistance = 0.5f;


    public void StartBorderAnt(int startPositionX, int startPositionY, TerrainDataMap map, MapGenerationPointCloud pointCloud)
    {
        antStartPositionX = startPositionX;
        antStartPositionY = startPositionY;
        antPositionX = startPositionX;
        antPositionY = startPositionY;
        //int checkedTilePositonX;
        //int checkedTilePositionY;
        int borderLayer = map.layerMap[startPositionX, startPositionY];
        int mapWidth = map.layerMap.GetLength(0);
        int mapHeight = map.layerMap.GetLength(1);
        antDirection = 0;
        List<Vector2> borderPositions = new List<Vector2>();
        int i = 0;
        while (true && i < 50000)
        {
            i++;

            if (antDirection == 0)//Check Top
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntStraightTile(antPositionX, antPositionY + 1, borderLayer, mapWidth, mapHeight, map.layerMap,pointCloud,borderLayer ,6 ,1 );

                if(skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;

            }

            if (antDirection == 1)//Check Top-Right
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntDiagonalTile(antPositionX + 1, antPositionY + 1, borderLayer, mapWidth, mapHeight, map.layerMap, antPositionX + 1, antPositionY, antPositionX, antPositionY + 1, pointCloud, borderLayer, 6, 2);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

            if (antDirection == 2)//Check Right
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntStraightTile(antPositionX + 1, antPositionY, borderLayer, mapWidth, mapHeight, map.layerMap, pointCloud, borderLayer, 0, 3);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

            if (antDirection == 3)//Check Bottom-Right
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntDiagonalTile(antPositionX + 1, antPositionY - 1, borderLayer, mapWidth, mapHeight, map.layerMap, antPositionX, antPositionY - 1, antPositionX + 1, antPositionY, pointCloud, borderLayer, 0, 4);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

            if (antDirection == 4)//Check Bottom
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntStraightTile(antPositionX, antPositionY - 1, borderLayer, mapWidth, mapHeight, map.layerMap, pointCloud, borderLayer, 2, 5);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

            if (antDirection == 5)//Check Bottom-Left
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntDiagonalTile(antPositionX - 1, antPositionY - 1, borderLayer, mapWidth, mapHeight, map.layerMap, antPositionX - 1, antPositionY, antPositionX, antPositionY - 1, pointCloud, borderLayer, 2, 6);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

            if (antDirection == 6)//Check Left
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntStraightTile(antPositionX - 1, antPositionY, borderLayer, mapWidth, mapHeight, map.layerMap, pointCloud, borderLayer, 4, 7);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

            if (antDirection == 7)//Check Top-Left
            {
                int skipOrBreak = -1;
                skipOrBreak = CheckBorderAntDiagonalTile(antPositionX - 1, antPositionY + 1, borderLayer, mapWidth, mapHeight, map.layerMap, antPositionX, antPositionY + 1, antPositionX - 1, antPositionY, pointCloud, borderLayer, 2, 6);

                if (skipOrBreak == 0)
                    continue;
                if (skipOrBreak == 1)
                    break;
            }

        }
        float scale = 0.0390625f;

        //for (int n = 1; n < borderPositions.Count; n++)
            //Debug.DrawLine(new Vector3((float)borderPositions[n].x * scale, terrainMapHeightLayers[borderLayer].height * scale, (float)borderPositions[n].y * scale), new Vector3((float)borderPositions[n - 1].x * scale, terrainMapHeightLayers[borderLayer].height * scale, (float)borderPositions[n - 1].y * scale), terrainMapHeightLayers[borderLayer].color, 60f);
    }




    // topright         bottomright     bottomleft      topleft
    // #31              ###             ###             12#  
    // ##2              ##3             2##             3##
    // ###              #21             13#             ### 
    public bool CheckBorderAntDiagonalTile(int checkedTilePositonX, int checkedTilePositionY, int borderLayer, int mapWidth, int mapHeight, int[,] layerMap, int secondTileToCheckPosX, int secondTileToCheckPosY, int thirdTileToCheckPosX, int thirdTileToCheckPosY)
    {
        bool tileIsValidBorder = false;
        if (GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
        {
            if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                return true;

        }
        else
            return true;

        if (GridTools.IsInsideArrayBounds2D(secondTileToCheckPosX, secondTileToCheckPosY, mapWidth, mapHeight))
        {
            if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                tileIsValidBorder = true;
        }
        else
            tileIsValidBorder = true;

        if (tileIsValidBorder)
        {
            tileIsValidBorder = false;
            if (GridTools.IsInsideArrayBounds2D(thirdTileToCheckPosX, thirdTileToCheckPosY, mapWidth, mapHeight))
            {
                if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                    tileIsValidBorder = true;
            }
            else
                tileIsValidBorder = true;
        }

        return tileIsValidBorder;
    }

    public int CheckBorderAntDiagonalTile(int checkedTilePositonX, int checkedTilePositionY, int borderLayer, int mapWidth, int mapHeight, int[,] layerMap, int secondTileToCheckPosX, int secondTileToCheckPosY, int thirdTileToCheckPosX, int thirdTileToCheckPosY,MapGenerationPointCloud pointClod,int layerIndex,int possibleDirectionA,int possibleDirectionB)
    {
        bool tileIsValidBorder = false;
        int result = -1;//-1 check next 0 = continue 1 = break;
        if (GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
        {
            if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                tileIsValidBorder = true;

        }
        else
            tileIsValidBorder = true;

        if (!tileIsValidBorder && GridTools.IsInsideArrayBounds2D(secondTileToCheckPosX, secondTileToCheckPosY, mapWidth, mapHeight))
        {
            if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                tileIsValidBorder = true;
        }
        else
            tileIsValidBorder = true;

        if (tileIsValidBorder)
        {
            tileIsValidBorder = false;
            if (GridTools.IsInsideArrayBounds2D(thirdTileToCheckPosX, thirdTileToCheckPosY, mapWidth, mapHeight) && GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
            {
                if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                    tileIsValidBorder = true;
            }
            else
                tileIsValidBorder = true;
        }

        if (!tileIsValidBorder)
        {

            pointClod.borderPoints[pointClod.borderPoints.Count -1].Add(new Vector2((float)antPositionX - borderDistance, (float)antPositionY + borderDistance));//special case ant moves diagonal
            if (GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
            {
                antPositionX = checkedTilePositonX;
                antPositionY = checkedTilePositionY;
                if (antPositionX == antStartPositionX && antPositionY == antStartPositionY)
                    result = 1;//break
            }
            antDirection = possibleDirectionA;
            result = 0;//continue
        }
        else
        {
            pointClod.borderPoints[pointClod.borderPoints.Count - 1].Add(new Vector2((float)antPositionX - borderDistance, (float)antPositionY + borderDistance));//Add border dir 1
                                                                                                                         //Debug.DrawLine(new Vector3((float)antPositionX * .01f, 2.5f, (float)antPositionY * .01f), new Vector3(((float)antPositionX + borderDistance) * .01f, 2.5f, ((float)antPositionY - borderDistance) * .01f), Color.yellow, 60f);
            antDirection = possibleDirectionB;
        }

        return result;
    }

    public int CheckBorderAntStraightTile(int checkedTilePositonX, int checkedTilePositionY, int borderLayer, int mapWidth, int mapHeight, int[,] layerMap, MapGenerationPointCloud pointClod, int layerIndex, int possibleDirectionA, int possibleDirectionB)
    {
        bool tileIsValidBorder = false;
        int result = -1;//-1 check next 0 = continue 1 = break;

        if (GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
        {
            if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                tileIsValidBorder = true;
        }
        else
            tileIsValidBorder = true;

        if (!tileIsValidBorder)
        {
            if (GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
            {
                antPositionX = checkedTilePositonX;
                antPositionY = checkedTilePositionY;
                if (antPositionX == antStartPositionX && antPositionY == antStartPositionY)
                    result = 1;
            }
            antDirection = possibleDirectionA;
            result = 0;
        }
        else
        {
            pointClod.borderPoints[pointClod.borderPoints.Count - 1].Add(new Vector2((float)antPositionX - borderDistance, (float)antPositionY));//Add border dir 2
                                                                                                        //Debug.DrawLine(new Vector3((float)antPositionX * .01f, 2.5f, (float)antPositionY * .01f), new Vector3(((float)antPositionX - borderDistance) * .01f, 2.5f, (float)antPositionY * .01f), Color.cyan, 60f);
            antDirection = possibleDirectionB;
        }

        return result;
    }

    public bool CheckBorderAntStraightTile(int checkedTilePositonX, int checkedTilePositionY, int borderLayer, int mapWidth, int mapHeight, int[,] layerMap)
    {
        bool tileIsValidBorder = false;
        if (GridTools.IsInsideArrayBounds2D(checkedTilePositonX, checkedTilePositionY, mapWidth, mapHeight))
        {
            if (layerMap[checkedTilePositonX, checkedTilePositionY] != borderLayer)
                tileIsValidBorder = true;
        }
        else
            tileIsValidBorder = true;

        return tileIsValidBorder;

    }
}


//posX = initialPosX + 1;
//posY = initialPosY + 1;
//if (posX < layerMap.GetLength(0) && posY < layerMap.GetLength(1))
//    if (map[posX, posY] == false && layerMap[posX, posY] == layer)
//    {
//        openNodes.Add(new Vector2Int(posX, posY));
//        map[posX, posY] = true;
//    }

//posX = initialPosX + 1;
//posY = initialPosY - 1;
//if (posX < layerMap.GetLength(0) && posY > 0)
//    if (map[posX, posY] == false && layerMap[posX, posY] == layer)
//    {
//        openNodes.Add(new Vector2Int(posX, posY));
//        map[posX, posY] = true;
//    }

//posX = initialPosX - 1;
//posY = initialPosY - 1;
//if (posX > 0 && posY > 0)
//    if (map[posX, posY] == false && layerMap[posX, posY] == layer)
//    {
//        openNodes.Add(new Vector2Int(posX, posY));
//        map[posX, posY] = true;
//    }

//posX = initialPosX - 1;
//posY = initialPosY + 1;
//if (posX > 0 && posY < layerMap.GetLength(1))
//    if (map[posX, posY] == false && layerMap[posX, posY] == layer)
//    {
//        openNodes.Add(new Vector2Int(posX, posY));
//        map[posX, posY] = true;
//    }

//List<Vector2Int>[] borderLines = new List<Vector2Int>[startNodes.Count];
//for (int i = 0; i < startNodes.Count; i++)
//    borderLines[i] = new List<Vector2Int>();
//for (int x = 0; x < map.width; x++)
//    for (int y = 0; y < map.height; y++)
//        checkMap[x, y] = false;

//for (int i = 0; i < startNodes.Count; i++)
//{
//     Debug.DrawLine(new Vector3((float)startNodes[i].x * .3f, 0f, (float)startNodes[i].y * .3f), new Vector3((float)startNodes[i].x * .3f, 1f, (float)startNodes[i].y * .3f), Color.red,60f);

//    borderLines[i].Add(new Vector2Int(startNodes[i].x, startNodes[i].y));

//    checkMap[startNodes[i].x, startNodes[i].y] = true;
//    int n = 0;

//    while (true && n < 1024)
//    {
//        n++;
//        int nodePosX = borderLines[i][borderLines[i].Count - 1].x;
//        int nodePosY = borderLines[i][borderLines[i].Count - 1].y;

//        int possibleNodePosX = nodePosX;
//        int possibleNodePosY = nodePosY + 1;

//        if (GridTools.IsInsideArrayBounds2D(possibleNodePosX, possibleNodePosY, map.width, map.height) && checkMap[possibleNodePosX, possibleNodePosY] == false && map.layerMap[nodePosX, nodePosY] == map.layerMap[possibleNodePosX, possibleNodePosY])
//        {
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }

//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//        }

//        possibleNodePosX = nodePosX + 1;
//        possibleNodePosY = nodePosY;

//        if (GridTools.IsInsideArrayBounds2D(possibleNodePosX, possibleNodePosY, map.width, map.height) && checkMap[possibleNodePosX, possibleNodePosY] == false && map.layerMap[nodePosX, nodePosY] == map.layerMap[possibleNodePosX, possibleNodePosY])
//        {
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }

//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//        }

//        possibleNodePosX = nodePosX;
//        possibleNodePosY = nodePosY - 1;

//        if (GridTools.IsInsideArrayBounds2D(possibleNodePosX, possibleNodePosY, map.width, map.height) && checkMap[possibleNodePosX, possibleNodePosY] == false && map.layerMap[nodePosX, nodePosY] == map.layerMap[possibleNodePosX, possibleNodePosY])
//        {
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }

//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//        }

//        possibleNodePosX = nodePosX - 1;
//        possibleNodePosY = nodePosY;

//        if (GridTools.IsInsideArrayBounds2D(possibleNodePosX, possibleNodePosY, map.width, map.height) && checkMap[possibleNodePosX, possibleNodePosY] == false && map.layerMap[nodePosX, nodePosY] == map.layerMap[possibleNodePosX, possibleNodePosY])
//        {
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }

//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX + 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY - 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//            if (IsNextBorderNode(possibleNodePosX, possibleNodePosY, possibleNodePosX - 1, possibleNodePosY + 1, map))//Border tile found
//            {
//                borderLines[i].Add(new Vector2Int(possibleNodePosX, possibleNodePosY));
//                checkMap[possibleNodePosX, possibleNodePosY] = true;
//                if (borderLines[i][borderLines[i].Count - 1].x == borderLines[i][0].x && borderLines[i][borderLines[i].Count - 1].y == borderLines[i][0].y)//loop closed
//                    break;
//                else
//                    continue;
//            }
//        }
//    }
//    Debug.Log("done");
//}

//for (int i = 0; i < startNodes.Count; i++)
//{
//    Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
//    for(int n = 1; n < borderLines[i].Count; n ++)
//        Debug.DrawLine(new Vector3((float)borderLines[i][n].x * .01f, map.layerHeightMap[borderLines[i][n].x, borderLines[i][n].y], (float)borderLines[i][n].y * .01f), new Vector3((float)borderLines[i][n - 1].x * .01f, map.layerHeightMap[borderLines[i][n].x, borderLines[i][n].y], (float)borderLines[i][n - 1].y * .01f), color, 60f);
//}