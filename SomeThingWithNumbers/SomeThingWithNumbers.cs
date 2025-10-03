using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//https://de.wikipedia.org/wiki/Atomorbital
//https://de.wikipedia.org/wiki/Kugelfl%C3%A4chenfunktionen
//https://de.wikipedia.org/wiki/Zugeordnete_Legendrepolynome
//https://de.wikipedia.org/wiki/Wasserstoff

public class SomeThingWithNumbers : MonoBehaviour
{
    int a = 5;

    float x;
    float y;
    float z;

    int width = 64;
    int height = 64;

    public Texture2D texture;
    public float[,][] textureArray;

    public bool stopThread = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texture = new Texture2D(width, height,TextureFormat.RGB24,false);
        x = -a / 2;
        y = -a / 2;
        z = -a / 2;


        ThreadStart start = new ThreadStart(UpdateThread);
        Thread newThread = new Thread(start);
        newThread.Start();
    }


    //--------------------------------currently working on.8,493
    public void DoStuffThread(float px, float py, float pz)
    {
        Vector3 point = new Vector3(px, py, pz);

        float m = 2;

        float l = 1f - (px * px + py * py + pz * pz);

        //float d = Mathf.Sqrt(((2f * l + 1f) / 2f) * ((l - m) / (l + m))) / 20f;

        float d = Mathf.Sqrt(((2f * l + 1f) / 2f) * ((l - m) / (l + m)));
        d += 1;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {

                Vector3 screenpoint = new Vector3(((float)x / (float)width) * 6f - 3f, 10 , ((float)y / (float)height) * 6f - 3f);
                
                //float maginitude = Mathf.Sqrt(screenpoint.x * screenpoint.x + screenpoint.y * screenpoint.y + screenpoint.z * screenpoint.z);
  
                float distance = ((px - screenpoint.x) * (px - screenpoint.x) + (py - screenpoint.y) * (py - screenpoint.y) + (pz - screenpoint.z) * (pz - screenpoint.z));
                //float alpha = distance / (15f);

                //UnityEngine.Color color = texture.GetPixel(x, y);
                textureArray[x, y][0] += distance;
                textureArray[x, y][1] = 0;
                textureArray[x, y][2] += (1f - d) * distance;
            }
    }

    public void UpdateThread()
    {
        textureArray = new float[width, height][];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                textureArray[x, y] = new float[3];
            }

        while (!stopThread)
        {
            DoStuffThread(x, y, z);

            float g = .1f;

            x += g;

            if (x == x / 2 && y == a / 2 && z == a / 2)
                stopThread = true;

            if (x > a / 2)
            {
                y += g;
                x = -a / 2;

                if (y > a / 2)
                {
                    z += g;
                    y = -a / 2;
                }
            }
        }
    }
    bool done = false;
    // Update is called once per frame 
    void Update()
    {

        if (done)
            return;
        Vector2 position = Vector2.zero;


        for (int i = 0; i < 360; i++)
        {

            position = new Vector2(Mathf.Cos(i),Mathf.Sin(i));

            Debug.DrawLine(Vector3.zero, new Vector3(position.x,0,position.y),UnityEngine.Color.white * new Vector4(1,1,1,.4f),40f);

            float d = Mathf.Abs(position.x) + Mathf.Abs(position.y);

            

            position.y /= d;
            position.x /= d;
            Debug.Log(position);
            Debug.DrawLine(Vector3.up * .1f, new Vector3(position.x, 0, position.y) + Vector3.up * .1f, UnityEngine.Color.white * new Vector4(1, .2f, 1, .4f), 40f);

        }





        done = true;
        return;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            float min = 0f;
            float max = 0f;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (textureArray[x, y][0] < min)
                        min = textureArray[x, y][0];

                    if (textureArray[x, y][0] > max)
                        max = textureArray[x, y][0];
                }


            float minb = 0f;
            float maxb = 0f;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (textureArray[x, y][2] < minb)
                        minb = textureArray[x, y][2];

                    if (textureArray[x, y][2] > maxb)
                        maxb = textureArray[x, y][2];
                }

            float minc = 0f;
            float maxc = 0f;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (textureArray[x, y][1] < minc)
                        minc = textureArray[x, y][1];

                    if (textureArray[x, y][1] > maxc)
                        maxc = textureArray[x, y][1];
                }

            Debug.Log(minc + " " + maxc + " -- ");

            Debug.Log(textureArray[10, 10][0]);
            Debug.Log(textureArray[32, 32][0]);


            Debug.Log(x + " " + y + " " + z);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, new UnityEngine.Color(Mathf.InverseLerp(min, max, textureArray[x, y][0]), Mathf.InverseLerp(minc, maxc, textureArray[x, y][1]), Mathf.InverseLerp(minb, maxb, textureArray[x, y][2])));
                }

            texture.Apply();
        }

        return;
        //--------------------------------


        //-------------------------------- part that worked and produced the images front view and side view
        //DoStuff(x,y,z);
        //DoStuffB(x, y, z);
        //for (int x = 0; x < width; x++)
        //    for (int y = 0; y < height; y++)
        //    {

        //        if ((x < 10 || x > width - 10) && (x < 10 || x > width - 10))
        //        {
        //            Vector3 screenpoint = new Vector3(((float)x / (float)width) * 6f - 3f, 10, ((float)y / (float)width) * 6f - 3f);
        //            Debug.DrawLine(screenpoint, screenpoint + Vector3.down, UnityEngine.Color.white, 30);
        //        }
        //    }
        //float g = .25f;

        //x += g;
        //if (x == a / 2)
        //{
        //    y += g;
        //    x = -a / 2;


        //    if (y == a / 2)
        //    {
        //        z += g;
        //        if (z == a / 2)
        //        {
        //            x = -a / 2;
        //            y = -a / 2;
        //        }
        //        y = -a / 2;

        //        if (x == a / 2 && y == a / 2 && z == a / 2)
        //            return;
        //    }
        //}
    }
    public void DoStuff(float px, float py, float pz)// Mathf.sin(x a(0 - 1))
    {
        Vector3 point = new Vector3(x, y, z);
        float m = 2f;

        float l = 1f - (x * x + y * y + z * z) / (float)a;

        //float d = Mathf.Sqrt(((2f * l + 1f) / 2f) * ((l - m) / (l + m))) / 20f;
        float d = Mathf.Sqrt(((2f * l + 1f) / 2f) * ((l - m) / (l + m)));

        //Debug.DrawLine(Vector3.zero, new Vector3(x, y, z), new Color(1f - d, 0, d, .1f), 5);



        Debug.DrawLine(Vector3.zero, new Vector3(x, y, z).normalized * d, new UnityEngine.Color(1f - d, ((l - m) / (l + m)), d, .02f), 120);
        Debug.DrawLine(new Vector3(x, y, z).normalized * d, new Vector3(x, y, z).normalized * (a - d), new UnityEngine.Color(1f - d, d, 0, .02f), 120);




        Vector3 offset = new Vector3(30 + pz, 0, 0);



        Debug.DrawLine(offset, offset + new Vector3((l - m), d, (l + m)), new UnityEngine.Color(1f - d, ((l - m) / (l + m)), d, .025f), 120);







        return;
        //--------------------------------

    }

    int shells = 4;

    public void DoStuffB(float px, float py, float pz)// Mathf.sin(x a(0 - 1))
    {
        Vector3 point = new Vector3(x, y, z);
        float m = 2f;


        //float ax = Mathf.Sin(((px + a / 2) * ((px + (float)a / 2f)) / a) * 360f) * 2f;
        //float ay = Mathf.Cos((py * ((py + (float)a / 2f)) / a) * 360f) * 2f;
        //float az = (Mathf.Cos((px * ((px + (float)a / 2f)) / a) * 360f) + Mathf.Sin((pz * ((pz + (float)a / 2f)) / a) * 360f));



        float ax = Mathf.Sin( ((px + a /2) * (((px + a / 2) + (float)a / 2f)) / a) * 360f) * 2f;
        float ay = Mathf.Cos( ((py + a / 2) * (((py + a / 2) + (float)a / 2f)) / a) * 360f) * 2f;
        float az = (Mathf.Cos( ((px + a / 2) * (((px + a / 2) + (float)a / 2f)) / a) * 360f) + Mathf.Sin(((pz + a / 2) * (((pz + a / 2) + (float)a / 2f)) / a) * 360f));


        //->   <-

        //ax -
        float distance = new Vector3(px,py,pz).magnitude;

        float n = 0;
        float threshold = a / (float)shells;

        
        for(int i = shells; i != 0; i--)
        {
            if (distance > threshold * (float)i)
                n = i;
        }

        float l = new Vector3(ax, ay, az).sqrMagnitude * n;

        threshold *= shells;
        //float d = Mathf.Sqrt(((2f * l + 1f) / 2f) * ((l - m) / (l + m))) / 20f;
        float d = Mathf.Sqrt(((2f * l + 1f)) * ((l - m ) / (l + m )));

        //Debug.DrawLine(Vector3.zero, new Vector3(x, y, z), new Color(1f - d, 0, d, .1f), 5);



        Debug.DrawLine(Vector3.zero, new Vector3(x, y, z).normalized * d, new UnityEngine.Color(1f - d,  0, d, distance / threshold), 120);
        //Debug.DrawLine(new Vector3(x, y, z).normalized * d, new Vector3(x, y, z).normalized * (a - d), new UnityEngine.Color(1f - d, d, 0, .02f), 120);

        Vector3 offset = new Vector3(30 + (distance / a) , 0, 0);


        Debug.DrawLine(new Vector3(x, y, z).normalized * d, new Vector3(x, y, z).normalized * d + new Vector3(x, y, z).normalized * l, new UnityEngine.Color(d, 0, 1f - d, distance / threshold), 120);


        Debug.DrawLine(offset, offset +  new Vector3(ax, ay, az), new UnityEngine.Color(1f - d, ((l - (m / n)) / (l + (m / n))), d, .025f), 120);


        return;
        //--------------------------------

    }
}











//float n = .25f;

//x += n;
//if (x == a / 2)
//{
//    y += n;
//    x = -a / 2;


//    if (y == a / 2)
//    {
//        z += n;
//        if (z == a / 2)
//        {
//            x = -a / 2;
//            y = -a / 2;
//        }
//        y = -a / 2;

//        if (x == a / 2 && y == a / 2 && z == a / 2)
//            return;
//    }
//}

//x++;
//if (x == a / 2)
//{ 
//    y++;
//    x = -a / 2;


//    if (y == a / 2)
//    {
//        z++;
//        if(z == a / 2)
//        {
//            x = -a / 2;
//            y = -a / 2;
//        }
//        y = -a / 2;

//        if (x == a / 2 && y == a / 2 && z == a / 2)
//            return;
//    }
//}