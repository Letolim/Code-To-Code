using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;


public class PaintTest : MonoBehaviour
{
    public Texture2D brushSample;
    public GameObject canvasObject;
    public GameObject sampleObject;
    public Color canvasColor;
    public bool runn;
    public bool save;
    public bool setSample;
    public bool resetNetwork;

    private Texture2D canvas;
    private Texture2D sample;
    private int imageWidth;
    private int imageHeight;
    public int drawingWidth = 512;
    public int drawingHeight = 512;


    private float[,,] imageSample;
    private float[,,] image;
    private float[,,] brush;

    private NetworkLayout[] networkLayout;
    private System.Random randomGen;
    private NeuralNetwork neuralNetwork;
    private float[] values;
    private int imageOffset;
    private float drawStepDistance = .25f;
    private bool threadIsRunning = false;

    private int sampleSizeX = 6;
    private int sampleSizeY = 6;
    private int samplePosX = 1;
    private int samplePosY = 1;


    // Start is called before the first frame update
    void Start()
    {
        sample = sampleObject.transform.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        imageHeight = sample.width;
        imageWidth = sample.height;

        image = new float[drawingWidth, drawingHeight, 3];
        imageSample = new float[imageWidth, imageHeight, 3];

        brush = new float[brushSample.width, brushSample.height, 1];
        
        canvas = new Texture2D(drawingWidth, drawingHeight, TextureFormat.ARGB32, false);
       
        randomGen = new System.Random(System.DateTime.Now.Millisecond);
        networkLayout = new NetworkLayout[2];
        int valuesLength = (drawingWidth * drawingHeight * 3) + (sampleSizeX * sampleSizeY * 3);
        networkLayout[0] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { valuesLength, 256,128,64, 0 });
        networkLayout[1] = new NetworkLayout(Neurode.NeurodeType.TanNeurode, new int[] { 0, 0, 0, 0, 7 });

        neuralNetwork = new NeuralNetwork(networkLayout, System.DateTime.Now.Millisecond);
        imageOffset = imageWidth * imageHeight;
        values = new float[valuesLength];

        for (int x = 0; x < brushSample.width; x++)
            for (int y = 0; y < brushSample.height; y++)
                brush[x, y, 0] = brushSample.GetPixel(x, y).a;
        
        for (int x = 0; x < drawingWidth; x++)
            for (int y = 0; y < drawingHeight; y++)
            {
                canvas.SetPixel(x, y, canvasColor);
                image[x, y, 0] = canvasColor.r;
                image[x, y, 1] = canvasColor.g;
                image[x, y, 2] = canvasColor.b;
            }

         for (int x = 0; x < imageWidth; x++)
            for (int y = 0; y < imageHeight; y++)
                {
                    imageSample[x,y,0] = sample.GetPixel(x,y).r;
                    imageSample[x, y, 1] = sample.GetPixel(x, y).g;
                    imageSample[x, y, 2] = sample.GetPixel(x, y).b;
                }
 
        canvasObject.transform.GetComponent<MeshRenderer>().material.mainTexture = canvas;
    }

    public void SetSampleImage()
    {
        sample = sampleObject.transform.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        imageHeight = sample.width;
        imageWidth = sample.height;
        imageSample = new float[imageWidth, imageHeight, 3];
        imageOffset = imageWidth * imageHeight;

        for (int x = 0; x < imageWidth; x++)
            for (int y = 0; y < imageHeight; y++)
            {
                imageSample[x, y, 0] = sample.GetPixel(x, y).r;
                imageSample[x, y, 1] = sample.GetPixel(x, y).g;
                imageSample[x, y, 2] = sample.GetPixel(x, y).b;
            }
        for (int x = 0; x < drawingWidth; x++)
            for (int y = 0; y < drawingHeight; y++)
            {
                canvas.SetPixel(x, y, canvasColor);
                image[x, y, 0] = canvasColor.r;
                image[x, y, 1] = canvasColor.g;
                image[x, y, 2] = canvasColor.b;
            }
    }


    public void PaintThread()
    {
        while (threadIsRunning)
        {
            int i = 0;
            for (int x = 0; x < drawingWidth; x++)
                for (int y = 0; y < drawingHeight; y++)
                {
                    values[i] = image[x, y, 0];
                    i++;
                    values[i] = image[x, y, 1];
                    i++;
                    values[i] = image[x, y, 2];
                    i++;
                }

            for (int x = 0; x < sampleSizeX; x++)
                for (int y = 0; y < sampleSizeY; y++)
                {
                    if (samplePosX + x < 0 || samplePosX + y < 0 || samplePosX + x >= imageWidth || samplePosY + y >= imageHeight)
                    {
                        values[i] = 0;
                        i++;
                        values[i] = 0;
                        i++;
                        values[i] = 0;
                        i++;
                        continue;
                    }
                    values[i] = imageSample[samplePosX + x, samplePosY + y, 0];
                    i++;
                    values[i] = imageSample[samplePosX + x, samplePosY + y, 1];
                    i++;
                    values[i] = imageSample[samplePosX + x, samplePosY + y, 2];
                    i++;

                }
            Debug.Log(samplePosX + " posX");
            Debug.Log(samplePosY + " posY");

            samplePosX++;

            if (samplePosX >= imageWidth)
            {
                samplePosX = 0;
                samplePosY++;
                if (samplePosY >= imageHeight)
                    samplePosY = 0;
            }

            //values[imageOffset * 2 + 1] = drawSteps;
            neuralNetwork.RunForward(values);
         

            float min = 5000;
            float max = -5000;

            for (int n = 0; n < neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1].Length; n++)
                    {
                        if (neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][n].Delta < min)
                            min = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][n].Delta;
                        if (neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][n].Delta > max)
                            max = neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][n].Delta;
                    }
            for (int n = 0; n < neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1].Length; n++)
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][n].Delta = Mathf.InverseLerp(min, max, neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][n].Delta);
                        
            DrawStroke(neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][0].Delta ,
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][1].Delta ,
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][2].Delta ,
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][3].Delta ,
                drawStepDistance,
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][4].Delta ,
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][5].Delta ,
                neuralNetwork.neurodes[neuralNetwork.neurodes.Length - 1][6].Delta );
         }
    }

    void Update()
    {
        if (setSample)
        {
            SetSampleImage();
            setSample = false;
        }

        if (resetNetwork)
        {
            samplePosX = 1;
            samplePosY = 1;
            neuralNetwork.Respawn();
            resetNetwork = false;
        }

        if (runn && !threadIsRunning)
        {
            Thread newTrainingThread = new Thread(new ThreadStart(PaintThread));
            newTrainingThread.Start();
            threadIsRunning = true;
        }

        if (!runn && threadIsRunning)
            threadIsRunning = false;

        for (int x = 0; x < drawingWidth; x++)
            for (int y = 0; y < drawingHeight; y++)
            {
                Color col = new Color(image[x, y, 0], image[x, y, 1], image[x, y, 2]);
                canvas.SetPixel(x, y, col);
            }
 
        canvas.Apply();

        if (save)
        {
            FileStream stream = new FileStream(@"C:\Users\nikit\Desktop\Tilt Brush\samplePaint.png", FileMode.Create);
            stream.Write(canvas.EncodeToPNG());
            stream.Close();
            stream.Dispose();
        }
    }

    public void DrawStroke(float fromX, float fromY, float toX, float toY,float stepDistance, float r,float g,float b)
    {
        fromX = .1f + fromX * .8f;
        fromY = .1f + fromY * .8f;
        toX = .1f + toX * .8f;
        toY = .1f + toY * .8f;

        fromX *= (float)drawingWidth;
        fromY *= (float)drawingHeight;
        toX *= (float)drawingWidth;
        toY *= (float)drawingHeight;

        float posX = fromX;
        float posY = fromY;

        float dirX = (toX - fromX);
        float dirY = (toY - fromY);
        float delta = (dirX + dirY);

        dirX /= delta;
        dirY /= delta;
        if (posX > toX)
            delta += posX - toX;
        else
            delta += toX - posX;

        if (posY > toY)
            delta += posY - toY;
        else
            delta += toY - posY;

        delta /= 2f;
        int maxFS = (int)((float)drawingHeight / stepDistance);
        int fs = 0;

        while (delta > 5f)
        {
            fs++;
            if (fs > maxFS)
                break;

            delta = 0;
            for (int x = 0; x < brush.GetLength(0); x++)
                for (int y = 0; y < brush.GetLength(1); y++)
                {
                    if (posX + x < 0 || posY + y < 0 || posX + x >= drawingWidth || posY + y >= drawingHeight)
                        continue;

                    image[x + (int)posX, y + (int)posY, 0] = image[x + (int)posX, y + (int)posY, 0] * (1f - brush[x, y, 0]) + (brush[x, y,0] * r);
                    image[x + (int)posX, y + (int)posY, 1] = image[x + (int)posX, y + (int)posY, 1] * (1f - brush[x, y, 0]) + (brush[x, y, 0] * g);
                    image[x + (int)posX, y + (int)posY, 2] = image[x + (int)posX, y + (int)posY, 2] * (1f - brush[x, y, 0]) + (brush[x, y, 0] * b);
                }

            posX += dirX * stepDistance;
            posY += dirY * stepDistance;

            if (posX > toX) 
                delta += posX - toX;
            else
                delta += toX - posX;  

            if (posY > toY)
                delta += posY - toY;
            else
                delta += toY - posY;

            delta /= 2f;
        }
    }
}
