using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshTest 
{
    private List<Vector3> vertices;
    private List<int> triangles;

    public float gridSize = .5f;


    //public void GenerateMesh()

    public Mesh[] GenerateMesh(MapGenerationPointCloud pointCloud)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        Mesh[] meshes = new Mesh[pointCloud.vertices.Count];

        for (int i = 0; i < pointCloud.vertices.Count; i++)
        {
            int triangleCount = 0;
            meshes[i] = new Mesh();
            vertices.Clear();
            triangles.Clear();
            for (int n = 0; n < pointCloud.vertices[i].Count; n++)
            {
                vertices.Add(new Vector3(-gridSize,0,-gridSize) + pointCloud.vertices[i][n]);  //0          //4
                vertices.Add(new Vector3(-gridSize, 0, gridSize) + pointCloud.vertices[i][n]); //1 //4      //5
                vertices.Add(new Vector3(gridSize, 0, -gridSize) + pointCloud.vertices[i][n]);//2 //3      //6
                vertices.Add(new Vector3(gridSize, 0, gridSize) + pointCloud.vertices[i][n]);    //5      //7
                
                triangles.Add(1 + triangleCount);
                triangles.Add(2 + triangleCount);
                triangles.Add(0 + triangleCount);


                triangles.Add(1 + triangleCount);
                triangles.Add(3 + triangleCount);
                triangles.Add(2 + triangleCount);

                triangleCount = vertices.Count;

            }
            meshes[i].vertices = vertices.ToArray();
            meshes[i].triangles = triangles.ToArray();
        }



        return meshes;
    }

    public Mesh[,] GenerateMesh(int[,] layerMap, int mapWidth, int mapHeight, int chunkCount,TerrainLayer[] terrainLayer, int[,] surflayerMap, SurfaceLayer[] surfaceLayers)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        int chunkSizeX = mapWidth / chunkCount;
        int chunkSizeY = mapHeight / chunkCount; // 128 / 16 = 8            

        List<Vector3>[,] verticeMap;
        List<int>[,] triangleMap;
        List<Vector2>[,] uvMap;

        PolygonQuad[,] quadMap = new PolygonQuad[mapWidth, mapHeight];

        int[,] TriangleCount = new int[chunkCount, chunkCount];
        verticeMap = new List<Vector3>[chunkCount, chunkCount];
        triangleMap = new List<int>[chunkCount, chunkCount];
        uvMap = new List<Vector2>[chunkCount, chunkCount];

        Mesh[,] meshes = new Mesh[chunkCount, chunkCount];

        for (int x = 0; x < chunkCount; x++)
            for (int y = 0; y < chunkCount; y++)
            {
                TriangleCount[x, y] = 0;
                verticeMap[x, y] = new List<Vector3>();
                triangleMap[x, y] = new List<int>();
                uvMap[x, y] = new List<Vector2>();
                meshes[x, y] = new Mesh();
            }

        int quadCount = mapWidth * mapHeight;
        System.Random randomGen = new System.Random(09292);

       for (int x = 0; x < mapWidth; x++)
            for (int y = 0; y < mapHeight; y++)
                quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, null, null);
                //quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, null,null,3f);

        for (int x = 0; x < mapWidth; x ++)
            for (int y = 0; y < mapHeight; y ++)
            {
                float meshDistortion = terrainLayer[layerMap[x,y]].meshDistortion;
                float heightDistortion = terrainLayer[layerMap[x,y]].heightDistortion;

                float offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                float offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                float height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x,y]].worldHeight;


                for (int n = 0; n < quadMap[x, y].linkedBottomLeft.Count; n++)
                    vertices[quadMap[x, y].linkedBottomLeft[n]] = vertices[quadMap[x, y].triangle2] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle2] += new Vector3(offsetX, height, offsetY);


                offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x, y]].worldHeight;
                for (int n = 0; n < quadMap[x, y].linkedTopLeft.Count; n++)
                    vertices[quadMap[x, y].linkedTopLeft[n]] = vertices[quadMap[x, y].triangle0] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle0] += new Vector3(offsetX, height, offsetY);


                offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x, y]].worldHeight;
                for (int n = 0; n < quadMap[x, y].linkedTopRight.Count; n++)
                    vertices[quadMap[x, y].linkedTopRight[n]] = vertices[quadMap[x, y].triangle4] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle4] += new Vector3(offsetX, height, offsetY);


                offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x, y]].worldHeight;
                for (int n = 0; n < quadMap[x, y].linkedBottomRight.Count; n++)
                    vertices[quadMap[x, y].linkedBottomRight[n]] = vertices[quadMap[x, y].triangle1] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle1] += new Vector3(offsetX, height, offsetY);

                vertices[quadMap[x, y].triangle3] = vertices[quadMap[x, y].triangle0];
                vertices[quadMap[x, y].triangle5] = vertices[quadMap[x, y].triangle1];
            }


        int chunkPosX;
        int chunkPosY;
        int tilePosX;
        int tilePosY;
        for (int i = 0; i < quadCount; i++)
        {
            tilePosX = i / mapWidth;
            tilePosY = i - tilePosX * mapWidth;
            chunkPosX = tilePosX / chunkSizeX;
            chunkPosY = tilePosY / chunkSizeY;
            //int triangleOffset = i * 6 - TriangleCount[chunkPosX, chunkPosY];
            int triangleOffset = quadMap[tilePosX, tilePosY].triangle0;

       

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle0]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle0 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle1]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle1 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle2]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle2 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle3]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle3 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle4]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle4 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle5]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle5 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));


            TriangleCount[chunkPosX, chunkPosY] += 6;
            //204135
           // TriangleCount[chunkPosX, chunkPosY] = meshes[chunkPosX, chunkPosY].triangles.Length;
            //i*0 012 123
            //6   678 459
            //12 //TCount - TcountCurrent( i *6) - TCountold
            //18
        }

        for (int i = 0; i < triangleMap[0, 0].Count; i++)
        {
            //Debug.Log(triangleMap[0, 0][i]);
        }

        for (int x = 0; x < chunkCount; x++)
            for (int y = 0; y < chunkCount; y++)
            {
                meshes[x, y].vertices = verticeMap[x, y].ToArray();
                meshes[x, y].triangles = triangleMap[x, y].ToArray();
                meshes[x, y].uv = uvMap[x, y].ToArray();

                meshes[x, y].Optimize();
                meshes[x, y].RecalculateNormals();
            }

        return meshes;


      
        //for (int i = 0; i < map.layers.Count; i++)
        //{

        //    bool[,] quadMapb = new bool[map.layers[i].GetLength(0), map.layers[i].GetLength(1)];
        //    for (int x = 0; x < map.layers[i].GetLength(0); x++)
        //        for (int y = 0; y < map.layers[i].GetLength(1); y++)
        //        {
        //            if (map.layers[i][x, y])
        //            {
        //                quadMapb[x, y] = true;
        //                quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, map, quadMapb);
        //            }
        //        }
        //    for (int x = 0; x < map.layers[i].GetLength(0); x ++)
        //        for (int y = 0; y < map.layers[i].GetLength(1); y ++)
        //        {
        //            if (quadMapb[x, y])
        //            {
        //                //float height = map.heightMap[x, y] * map.layerHeightMap[x, y] + map.layerHeightMap[x, y] / 3f;
        //                float height = 0;
        //                if (map.terrainLayer[map.layerMap[x, y]].layerName == "Mountain")
        //                    height = Random.Range(.1f, .2f) * 25 + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;
        //                else
        //                    if (map.terrainLayer[map.layerMap[x, y]].layerName == "Ground")
        //                    height = Random.Range(-.025f, .025f) + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;
        //                else
        //                    height = Random.Range(-.1f, .1f) + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;

        //                float offsetX = Random.Range(-.2f, .2f);
        //                float offsetY = Random.Range(-.2f, .2f);

        //                for (int n = 0; n < quadMap[x, y].linkedBottomLeft.Count; n++)
        //                    vertices[quadMap[x, y].linkedBottomLeft[n]] = vertices[quadMap[x, y].triangle2] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle2] += new Vector3(offsetX, height, offsetY);


        //                offsetX = Random.Range(-.2f, .2f);
        //                offsetY = Random.Range(-.2f, .2f);
        //                for (int n = 0; n < quadMap[x, y].linkedTopLeft.Count; n++)
        //                    vertices[quadMap[x, y].linkedTopLeft[n]] = vertices[quadMap[x, y].triangle0] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle0] += new Vector3(offsetX, height, offsetY);

        //                offsetX = Random.Range(-.2f, .2f);
        //                offsetY = Random.Range(-.2f, .2f);
        //                for (int n = 0; n < quadMap[x, y].linkedTopRight.Count; n++)
        //                    vertices[quadMap[x, y].linkedTopRight[n]] = vertices[quadMap[x, y].triangle4] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle4] += new Vector3(offsetX, height, offsetY);

        //                offsetX = Random.Range(-.2f, .2f);
        //                offsetY = Random.Range(-.2f, .2f);
        //                for (int n = 0; n < quadMap[x, y].linkedBottomRight.Count; n++)
        //                    vertices[quadMap[x, y].linkedBottomRight[n]] = vertices[quadMap[x, y].triangle1] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle1] += new Vector3(offsetX, height, offsetY);

        //                vertices[quadMap[x, y].triangle3] = vertices[quadMap[x, y].triangle0];
        //                vertices[quadMap[x, y].triangle5] = vertices[quadMap[x, y].triangle1];


        //                // 3 = 0       5 = 1
        //            }
        //        }

        //    for (int n = 0; n < vertices.Count; n++)
        //    {
        //        //vertices[n] += new Vector3(Random.Range(-.3f, .3f), 0, Random.Range(-.3f, .3f));
        //    }
        //    meshes[i].vertices = vertices.ToArray();
        //    meshes[i].triangles = triangles.ToArray();
        //    meshes[i].Optimize();
        //    meshes[i].RecalculateNormals();
        //}


        //return meshes;
    }

    public Mesh[,] GenerateMesh(int[,] layerMap, int mapWidth, int mapHeight, int chunkCount, TerrainLayer[] terrainLayer, int[,] surflayerMap, SurfaceLayer[] surfaceLayers, float[,] heightMap,GameObject[] treeTemplates,GameObject grassTemplate)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        int chunkSizeX = mapWidth / chunkCount;
        int chunkSizeY = mapHeight / chunkCount; // 128 / 16 = 8            

        List<Vector3>[,] verticeMap;
        List<int>[,] triangleMap;
        List<Vector2>[,] uvMap;

        PolygonQuad[,] quadMap = new PolygonQuad[mapWidth, mapHeight];

        int[,] TriangleCount = new int[chunkCount, chunkCount];
        verticeMap = new List<Vector3>[chunkCount, chunkCount];
        triangleMap = new List<int>[chunkCount, chunkCount];
        uvMap = new List<Vector2>[chunkCount, chunkCount];

        Mesh[,] meshes = new Mesh[chunkCount, chunkCount];

        for (int x = 0; x < chunkCount; x++)
            for (int y = 0; y < chunkCount; y++)
            {
                TriangleCount[x, y] = 0;
                verticeMap[x, y] = new List<Vector3>();
                triangleMap[x, y] = new List<int>();
                uvMap[x, y] = new List<Vector2>();
                meshes[x, y] = new Mesh();
            }

        int quadCount = mapWidth * mapHeight;
        System.Random randomGen = new System.Random(09292);

        for (int x = 0; x < mapWidth; x++)
            for (int y = 0; y < mapHeight; y++)
                quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, null, null);
        //quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, null,null,3f);

        for (int x = 0; x < mapWidth; x++)
            for (int y = 0; y < mapHeight; y++)
            {
                float meshDistortion = terrainLayer[layerMap[x, y]].meshDistortion;
                float heightDistortion = terrainLayer[layerMap[x, y]].heightDistortion;

                float offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                float offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                float height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + (heightMap[x, y] / 2f);

                for (int n = 0; n < quadMap[x, y].linkedBottomLeft.Count; n++)
                    vertices[quadMap[x, y].linkedBottomLeft[n]] = vertices[quadMap[x, y].triangle2] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle2] += new Vector3(offsetX, height, offsetY);


                offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x, y]].worldHeight;
                for (int n = 0; n < quadMap[x, y].linkedTopLeft.Count; n++)
                    vertices[quadMap[x, y].linkedTopLeft[n]] = vertices[quadMap[x, y].triangle0] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle0] += new Vector3(offsetX, height, offsetY);


                offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x, y]].worldHeight;
                for (int n = 0; n < quadMap[x, y].linkedTopRight.Count; n++)
                    vertices[quadMap[x, y].linkedTopRight[n]] = vertices[quadMap[x, y].triangle4] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle4] += new Vector3(offsetX, height, offsetY);


                offsetX = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                offsetY = MathTools.RandomDoubleToFloatRange(-meshDistortion, meshDistortion, randomGen.NextDouble());
                height = MathTools.RandomDoubleToFloatRange(-heightDistortion, heightDistortion, randomGen.NextDouble()) + terrainLayer[layerMap[x, y]].worldHeight;
                for (int n = 0; n < quadMap[x, y].linkedBottomRight.Count; n++)
                    vertices[quadMap[x, y].linkedBottomRight[n]] = vertices[quadMap[x, y].triangle1] + new Vector3(offsetX, height, offsetY);
                vertices[quadMap[x, y].triangle1] += new Vector3(offsetX, height, offsetY);

                vertices[quadMap[x, y].triangle3] = vertices[quadMap[x, y].triangle0];
                vertices[quadMap[x, y].triangle5] = vertices[quadMap[x, y].triangle1];



                //Tree test
                if (surfaceLayers[surflayerMap[x, y]].surfaceName == "Forest" && MathTools.RandomRange(0f, 1f) > .4f)
                {
                    int nextTree = MathTools.RandomRange(0, treeTemplates.Length);
                    GameObject newTree = GameObject.Instantiate(treeTemplates[nextTree]);
                    newTree.transform.position = (vertices[quadMap[x, y].triangle0] + vertices[quadMap[x, y].triangle1] + vertices[quadMap[x, y].triangle2] + vertices[quadMap[x, y].triangle4]) / 4f;
                    newTree.transform.Rotate(new Vector3(0,MathTools.RandomRange(0, 360),0));
                    newTree.transform.localScale = Vector3.one * MathTools.RandomRange(0.8f, 1f);
                }
                if (surfaceLayers[surflayerMap[x, y]].surfaceName == "Grass" && MathTools.RandomRange(0f, 1f) > .4f)
                {
                    GameObject newTree = GameObject.Instantiate(grassTemplate);
                    newTree.transform.position = (vertices[quadMap[x, y].triangle0] + vertices[quadMap[x, y].triangle1] + vertices[quadMap[x, y].triangle2] + vertices[quadMap[x, y].triangle4]) / 4f + new Vector3(0,0.2f,0);
                    newTree.transform.Rotate(new Vector3(0, MathTools.RandomRange(0, 360), 0));
                    newTree.transform.localScale = Vector3.one * MathTools.RandomRange(1.2f, 1.5f);
                }

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------

            }


        int chunkPosX;
        int chunkPosY;
        int tilePosX;
        int tilePosY;
        for (int i = 0; i < quadCount; i++)
        {
            tilePosX = i / mapWidth;
            tilePosY = i - tilePosX * mapWidth;
            chunkPosX = tilePosX / chunkSizeX;
            chunkPosY = tilePosY / chunkSizeY;
            //int triangleOffset = i * 6 - TriangleCount[chunkPosX, chunkPosY];
            int triangleOffset = quadMap[tilePosX, tilePosY].triangle0;



            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle0]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle0 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle1]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle1 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle2]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle2 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle3]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle3 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle4]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle4 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));

            verticeMap[chunkPosX, chunkPosY].Add(vertices[quadMap[tilePosX, tilePosY].triangle5]);
            //triangleMap[chunkPosX, chunkPosY].Add(quadMap[tilePosX, tilePosY].triangle5 - triangleOffset + TriangleCount[chunkPosX, chunkPosY]);
            triangleMap[chunkPosX, chunkPosY].Add(verticeMap[chunkPosX, chunkPosY].Count - 1);
            uvMap[chunkPosX, chunkPosY].Add(new Vector2(0.5f, (float)surfaceLayers[surflayerMap[tilePosX, tilePosY]].layerIndex / (float)surfaceLayers.Length));


            TriangleCount[chunkPosX, chunkPosY] += 6;
            //204135
            // TriangleCount[chunkPosX, chunkPosY] = meshes[chunkPosX, chunkPosY].triangles.Length;
            //i*0 012 123
            //6   678 459
            //12 //TCount - TcountCurrent( i *6) - TCountold
            //18
        }

        for (int i = 0; i < triangleMap[0, 0].Count; i++)
        {
            //Debug.Log(triangleMap[0, 0][i]);
        }

        for (int x = 0; x < chunkCount; x++)
            for (int y = 0; y < chunkCount; y++)
            {
                meshes[x, y].vertices = verticeMap[x, y].ToArray();
                meshes[x, y].triangles = triangleMap[x, y].ToArray();
                meshes[x, y].uv = uvMap[x, y].ToArray();

                meshes[x, y].Optimize();
                meshes[x, y].RecalculateNormals();
            }

        return meshes;



        //for (int i = 0; i < map.layers.Count; i++)
        //{

        //    bool[,] quadMapb = new bool[map.layers[i].GetLength(0), map.layers[i].GetLength(1)];
        //    for (int x = 0; x < map.layers[i].GetLength(0); x++)
        //        for (int y = 0; y < map.layers[i].GetLength(1); y++)
        //        {
        //            if (map.layers[i][x, y])
        //            {
        //                quadMapb[x, y] = true;
        //                quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, map, quadMapb);
        //            }
        //        }
        //    for (int x = 0; x < map.layers[i].GetLength(0); x ++)
        //        for (int y = 0; y < map.layers[i].GetLength(1); y ++)
        //        {
        //            if (quadMapb[x, y])
        //            {
        //                //float height = map.heightMap[x, y] * map.layerHeightMap[x, y] + map.layerHeightMap[x, y] / 3f;
        //                float height = 0;
        //                if (map.terrainLayer[map.layerMap[x, y]].layerName == "Mountain")
        //                    height = Random.Range(.1f, .2f) * 25 + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;
        //                else
        //                    if (map.terrainLayer[map.layerMap[x, y]].layerName == "Ground")
        //                    height = Random.Range(-.025f, .025f) + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;
        //                else
        //                    height = Random.Range(-.1f, .1f) + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;

        //                float offsetX = Random.Range(-.2f, .2f);
        //                float offsetY = Random.Range(-.2f, .2f);

        //                for (int n = 0; n < quadMap[x, y].linkedBottomLeft.Count; n++)
        //                    vertices[quadMap[x, y].linkedBottomLeft[n]] = vertices[quadMap[x, y].triangle2] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle2] += new Vector3(offsetX, height, offsetY);


        //                offsetX = Random.Range(-.2f, .2f);
        //                offsetY = Random.Range(-.2f, .2f);
        //                for (int n = 0; n < quadMap[x, y].linkedTopLeft.Count; n++)
        //                    vertices[quadMap[x, y].linkedTopLeft[n]] = vertices[quadMap[x, y].triangle0] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle0] += new Vector3(offsetX, height, offsetY);

        //                offsetX = Random.Range(-.2f, .2f);
        //                offsetY = Random.Range(-.2f, .2f);
        //                for (int n = 0; n < quadMap[x, y].linkedTopRight.Count; n++)
        //                    vertices[quadMap[x, y].linkedTopRight[n]] = vertices[quadMap[x, y].triangle4] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle4] += new Vector3(offsetX, height, offsetY);

        //                offsetX = Random.Range(-.2f, .2f);
        //                offsetY = Random.Range(-.2f, .2f);
        //                for (int n = 0; n < quadMap[x, y].linkedBottomRight.Count; n++)
        //                    vertices[quadMap[x, y].linkedBottomRight[n]] = vertices[quadMap[x, y].triangle1] + new Vector3(offsetX, height, offsetY);
        //                vertices[quadMap[x, y].triangle1] += new Vector3(offsetX, height, offsetY);

        //                vertices[quadMap[x, y].triangle3] = vertices[quadMap[x, y].triangle0];
        //                vertices[quadMap[x, y].triangle5] = vertices[quadMap[x, y].triangle1];


        //                // 3 = 0       5 = 1
        //            }
        //        }

        //    for (int n = 0; n < vertices.Count; n++)
        //    {
        //        //vertices[n] += new Vector3(Random.Range(-.3f, .3f), 0, Random.Range(-.3f, .3f));
        //    }
        //    meshes[i].vertices = vertices.ToArray();
        //    meshes[i].triangles = triangles.ToArray();
        //    meshes[i].Optimize();
        //    meshes[i].RecalculateNormals();
        //}


        //return meshes;
    }

    public Mesh[] GenerateMesh(TerrainDataMap map, int c)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        Mesh[] meshes = new Mesh[map.layers.Count];
        for (int i = 0; i < map.layers.Count; i++)
        {
            PolygonQuad[,] quadMap = new PolygonQuad[map.layers[i].GetLength(0), map.layers[i].GetLength(1)];
            meshes[i] = new Mesh();
            vertices.Clear();
            triangles.Clear();
            bool[,] quadMapb = new bool[map.layers[i].GetLength(0), map.layers[i].GetLength(1)];
            for (int x = 0; x < map.layers[i].GetLength(0); x++)
                for (int y = 0; y < map.layers[i].GetLength(1); y++)
                {
                    if (map.layers[i][x, y])
                    {
                        quadMapb[x, y] = true;
                        quadMap[x, y] = new PolygonQuad(x, y, vertices, triangles, quadMap, gridSize, map, quadMapb);
                    }
                }
            for (int x = 0; x < map.layers[i].GetLength(0); x ++)
                for (int y = 0; y < map.layers[i].GetLength(1); y ++)
                {
                    if (quadMapb[x, y])
                    {
                        //float height = map.heightMap[x, y] * map.layerHeightMap[x, y] + map.layerHeightMap[x, y] / 3f;
                        float height = 0;
                        if (map.terrainLayer[map.layerMap[x, y]].layerName == "Mountain")
                            height = Random.Range(.1f, .2f) * 25 + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;
                        else
                            if (map.terrainLayer[map.layerMap[x, y]].layerName == "Ground")
                            height = Random.Range(-.025f, .025f) + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;
                        else
                            height = Random.Range(-.1f, .1f) + map.layerHeightMap[x, y] + map.heightMap[x, y] * .25f;

                        float offsetX = Random.Range(-.2f, .2f);
                        float offsetY = Random.Range(-.2f, .2f);

                        for (int n = 0; n < quadMap[x, y].linkedBottomLeft.Count; n++)
                            vertices[quadMap[x, y].linkedBottomLeft[n]] = vertices[quadMap[x, y].triangle2] + new Vector3(offsetX, height, offsetY);
                        vertices[quadMap[x, y].triangle2] += new Vector3(offsetX, height, offsetY);


                        offsetX = Random.Range(-.2f, .2f);
                        offsetY = Random.Range(-.2f, .2f);
                        for (int n = 0; n < quadMap[x, y].linkedTopLeft.Count; n++)
                            vertices[quadMap[x, y].linkedTopLeft[n]] = vertices[quadMap[x, y].triangle0] + new Vector3(offsetX, height, offsetY);
                        vertices[quadMap[x, y].triangle0] += new Vector3(offsetX, height, offsetY);

                        offsetX = Random.Range(-.2f, .2f);
                        offsetY = Random.Range(-.2f, .2f);
                        for (int n = 0; n < quadMap[x, y].linkedTopRight.Count; n++)
                            vertices[quadMap[x, y].linkedTopRight[n]] = vertices[quadMap[x, y].triangle4] + new Vector3(offsetX, height, offsetY);
                        vertices[quadMap[x, y].triangle4] += new Vector3(offsetX, height, offsetY);

                        offsetX = Random.Range(-.2f, .2f);
                        offsetY = Random.Range(-.2f, .2f);
                        for (int n = 0; n < quadMap[x, y].linkedBottomRight.Count; n++)
                            vertices[quadMap[x, y].linkedBottomRight[n]] = vertices[quadMap[x, y].triangle1] + new Vector3(offsetX, height, offsetY);
                        vertices[quadMap[x, y].triangle1] += new Vector3(offsetX, height, offsetY);

                        vertices[quadMap[x, y].triangle3] = vertices[quadMap[x, y].triangle0];
                        vertices[quadMap[x, y].triangle5] = vertices[quadMap[x, y].triangle1];


                        // 3 = 0       5 = 1
                    }
                }

            for (int n = 0; n < vertices.Count; n++)
            {
                //vertices[n] += new Vector3(Random.Range(-.3f, .3f), 0, Random.Range(-.3f, .3f));
            }
            meshes[i].vertices = vertices.ToArray();
            meshes[i].triangles = triangles.ToArray();
            meshes[i].Optimize();
            meshes[i].RecalculateNormals();
        }


        return meshes;
    }

    private class PolygonQuad
    {
        public int topLeft;
        public int topRight;
        public int bottomLeft;
        public int bottomRight;

        public int triangle0 = -1;//top left
        public int triangle1 = -1;//bottom right 
        public int triangle2 = -1;//bottom left 

        public int triangle3 = -1;//top left
        public int triangle4 = -1;//top right
        public int triangle5 = -1;//bottom right 

        public List<int> linkedTopLeft;
        public List<int> linkedTopRight;
        public List<int> linkedBottomLeft;
        public List<int> linkedBottomRight;

        public PolygonQuad(int posX, int posY, List<Vector3> vertices, List<int> triangles, PolygonQuad[,] quadMap, float gridSize, TerrainDataMap dataMap, bool[,] map)
        {
            bool gotLeft = false;
            bool gotBottom = false;
            bool gotBottomLeft = false;

            linkedTopLeft = new List<int>();
            linkedTopRight = new List<int>();
            linkedBottomLeft = new List<int>();
            linkedBottomRight = new List<int>();



            if (GridTools.IsInsideArrayBounds2D(posX - 1, posY, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX - 1, posY] != null)
                gotLeft = true;
            if (GridTools.IsInsideArrayBounds2D(posX, posY - 1, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX, posY - 1] != null)
                gotBottom = true;
            if (GridTools.IsInsideArrayBounds2D(posX - 1, posY - 1, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX - 1, posY - 1] != null)
                gotBottomLeft = true;


            float maxPointOffset = 0.3f;
            //triangle0

            if (gotLeft)
            {
                linkedTopLeft.Add(quadMap[posX - 1, posY].triangle4);
                linkedBottomLeft.Add(quadMap[posX - 1, posY].triangle1);
                linkedBottomLeft.Add(quadMap[posX - 1, posY].triangle5);
            }

            if (gotBottom)
            {
                linkedBottomRight.Add(quadMap[posX, posY - 1].triangle4);
                linkedBottomLeft.Add(quadMap[posX, posY - 1].triangle0);
                linkedBottomLeft.Add(quadMap[posX, posY - 1].triangle3);
            }

            if (gotBottomLeft) 
                linkedBottomLeft.Add(quadMap[posX - 1, posY - 1].triangle4);

            

            vertices.Add(new Vector3(-gridSize + (float)posX, 0, gridSize + (float)posY));
            triangle0 = vertices.Count - 1;
            triangles.Add(triangle0);
            
            vertices.Add(new Vector3(gridSize + (float)posX, 0, -gridSize + (float)posY));
            triangle1 = vertices.Count - 1;
            triangles.Add(triangle1);

            //triangle2
            vertices.Add(new Vector3(-gridSize + (float)posX, 0, -gridSize + (float)posY));
            triangle2 = vertices.Count - 1;
            triangles.Add(triangle2);

            triangle3 = triangle0;
            triangles.Add(triangle3);


            //triangle4 //top right
            vertices.Add(new Vector3(gridSize + (float)posX, 0, gridSize + (float)posY));
            triangle4 = vertices.Count - 1;
            triangles.Add(triangle4);
            //triangle5 //bottom right 

            triangle5 = triangle1;
            triangles.Add(triangle5);


            if (gotLeft)
            {
                quadMap[posX - 1, posY].linkedTopRight.Add(triangle0);
                quadMap[posX - 1, posY].linkedTopRight.Add(triangle3);
                quadMap[posX - 1, posY].linkedBottomRight.Add(triangle2);
            }

            if (gotBottom)
            {
                quadMap[posX, posY - 1].linkedTopRight.Add(triangle1);
                quadMap[posX, posY - 1].linkedTopRight.Add(triangle5);
                quadMap[posX, posY - 1].linkedTopLeft.Add(triangle2);
            }

            if (gotBottomLeft)
                quadMap[posX - 1, posY - 1].linkedTopRight.Add(triangle2);
            
            return;

            if (GridTools.IsInsideArrayBounds2D(posX, posY - 1, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX, posY - 1] != null)
            {
                //1 = 4 | 2 = 0 | 5 = 4
                triangle1 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle1);
                triangle5 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle5);
                triangle2 = quadMap[posX, posY - 1].triangle0;
                triangles.Add(triangle2);
            }

            if (GridTools.IsInsideArrayBounds2D(posX - 1, posY, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX - 1, posY] != null)
            {
                //0 = 4| 2 = 1| 3 = 1
                triangle0 = quadMap[posX - 1, posY].triangle4;
                triangles.Add(triangle0);
                if (triangle2 == -1)
                {
                    triangle2 = quadMap[posX - 1, posY].triangle1;
                    triangles.Add(triangle2);
                }
                triangle3 = quadMap[posX - 1, posY].triangle1;
                triangles.Add(triangle3);
            }

            if (triangle0 == -1)//top left & 3
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, gridSize + (float)posY));
                triangle0 = vertices.Count - 1;
                triangles.Add(triangle0);
                triangle3 = vertices.Count - 1;
                triangles.Add(triangle3);
            }

            if (triangle1 == -1)//bottom right & 5
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle1 = vertices.Count - 1;
                triangles.Add(triangle1);
                triangle5 = vertices.Count - 1;
                triangles.Add(triangle5);
            }

            if (triangle2 == -1)// bottom left
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle2 = vertices.Count - 1;
                triangles.Add(triangle2);
            }

            if (triangle4 == -1)//top right
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, gridSize + (float)posY));
                triangle4 = vertices.Count - 1;
                triangles.Add(triangle2);
            }
            Debug.Log("t Count = " + triangles.Count);
        }

        public PolygonQuad(int posX, int posY, List<Vector3> vertices, List<int> triangles, PolygonQuad[,] quadMap, float gridSize, TerrainDataMap dataMap, bool[,] map,float d)
        {
            bool gotLeft = false;
            bool gotBottom = false;

            if (GridTools.IsInsideArrayBounds2D(posX - 1, posY, quadMap.GetLength(0), quadMap.GetLength(1)))
                gotLeft = false;
            if (GridTools.IsInsideArrayBounds2D(posX, posY - 1, quadMap.GetLength(0), quadMap.GetLength(1)))
                gotBottom = false;


            linkedTopLeft = new List<int>();
            linkedTopRight = new List<int>();
            linkedBottomLeft = new List<int>();
            linkedBottomRight = new List<int>();
            //triangle0
            if (gotLeft)
            {
                triangle0 = quadMap[posX - 1, posY].triangle4;
                triangles.Add(triangle0);
            }
            else
            {
                vertices.Add(new Vector3(-gridSize * Random.Range(0f, 5f) + (float)posX, 0, gridSize + (float)posY));
                triangle0 = vertices.Count - 1;
                triangles.Add(triangle0);
            }
            //triangle1 //bottom right 
            if (gotBottom)
            {
                triangle1 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle1);
            }
            else
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle1 = vertices.Count - 1;
                triangles.Add(triangle1);
            }
            //triangle2
            if (gotLeft)
            {
                triangle2 = quadMap[posX - 1, posY].triangle1;
                triangles.Add(triangle2);
            }
            else
               if (gotBottom)
            {
                triangle2 = quadMap[posX, posY - 1].triangle0;
                triangles.Add(triangle2);
            }
            else
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle2 = vertices.Count - 1;
                triangles.Add(triangle2);
            }
            //triangle3
            if (gotLeft)
            {
                triangle3 = quadMap[posX - 1, posY].triangle4;
                triangles.Add(triangle3);
            }
            else
            {
                triangle3 = triangle0;
                triangles.Add(triangle3);
            }
            //triangle4 //top right
            vertices.Add(new Vector3(gridSize + (float)posX, 0, gridSize + (float)posY));
            triangle4 = vertices.Count - 1;
            triangles.Add(triangle4);
            //triangle5 //bottom right 
            if (gotBottom)
            {
                triangle5 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle5);
            }
            else
            {
                triangle5 = triangle1;
                triangles.Add(triangle5);
            }


            return;

            if (GridTools.IsInsideArrayBounds2D(posX, posY - 1, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX, posY - 1] != null)
            {
                //1 = 4 | 2 = 0 | 5 = 4
                triangle1 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle1);
                triangle5 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle5);
                triangle2 = quadMap[posX, posY - 1].triangle0;
                triangles.Add(triangle2);
            }

            if (GridTools.IsInsideArrayBounds2D(posX - 1, posY, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX - 1, posY] != null)
            {
                //0 = 4| 2 = 1| 3 = 1
                triangle0 = quadMap[posX - 1, posY].triangle4;
                triangles.Add(triangle0);
                if (triangle2 == -1)
                {
                    triangle2 = quadMap[posX - 1, posY].triangle1;
                    triangles.Add(triangle2);
                }
                triangle3 = quadMap[posX - 1, posY].triangle1;
                triangles.Add(triangle3);
            }

            if (triangle0 == -1)//top left & 3
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, gridSize + (float)posY));
                triangle0 = vertices.Count - 1;
                triangles.Add(triangle0);
                triangle3 = vertices.Count - 1;
                triangles.Add(triangle3);
            }

            if (triangle1 == -1)//bottom right & 5
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle1 = vertices.Count - 1;
                triangles.Add(triangle1);
                triangle5 = vertices.Count - 1;
                triangles.Add(triangle5);
            }

            if (triangle2 == -1)// bottom left
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle2 = vertices.Count - 1;
                triangles.Add(triangle2);
            }

            if (triangle4 == -1)//top right
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, gridSize + (float)posY));
                triangle4 = vertices.Count - 1;
                triangles.Add(triangle2);
            }
            Debug.Log("t Count = " + triangles.Count);
        }


        public PolygonQuad(int posX,int posY, List<Vector3> vertices, List<int> triangles, PolygonQuad[,] quadMap,float gridSize, bool[,] map,int i)
        {

            linkedTopLeft = new List<int>();
            linkedTopRight = new List<int>();
            linkedBottomLeft = new List<int>();
            linkedBottomRight = new List<int>();

            if (GridTools.IsInsideArrayBounds2D(posX, posY - 1, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX, posY - 1] != null)
            {
                //1 = 4 | 2 = 0 | 5 = 4
                triangle1 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle1);
                triangle5 = quadMap[posX, posY - 1].triangle4;
                triangles.Add(triangle5);
                triangle2 = quadMap[posX, posY - 1].triangle0;
                triangles.Add(triangle2);
            }

            if (GridTools.IsInsideArrayBounds2D(posX - 1, posY, quadMap.GetLength(0), quadMap.GetLength(1)) && quadMap[posX - 1, posY] != null)
            {
                //0 = 4| 2 = 1| 3 = 1
                triangle0 = quadMap[posX - 1, posY].triangle4;
                triangles.Add(triangle0);
                if (triangle2 == -1)
                {
                    triangle2 = quadMap[posX - 1, posY].triangle1;
                    triangles.Add(triangle2);
                }
                triangle3 = quadMap[posX - 1, posY].triangle1;
                triangles.Add(triangle3);
            }

            if (triangle0 == -1)//top left & 3
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, gridSize + (float)posY));
                triangle0 = vertices.Count - 1;
                triangles.Add(triangle0);
                triangle3 = vertices.Count - 1;
                triangles.Add(triangle3);
            }

            if (triangle1 == -1)//bottom right & 5
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle1 = vertices.Count - 1;
                triangles.Add(triangle1);
                triangle5 = vertices.Count - 1;
                triangles.Add(triangle5);
            }

            if (triangle2 == -1)// bottom left
            {
                vertices.Add(new Vector3(-gridSize + (float)posX, 0, -gridSize + (float)posY));
                triangle2 = vertices.Count - 1;
                triangles.Add(triangle2);
            }

            if (triangle4 == -1)//top right
            {
                vertices.Add(new Vector3(gridSize + (float)posX, 0, gridSize + (float)posY)); 
                triangle4 = vertices.Count - 1;
                triangles.Add(triangle2);
            }
        }
    }


}
