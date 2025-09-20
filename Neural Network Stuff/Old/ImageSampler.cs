using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;


public class ImageSampler : MonoBehaviour
{
    public int steps = 25;
    int currentStep = 0;
    bool threadRunning = false;
    int sampleSizeX = 18;
    int sampleSizeY = 18;
    public bool sampleBW = false;
    int depth = 0;
    int resultDepth = 27;

    public Texture2D sampleImage;
    public GameObject unlitPlane;

    int imageWidth = 512;
    int imageHeight = 512;
    float[,,] imageArray;
    NetworkLayout[] networkLayout;
    System.Random randomGen;

    //progress
    public TextMesh textMeshP;
    float stepP = 0;
    float currentP = 0;
    string textP = "";

    float minR = 50000;
    float maxR = -50000;
    float avarageR = 0;

    float minG = 50000;
    float maxG = -50000;
    float avarageG = 0;

    float minB = 50000;
    float maxB = -50000;
    float avarageB = 0;


    // Start is called before the first frame update
    void Start()
    {
        randomGen = new System.Random(System.DateTime.Now.Millisecond);
        networkLayout = new NetworkLayout[1];


        if (sampleBW)
        {
            networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { sampleSizeX * sampleSizeY + 3, 128, 64, 32, 16, 1 });
            //networkLayout[1] = new NetworkLayout(Neurode.NeurodeType.MemoryNeurode, new int[] { 0, 0, 0, 64, 0, 0, 0, 0 });

            depth = 1;
        }
        else
        {
            networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { sampleSizeX * sampleSizeY * 3 + 9, 64, 64,64, resultDepth });
            //networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { sampleSizeX * sampleSizeY * 3 + 9, 16, 8, resultDepth });
            //networkLayout[1] = new NetworkLayout(Neurode.NeurodeType.MemoryNeurode, new int[] { 0, 32, 32, 32, 32, 32, 32, 0 });

            depth = 3;
        }

        NeuralNetwork neuralNetwork = new NeuralNetwork(networkLayout, randomGen.Next(0, 2094967296));
        threadNetwork = new NeuralNetwork(networkLayout, randomGen.Next(0, 2094967296));
        threadNetwork.clampValues = true;
        Texture2D result = new Texture2D(sampleImage.width, sampleImage.height, TextureFormat.ARGB32, false);
        threadImage = new Texture2D(sampleImage.width * 3, sampleImage.height * 3, TextureFormat.ARGB32, false);
        imageWidth = sampleImage.width;
        imageHeight = sampleImage.height;
        stepP = 100f / (float)(imageHeight * imageWidth);

        imageArray = new float[imageWidth, imageHeight, resultDepth];

        float valueCountR = 0;
        float valueCountG = 0;
        float valueCountB = 0;

        for (int x = 0; x < sampleImage.width; x++)
            for (int y = 0; y < sampleImage.height; y++)
            {
                if (depth != 1)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        if (z == 0)
                        {
                            imageArray[x, y, z] = sampleImage.GetPixel(x, y).r;
                            if (imageArray[x, y, z] >= .1f)
                            {
                                if (imageArray[x, y, z] > maxR)
                                    maxR = imageArray[x, y, z];
                                if (imageArray[x, y, z] < minR)
                                    minR = imageArray[x, y, z];
                                avarageR += imageArray[x, y, z];
                                valueCountR++;
                            }
                        }
                        if (z == 1)
                        {
                            imageArray[x, y, z] = sampleImage.GetPixel(x, y).g;
                            if (imageArray[x, y, z] >= .1f)
                            {
                                if (imageArray[x, y, z] > maxG)
                                    maxG = imageArray[x, y, z];
                                if (imageArray[x, y, z] < minG)
                                    minG = imageArray[x, y, z];
                                avarageG += imageArray[x, y, z];
                                valueCountG++;
                            }
                        }
                        if (z == 2)
                        {
                            imageArray[x, y, z] = sampleImage.GetPixel(x, y).b;
                            if (imageArray[x, y, z] >= .1f)
                            {
                                if (imageArray[x, y, z] > maxB)
                                    maxB = imageArray[x, y, z];
                                if (imageArray[x, y, z] < minB)
                                    minB = imageArray[x, y, z];
                                avarageB += imageArray[x, y, z];
                                valueCountB++;
                            }
                        }
                    }
                }
                else
                {
                    imageArray[x, y, 0] = sampleImage.GetPixel(x, y).grayscale;
                    valueCountR++;
                }

                //threadImage.SetPixel(x, y, sampleImage.GetPixel(x, y));
                //result.SetPixel(x, y, sampleImage.GetPixel(x, y));
            }

        avarageR /= valueCountR;
        avarageG /= valueCountG;
        avarageB /= valueCountB;

        //result.Apply();
        //threadImage.Apply();

        //SampleImage(sampleImage, neuralNetwork, result);
        //result.Apply();
        //SampleImage(result, neuralNetwork, result);
        //result.Apply();


        //unlitPlane.GetComponent<MeshRenderer>().material.mainTexture = result;


        //-------------------------------------------------------------
        brush = new float[brushSample.width, brushSample.height, 1];

        for (int x = 0; x < brushSample.width; x++)
            for (int y = 0; y < brushSample.height; y++)
                brush[x, y, 0] = brushSample.GetPixel(x, y).a;
    }





    private void SaveImage()
    {
        threadImage = new Texture2D(sampleImage.width * 3, sampleImage.height * 3, TextureFormat.ARGB32, false);

        if (depth == 1)
        {
            float min = 5000;
            float max = -5000;

            for (int x = 0; x < imageWidth; x++)
                for (int y = 0; y < imageHeight; y++)
                {
                    if (imageArray[x, y, 0] < min)
                        min = imageArray[x, y, 0];
                    if (imageArray[x, y, 0] > max)
                        max = imageArray[x, y, 0];
                }


            for (int x = 0; x < imageWidth; x++)
                for (int y = 0; y < imageHeight; y++)
                    imageArray[x, y, 0] = Mathf.InverseLerp(min, max, imageArray[x, y, 0]);

            for (int x = 0; x < sampleImage.width; x++)
                for (int y = 0; y < sampleImage.height; y++)
                    threadImage.SetPixel(x, y, new Color(imageArray[x, y, 0], imageArray[x, y, 0], imageArray[x, y, 0]));
            threadImage.Apply();
            FileStream stream = new FileStream(@"C:\Users\nikit\Desktop\Tilt Brush\sampleS_BW" + currentStep + ".png", FileMode.Create);
            stream.Write(threadImage.EncodeToPNG());
            stream.Close();
            stream.Dispose();

        }
        else
        {
            float min = 5000;
            float max = -5000;
            float[,,] image = new float[imageWidth * 3, imageHeight * 3, 3];

            for (int x = 0; x < imageWidth; x++)
                for (int y = 0; y < imageHeight; y++)
                    for (int z = 0; z < resultDepth; z++)
                    {
                        if (imageArray[x, y, z] < min)
                            min = imageArray[x, y, z];
                        if (imageArray[x, y, z] > max)
                            max = imageArray[x, y, z];
                    }

            for (int x = 0; x < sampleImage.width; x++)
                for (int y = 0; y < sampleImage.height; y++)
                {
                    int index = 0;

                    for (int xIn = 0; xIn < 3; xIn++)
                        for (int yIn = 0; yIn < 3; yIn++)
                        {
                            int posX = x * 3 + xIn;
                            int posY = y * 3 + yIn;

                            for (int z = 0; z < depth; z++)
                            {
                                image[posX, posY, z] = Mathf.InverseLerp(min, max, imageArray[x, y, index]);
                                index++;
                            }
                        }




                }

            for (int x = 0; x < sampleImage.width * 3; x++)
                for (int y = 0; y < sampleImage.height * 3; y++)
                {
                    threadImage.SetPixel(x, y, new Color(image[x, y, 0], image[x, y, 1], image[x, y, 2]));
                }
            //threadImage.SetPixel(posX, posY, new Color(image[x, y, index], image[x, y, index + 1], image[x, y, index + 2]));


            //int index = -1;
            //float val = -1f;

            //for (int i = 0; i < 3; i++)
            //    if (image[x, y, i] > val)
            //    {
            //        val = image[x, y, i];
            //        index = i;
            //    }

            //threadImage.SetPixel(x, y, new Color(image[x, y, index], image[x, y, index], image[x, y, index]));
            //}

            threadImage.Apply();
            FileStream stream = new FileStream(@"C:\Users\nikit\Desktop\Tilt Brush\sampleS_FS" + currentStep + ".png", FileMode.Create);
            stream.Write(threadImage.EncodeToPNG());
            stream.Close();
            stream.Dispose();

            return;

            float minR = 5000;
            float maxR = -5000;
            float minG = 5000;
            float maxG = -5000;
            float minB = 5000;
            float maxB = -5000;


            image = new float[imageWidth, imageHeight, 3];

            for (int x = 0; x < imageWidth; x++)
                for (int y = 0; y < imageHeight; y++)
                {
                    if (imageArray[x, y, 0] < minR)
                        minR = imageArray[x, y, 0];
                    if (imageArray[x, y, 0] > maxR)
                        maxR = imageArray[x, y, 0];

                    if (imageArray[x, y, 1] < minG)
                        minG = imageArray[x, y, 1];
                    if (imageArray[x, y, 1] > maxG)
                        maxG = imageArray[x, y, 1];

                    if (imageArray[x, y, 2] < minB)
                        minB = imageArray[x, y, 2];
                    if (imageArray[x, y, 2] > maxB)
                        maxB = imageArray[x, y, 2];
                }

            for (int x = 0; x < imageWidth; x++)
                for (int y = 0; y < imageHeight; y++)
                {
                    image[x, y, 0] = Mathf.InverseLerp(minR, maxR, imageArray[x, y, 0]);
                    image[x, y, 1] = Mathf.InverseLerp(minG, maxG, imageArray[x, y, 1]);
                    image[x, y, 2] = Mathf.InverseLerp(minB, maxB, imageArray[x, y, 2]);
                }


            for (int x = 0; x < sampleImage.width; x++)
                for (int y = 0; y < sampleImage.height; y++)
                    threadImage.SetPixel(x, y, new Color(image[x, y, 0], image[x, y, 1], image[x, y, 2]));

            threadImage.Apply();
            stream = new FileStream(@"C:\Users\nikit\Desktop\Tilt Brush\sampleS_SS" + currentStep + ".png", FileMode.Create);
            stream.Write(threadImage.EncodeToPNG());
            stream.Close();
            stream.Dispose();
        }




        //for (int x = 0; x < sampleImage.width; x++)
        //    for (int y = 0; y < sampleImage.height; y++)
        //    {
        //        if (depth != 1)
        //            threadImage.SetPixel(x, y, new Color(imageArray[x, y, 0], imageArray[x, y, 1], imageArray[x, y, 2]));
        //        else
        //            threadImage.SetPixel(x, y, new Color(imageArray[x, y, 0], imageArray[x, y, 0], imageArray[x, y, 0]));
        //    }
        //threadImage.Apply();
    }
    public Texture2D brushSample;
    private float[,,] brush;
    private float arcMultiplyer = 5f;
    private float distanceMultiplyer = 20f;

    public void DrawStroke(float centerX, float centerY, float r, float g, float b,float distance,float dirX,float dirY,float arcDelta,float[,,] image, float stepDistance)// float toX, float toY, float stepDistance)
    {
        float delta = (dirX + dirY) / 2f;
        float curDistance = distance;
        
        dirX -= .5f;
        dirY -= .5f;
        dirX *= delta;
        dirY *= delta;

        arcDelta *= arcMultiplyer;
        distance *= distanceMultiplyer;

        float posX = centerX + dirX * (distance / 2f);
        float posY = centerY + dirY * (distance / 2f);

        while(curDistance > 0)
        {
            posX += stepDistance * dirX + Mathf.Sin((180f * curDistance / distance) * Mathf.Deg2Rad) * arcDelta;
            posY += stepDistance * dirY;

            if (posX < 0 || posY < 0 || posX >= image.GetLength(0) || posY >= image.GetLength(1))
            {
                distance -= stepDistance;
                continue;
            }

            for (int x = 0; x < brush.GetLength(0); x++)
                for (int y = 0; y < brush.GetLength(1); y++)
                {
                    image[x + (int)posX, y + (int)posY, 0] = image[x + (int)posX, y + (int)posY, 0] * (1f - brush[x, y, 0]) + (brush[x, y, 0] * r);
                    image[x + (int)posX, y + (int)posY, 1] = image[x + (int)posX, y + (int)posY, 1] * (1f - brush[x, y, 0]) + (brush[x, y, 0] * g);
                    image[x + (int)posX, y + (int)posY, 2] = image[x + (int)posX, y + (int)posY, 2] * (1f - brush[x, y, 0]) + (brush[x, y, 0] * b);
                }

            curDistance -= stepDistance;
        }
    }


    // Update is called once per frame
    void Update()
    {
        textMeshP.text = textP;
        if (newImageComposed && !threadRunning)
        {
            currentP = 0;
            SaveImage();
            GameObject displayPlane = GameObject.Instantiate(unlitPlane);
            displayPlane.transform.position = Vector3.right * currentStep;
            displayPlane.GetComponent<MeshRenderer>().material.mainTexture = threadImage;
            newImageComposed = false;
            threadNetwork.SaveNetwork("sampleS" + currentStep);
        }
        if (currentStep <= steps && !threadRunning && !newImageComposed)
        {
            for (int x = 0; x < sampleImage.width; x++)
                for (int y = 0; y < sampleImage.height; y++)
                {
                    if (depth != 1)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            if (z == 0)
                                imageArray[x, y, z] = sampleImage.GetPixel(x, y).r;
                            if (z == 1)
                                imageArray[x, y, z] = sampleImage.GetPixel(x, y).g;
                            if (z == 2)
                                imageArray[x, y, z] = sampleImage.GetPixel(x, y).b;
                        }
                    }
                    else
                        imageArray[x, y, 0] = sampleImage.GetPixel(x, y).grayscale;
                }

            threadNetwork = new NeuralNetwork(networkLayout, randomGen.Next(0, 2094967296));


            Thread newTrainingThread = new Thread(new ThreadStart(SampleImageThread));
            newTrainingThread.Start();
            threadRunning = true;
            currentStep++;
        }
    }

    private NeuralNetwork threadNetwork;
    private Texture2D threadImage;
    private bool newImageComposed = false;

    public void SampleImageThread()
    {
        float[,,] result = new float[imageWidth, imageHeight, resultDepth];

        for (int x = 0; x < imageWidth; x++)
            for (int y = 0; y < imageHeight; y++)
            {
                Neurode[] outData = SampleBlock(x, y, threadNetwork, imageArray);

                for (int i = 0; i < outData.Length; i++)
                    result[x, y, i] = outData[i].Delta;

                currentP += stepP;
                textP = "" + currentP;
            }

        for (int x = 0; x < imageWidth; x++)
            for (int y = 0; y < imageHeight; y++)
            {
                if (sampleBW)
                {
                    float value = result[x, y, 0];
                    imageArray[x, y, 0] = value;
                    continue;
                }

                for (int z = 0; z < resultDepth; z++)
                {
                    imageArray[x, y, z] = result[x, y, z];
                }
            }

        threadRunning = false;
        newImageComposed = true;
    }



    public Texture2D SampleImage(Texture2D image, NeuralNetwork neuralNetwork, Texture2D resultingTexture)
    {
        float[,,] result = new float[image.width, image.height, depth];


        for (int x = sampleSizeX; x < image.width; x++)
            for (int y = sampleSizeY; y < image.height; y++)
                for (int z = 0; z < depth; z++)
                {
                    result[x, y, z] = SampleBlock(x, y, neuralNetwork, image, z);
                }

        float min = 5000;
        float max = -5000;

        for (int x = 0; x < image.width; x++)
            for (int y = 0; y < image.height; y++)
                for (int z = 0; z < depth; z++)
                {
                    if (result[x, y, z] < min)
                        min = result[x, y, z];
                    if (result[x, y, z] > max)
                        max = result[x, y, z];
                }


        for (int x = 0; x < image.width; x++)
            for (int y = 0; y < image.height; y++)
            {
                if (sampleBW)
                {
                    float value = Mathf.InverseLerp(min, max, result[x, y, 0]);
                    //float value = result[x, y, 0];

                    resultingTexture.SetPixel(x, y, new Color(value, value, value));
                    continue;
                }

                Color col = new Color();
                for (int z = 0; z < 3; z++)
                {
                    if (z == 0)
                        col.r = Mathf.InverseLerp(min, max, result[x, y, z]);
                    if (z == 1)
                        col.g = Mathf.InverseLerp(min, max, result[x, y, z]);
                    if (z == 2)
                        col.b = Mathf.InverseLerp(min, max, result[x, y, z]);
                }
                resultingTexture.SetPixel(x, y, col);

            }

        return resultingTexture;
    }

    public float SampleBlock(int posX, int posY, NeuralNetwork neuralNetwork, Texture2D sampleImage, int depth)
    {
        float[] values = new float[sampleSizeX * sampleSizeY * 3];
        int i = 0;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                values[i] = sampleImage.GetPixel(posX + x, posY + y).r;
                i++;
                values[i] = sampleImage.GetPixel(posX + x, posY + y).g;
                i++;
                values[i] = sampleImage.GetPixel(posX + x, posY + y).b;
                i++;
            }

        neuralNetwork.RunForward(values);

        return neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][depth].Delta;
    }


    public Neurode[] SampleBlock(int posX, int posY, NeuralNetwork neuralNetwork, float[,,] sampleImage)
    {
        float[] values = new float[sampleSizeX * sampleSizeY * 3 + 9];
        int i = 0;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                if (posX + x < 0 || posY + y < 0 || posX + x >= imageWidth || posY + y >= imageHeight)
                {
                    if (this.depth != 1)
                        i += 3;
                    else
                        i++;
                    continue;
                }
                if (this.depth != 1)
                {
                    values[i] = sampleImage[posX + x, posY + y, 0];
                    i++;
                    values[i] = sampleImage[posX + x, posY + y, 1];
                    i++;
                    values[i] = sampleImage[posX + x, posY + y, 2];
                    i++;
                }
                else
                {
                    values[i] = sampleImage[posX + x, posY + y, 0];
                    i++;
                }
            }


        if (depth != 1)
        {
            values[values.Length - 1] = avarageR;
            values[values.Length - 2] = avarageG;
            values[values.Length - 3] = avarageB;

            values[values.Length - 4] = minR;
            values[values.Length - 5] = maxR;

            values[values.Length - 6] = minG;
            values[values.Length - 7] = maxG;

            values[values.Length - 8] = minB;
            values[values.Length - 9] = maxB;
        }

        neuralNetwork.RunForward(values);

        return neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1];
    }















    public void SampleBlockBW(int posX, int posY, NeuralNetwork neuralNetwork, Texture2D sampleImage, Texture2D resultingImage)
    {
        float[] values = new float[sampleSizeX * sampleSizeY * 3];
        float[,] result = new float[sampleSizeX, sampleSizeY];
        int i = 0;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {

                values[i] = (sampleImage.GetPixel(posX + x, posY + y).r + sampleImage.GetPixel(posX + x, posY + y).g + sampleImage.GetPixel(posX + x, posY + y).b) / 3f;
                i++;
            }

        neuralNetwork.RunForward(values);

        i = 0;


        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                result[x, y] = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                i++;
            }

        float min = 5000;
        float max = -5000;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                if (result[x, y] < min)
                    min = result[x, y];
                if (result[x, y] > max)
                    max = result[x, y];
            }

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                float value = Mathf.InverseLerp(min, max, result[x, y]);
                resultingImage.SetPixel(x + posX, y + posY, new Color(value, value, value));

            }
    }

    public void SampleBlock(int posX, int posY, NeuralNetwork neuralNetwork, Texture2D sampleImage, Texture2D resultingImage)
    {
        float[] values = new float[sampleSizeX * sampleSizeY * 3];
        float[,,] result = new float[sampleSizeX, sampleSizeY, 3];
        int i = 0;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                values[i] = sampleImage.GetPixel(posX + x, posY + y).r;
                i++;
                values[i] = sampleImage.GetPixel(posX + x, posY + y).g;
                i++;
                values[i] = sampleImage.GetPixel(posX + x, posY + y).b;
                i++;
            }

        neuralNetwork.RunForward(values);

        i = 0;


        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                int n = 0;
                result[x, y, n] = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                n++;
                i++;
                result[x, y, n] = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                n++;
                i++;
                result[x, y, n] = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                n++;
                i++;

            }

        float min = 5000;
        float max = -5000;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
                for (int z = 0; z < 3; z++)
                {
                    if (result[x, y, z] < min)
                        min = result[x, y, z];
                    if (result[x, y, z] > max)
                        max = result[x, y, z];
                }

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                Color col = new Color();
                for (int z = 0; z < 3; z++)
                {
                    if (z == 0)
                        col.r = Mathf.InverseLerp(min, max, result[x, y, z]);
                    if (z == 1)
                        col.g = Mathf.InverseLerp(min, max, result[x, y, z]);
                    if (z == 2)
                        col.b = Mathf.InverseLerp(min, max, result[x, y, z]);
                }
                resultingImage.SetPixel(x + posX, y + posY, col);
            }
    }


    public void SampleBlocky(int posX, int posY, NeuralNetwork neuralNetwork, Texture2D sampleImage, Texture2D result)
    {
        float[] values = new float[sampleSizeX * sampleSizeY * 3];
        int i = 0;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                values[i] = sampleImage.GetPixel(posX + x, posY + y).r;
                i++;
                values[i] = sampleImage.GetPixel(posX + x, posY + y).g;
                i++;
                values[i] = sampleImage.GetPixel(posX + x, posY + y).b;
                i++;
            }

        neuralNetwork.RunForward(values);

        i = 0;
        float min = 5000;
        float max = -5000;

        for (int x = 0; x < sampleSizeX; x++)
            for (int y = 0; y < sampleSizeY; y++)
            {
                Color col = new Color();

                col.r = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                i++;
                col.g = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                i++;
                col.b = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][i].Delta;
                i++;


                result.SetPixel(x + posX, y + posY, col);

            }
        result.Apply();
    }
}
