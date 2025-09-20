using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RayCasterWIP : MonoBehaviour
{
    public RayLight[] lights;

    public Material material;

    private Texture2D texture;
    private int width;
    private int height;

    public int reflectionPases = 10;
    public int steps = 1;

    public bool useFiveRays = true;

    private int x = 0;
    private int y = 0;
    bool done = false;


    // Start is called before the first frame update
    void Start()
    {
        width = (int)Camera.main.pixelRect.width;
        height = (int)Camera.main.pixelRect.height;
        texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        material.mainTexture = texture;
        Debug.Log(System.DateTime.Now.Millisecond);
        e = new Vector2((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f);

        //for(float n = 3; n < 30; n += 3)
        //    for(int i = 0; i < 360; i += 1)
        //    {
        //        float b = (n  / 30f) * 834f;


        //        float a = Mathf.Sin((float)(((float)45 + i / 4)) * Mathf.Deg2Rad);

        //        i += (int)b;



        //        for (int o = 0; o < 13; o++)
        //        {
        //            Vector3 offset = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        //            Debug.DrawLine(offset + new Vector3(Mathf.Sin((float)i * Mathf.Deg2Rad) * a, 0, Mathf.Cos((float)i * Mathf.Deg2Rad) * .72f) * n,
        //                          Vector3.up + offset + new Vector3(Mathf.Sin((float)(i + 1) * Mathf.Deg2Rad) * a, 0, Mathf.Cos((float)(i + 1) * Mathf.Deg2Rad) * .72f) * n, UnityEngine.Color.cyan, 30f);

        //        }

        //        Debug.DrawLine( new Vector3(Mathf.Sin((float)i * Mathf.Deg2Rad) * a, 0, Mathf.Cos((float)i * Mathf.Deg2Rad) * .72f) * n , 
        //                        new Vector3(Mathf.Sin((float)(i + 1) * Mathf.Deg2Rad) * a, 0, Mathf.Cos((float)(i + 1) * Mathf.Deg2Rad) * .72f) * n, UnityEngine.Color.white,30f);
                
        //    }

    }

    Vector2 a = new Vector2(4.5f, 3f);
    Vector2 b = new Vector2(-2, 0);
    Vector2 c = new Vector2(6, 0);
    Vector2 d = new Vector2(5, -3);
    Vector2 e = new Vector2(5, -3);



    Vector3 planeNormal = Vector3.up;
    Vector3 position = new Vector3 (0,100,0);




    public void Plane(Vector3 pointA, Vector3 pointB, Vector3 pointC, bool draw)
    {
        DrawCross(pointA, UnityEngine.Color.blue);
        DrawCross(pointB, UnityEngine.Color.green);
        DrawCross(pointC, UnityEngine.Color.red);


        //Plane normal--------------------------------------------------------------
        Vector3 dirBA = pointB - pointA;
        Vector3 dirCA = pointC - pointA;

        Vector3 planeNormal = new Vector3(dirBA.y * dirCA.z - dirCA.y * dirBA.z,
                                   dirBA.z * dirCA.x - dirCA.z * dirBA.x,
                                   dirBA.x * dirCA.y - dirCA.x * dirBA.y);

        float magnitude = Mathf.Sqrt(planeNormal.x * planeNormal.x + planeNormal.y * planeNormal.y + planeNormal.z * planeNormal.z);

        planeNormal = new Vector3(planeNormal.x / magnitude, planeNormal.y / magnitude, planeNormal.z / magnitude);//forward
        //--------------------------------------------------------------------------

        Vector3 center = (pointA + pointB + pointC) / 3f;

        Debug.DrawLine(center, center + planeNormal, UnityEngine.Color.red, 15f);//direction
        Debug.DrawLine(center, center + planeNormal, UnityEngine.Color.red, 15f);//direction

        Vector3 dirAB = pointA - pointC;
        Vector3 dirAC = pointA - pointB;


        Vector3 pointOnPlane = pointA + dirCA.normalized;
        Vector3 pointAwayFromPlane = pointA + dirCA.normalized + planeNormal + Vector3.up * 1000f;

        Debug.DrawLine(pointB, pointB + dirBA * -10f, UnityEngine.Color.gray, 15f);//direction
        Debug.DrawLine(pointB, pointB + dirBA * 10f, UnityEngine.Color.gray, 15f);//direction

        Debug.DrawLine(pointC, pointC + dirCA * -10f, UnityEngine.Color.gray, 15f);//direction
        Debug.DrawLine(pointC, pointC + dirCA * 10f, UnityEngine.Color.gray, 15f);//direction


        Debug.DrawLine(pointB, pointB + dirAB * -10f, UnityEngine.Color.gray, 15f);//direction
        Debug.DrawLine(pointB, pointB + dirAB * 10f, UnityEngine.Color.gray, 15f);//direction

        Debug.DrawLine(pointC, pointC + dirAC * -10f, UnityEngine.Color.gray, 15f);//direction
        Debug.DrawLine(pointC, pointC + dirAC * 10f, UnityEngine.Color.gray, 15f);//direction




        if (draw)
        {
            for (float i = 0; i < 100; i += 5)
            {
                Debug.DrawLine(pointB + dirBA.normalized * i, pointB + dirBA.normalized * i - dirAB * 50f, UnityEngine.Color.magenta, 15f);//direction
                Debug.DrawLine(pointB + dirBA.normalized * i, pointB + dirBA.normalized * i + dirAB * 50f, UnityEngine.Color.magenta, 15f);//direction

                Debug.DrawLine(pointB + dirBA.normalized * -i, pointB + dirBA.normalized * -i - dirAB * 50f, UnityEngine.Color.magenta, 15f);//direction
                Debug.DrawLine(pointB + dirBA.normalized * -i, pointB + dirBA.normalized * -i + dirAB * 50f, UnityEngine.Color.magenta, 15f);//direction
            }

            for (float i = 0; i < 100; i += 5)
            {
                Debug.DrawLine((pointB + dirBA.normalized * i) + dirAB.normalized * i, (pointB + dirBA.normalized * i) + dirAB.normalized * i - dirBA * 50f, UnityEngine.Color.magenta, 15f);//direction
                Debug.DrawLine((pointB + dirBA.normalized * i) + dirAB.normalized * i, (pointB + dirBA.normalized * i) + dirAB.normalized * i + dirBA * 50f, UnityEngine.Color.magenta, 15f);//direction

                Debug.DrawLine((pointB + dirBA.normalized * -i) + dirAB.normalized * -i, (pointB + dirBA.normalized * -i) + dirAB.normalized * -i - dirBA * 50f, UnityEngine.Color.magenta, 15f);//direction
                Debug.DrawLine((pointB + dirBA.normalized * -i) + dirAB.normalized * -i, (pointB + dirBA.normalized * -i) + dirAB.normalized * -i + dirBA * 50f, UnityEngine.Color.magenta, 15f);//direction
            }


        }


        Debug.DrawLine(pointC, pointC + dirAC * -10f, UnityEngine.Color.gray, 15f);//direction
        Debug.DrawLine(pointC, pointC + dirAC * 10f, UnityEngine.Color.gray, 15f);//direction





        Debug.DrawLine(center + dirBA * 5, (planeNormal + Vector3.up) * 10f, UnityEngine.Color.red, 15f);//direction
        Debug.DrawLine(center + dirBA * 5, (planeNormal + Vector3.left) * 10f, UnityEngine.Color.green, 15f);//direction
        Debug.DrawLine(center + dirBA * 5, (planeNormal + Vector3.right) * 10f, UnityEngine.Color.blue, 15f);//direction
        Debug.DrawLine(center + dirBA * 5, (planeNormal + Vector3.down) * 10f, UnityEngine.Color.magenta, 15f);//direction
        Debug.DrawLine(center + dirBA * 5, (planeNormal + Vector3.up) * 10f, UnityEngine.Color.yellow, 15f);//direction
        Debug.DrawLine(center + dirBA * 5, (planeNormal + Vector3.forward) * 10f, UnityEngine.Color.cyan, 15f);//direction





        //Vector3 center = (pointB + pointC) / 3f;//point an plane



        Debug.Log(center);

        Debug.Log((center.normalized - planeNormal));
        Debug.Log((pointAwayFromPlane.normalized - planeNormal).sqrMagnitude);

        return;


 



        //Fmoi ------------------------------
        //Debug.DrawLine(new Vector3(13, 0, 0), new Vector3(13, 0, 0) + new Vector3(dirBA.x * dirCA.y, dirBA.y * dirCA.z, dirBA.z * dirCA.x).normalized, UnityEngine.Color.gray, 15f);
        Debug.DrawLine(center, center + new Vector3(dirBA.y * dirCA.z, dirBA.z * dirCA.x, dirBA.x * dirCA.y).normalized, UnityEngine.Color.gray, 15f);//direction
        DrawCross(center, UnityEngine.Color.red);
        //Debug.DrawLine(new Vector3(10,0,0), new Vector3(10, 0, 0) + new Vector3(dirCA.x * dirBA.y, dirCA.y * dirBA.z, dirCA.z * dirBA.x).normalized, UnityEngine.Color.gray, 15f);
        Debug.DrawLine(center, center + new Vector3(dirCA.y * dirBA.z, dirCA.z * dirBA.x, dirCA.x * dirBA.y).normalized, UnityEngine.Color.gray, 15f);//direction

        //Draw a cross oriented to the planes normal
        //forward 0 & 0 = x next plane
        //forward ? & ? = x next plane
        // ||
        //forward 0,0 = x,0 next plane
        //forward 0,0 = 0,x next plane

        //Thinking (shortcut?) no spoiler pls
        //yz zx xy 
        //zx yz xy 
        //xy yz zx 
        //ax + by + cz + d = 0
        //------------------------------------

        Debug.DrawLine(center, center + planeNormal.normalized, UnityEngine.Color.yellow, 15f);





 

        float distance = -(pointA.x * planeNormal.x + pointA.y * planeNormal.y + pointA.z * planeNormal.z);





        Debug.Log(planeNormal);


        pointA = ((pointA + dirCA * 30f) - (pointA + dirBA * 30f)) / 2f;







        float distanceAwayFromPlaneA = (planeNormal.x * center.x + planeNormal.y * center.y + planeNormal.z * center.z);

        float distanceAwayFromPlaneB = (planeNormal.x * pointAwayFromPlane.x + planeNormal.y * pointAwayFromPlane.y + planeNormal.z * pointAwayFromPlane.z);

        Debug.Log(distance + " distance");
        Debug.Log(distanceAwayFromPlaneA + distance + " distanceA");//>
        Debug.Log(distanceAwayFromPlaneB + distance + " distanceB");//<

        //Debug.Log(Vector3.Magnitude(new Vector3(planeNormal.x * pointAwayFromPlane.x, planeNormal.y * pointAwayFromPlane.y, planeNormal.z * pointAwayFromPlane.z)) + distance);

        //Debug.Log(Vector3.Cross(planeNormal, pointOnPlane - pointA));
                                                           

    }

    float size = .25f;

    public void DrawCross(Vector3 position, UnityEngine.Color color)
    {
        Debug.DrawLine(new Vector3(position.x, position.y - size, position.z), new Vector3(position.x, position.y + size, position.z), color);
        Debug.DrawLine(new Vector3(position.x - size, position.y, position.z), new Vector3(position.x + size, position.y, position.z), color);
        Debug.DrawLine(new Vector3(position.x, position.y, position.z - size), new Vector3(position.x, position.y, position.z + size), color);
    }

    public void Hmmm()
    {
        return;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            d = new Vector2(d.x  - 1, d.y);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            d = new Vector2(d.x + 1, d.y);


        if (Input.GetKeyDown(KeyCode.UpArrow))
            d = new Vector2(d.x , d.y - 1);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            d = new Vector2(d.x, d.y + 1);

        float dotA = Vector2.Dot(a, d);
        float dotB = Vector2.Dot(b, d);
        float dotC = Vector2.Dot(c, d);
        float dotE = Vector2.Dot(e, d);



        Debug.Log("dotA = " + dotA);
        Debug.Log("dotB = " + dotB);
        Debug.Log("dotC = " + dotC);
        Debug.Log("dotE = " + dotE);


































        //Debug.Log("dotA = " + dotA);
        //Debug.Log("dotB = " + dotB);
        //Debug.Log("dotC = " + dotC);

        float magA = a.magnitude;
        float magB = b.magnitude;
        float magC = c.magnitude;
        float magE = e.magnitude;


        //Debug.Log("magA = " + magA);
        //Debug.Log("magB = " + magB);
        //Debug.Log("magC = " + magC);

        float cosinA = dotA / (magA * d.magnitude);
        float cosinB = dotB / (magB * d.magnitude);
        float cosinC = dotC / (magC * d.magnitude);
        float cosinE = dotE / (magE * d.magnitude);

    
        Vector3.Cross(a, b).Normalize();

        if (cosinE > -.09 && cosinE < .1f)
            Debug.Log("!");

        Debug.DrawLine(new Vector3(a.x, 0, a.y), new Vector3(Mathf.Asin(cosinA) * 5f, 0, Mathf.Acos(cosinA)) * 5f + new Vector3(a.x, 0, a.y), UnityEngine.Color.red, 1);
        Debug.DrawLine(new Vector3(b.x, 0, b.y), new Vector3(Mathf.Asin(cosinB) * 5f, 0, Mathf.Acos(cosinB)) * 5f + new Vector3(b.x, 0, b.y), UnityEngine.Color.green, 1f);
        Debug.DrawLine(new Vector3(c.x, 0, c.y), new Vector3(Mathf.Asin(cosinC) * 5f, 0, Mathf.Acos(cosinC)) * 5f + new Vector3(c.x, 0, c.y), UnityEngine.Color.blue, 1f);

        Debug.DrawLine(new Vector3(e.x, 0, e.y), new Vector3(Mathf.Asin(cosinE) * 5f, 0, Mathf.Acos(cosinE)) * 5f + new Vector3(e.x, 0, e.y), UnityEngine.Color.magenta, 1f);

        Debug.DrawLine(new Vector3(d.x, 0, d.y), new Vector3(d.x, 0, d.y) + Vector3.up, UnityEngine.Color.white, 1f);
        Debug.DrawLine(new Vector3(e.x, 0, e.y), new Vector3(e.x, 0, e.y) + Vector3.up, UnityEngine.Color.white, 1);
        
        Debug.DrawLine(new Vector3(a.x, 0, a.y), new Vector3(a.x, 0, a.y) + Vector3.up, UnityEngine.Color.red, 1f);
        Debug.DrawLine(new Vector3(b.x, 0, b.y), new Vector3(b.x, 0, b.y) + Vector3.up, UnityEngine.Color.green, 1f);
        Debug.DrawLine(new Vector3(c.x, 0, c.y), new Vector3(c.x, 0, c.y) + Vector3.up, UnityEngine.Color.blue, 1f);


        //if (cosinA > -.09 && cosinA < .1f)
        //    Debug.Log("?");
        //if (cosinB > -.09 && cosinB < .1f)
        //    Debug.Log("?");
        //if (cosinC > -.09 && cosinC < .1f)
        //    Debug.Log("?");

        //Debug.Log(cosinA);
        //Debug.Log(cosinB);
        //Debug.Log(cosinC);

        //Debug.Log(Mathf.Asin(cosinA));
        //Debug.Log(Mathf.Asin(cosinB));
        //Debug.Log(Mathf.Asin(cosinC));


        //Debug.Log("AtanA = " + Mathf.Atan2(cosinE, cosinA));
        //Debug.Log("AtanB = " + Mathf.Atan2(cosinE, cosinB));
        //Debug.Log("AtanC = " + Mathf.Atan2(cosinE, cosinC));

        Debug.DrawLine(new Vector3(d.x, 0, d.y), (new Vector3(e.x, 0, e.y) - new Vector3(d.x, 0, d.y)).normalized * 50f, UnityEngine.Color.white, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pointA = new Vector3(30, 50, 20);
        Vector3 pointB = new Vector3(10, 5, 10);
        Vector3 pointC = new Vector3(-100, 150, 10);

        Plane(pointA, pointB, pointC, true);

        pointA = new Vector3(30, 50, 20);
        pointB = new Vector3(10, 5, 20);
        pointC = new Vector3(-100, 150, 20);

        Plane(pointA, pointB, pointC, false);

        //Vector3 pointA = new Vector3(0, 50, 0);
        //Vector3 pointB = new Vector3(30, 0, 0);
        //Vector3 pointC = new Vector3(-30, 0, 0);

        //Plane(pointA, pointB, pointC);

        //pointA = new Vector3(0, 50, 2.5f);
        //pointB = new Vector3(60, 0, 15);
        //pointC = new Vector3(-60, 0, -10);

        //Plane(pointA, pointB, pointC);
        // pointB = new Vector3(30, 50, 20);
        // pointA = new Vector3(10, 5, 10);
        // pointC = new Vector3(-10, 10, 10);

        //Plane(pointA, pointB, pointC);
        //pointA = new Vector3(45, 75, 30);
        //pointB = new Vector3(-15, 5, 15);
        //pointC = new Vector3(15, 10, 15);

        //Plane(pointA, pointB, pointC);


        if (done)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            texture.Apply();

        int rc = 0;

        for (int i = 0; i < steps; i++)
        {
            Vector3 albedo = Vector3.zero;
            Vector3 albedoB = Vector3.one * .5f;
            if (useFiveRays == true)
                rc = 5;
            else
                rc = 1;
            float delta = 1f / (float)rc;

            for (int o = 0; o < rc; o++)
            {
                if (y == height)
                {
                    done = true;
                    texture.Apply();
                    Debug.Log(System.DateTime.Now.Millisecond);
                    return;
                }

                RaycastHit hitPointCamera;

                Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)), out hitPointCamera, 5000);

                if (o == 0)
                    Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)), out hitPointCamera, 5000);
                else
                {
                    Vector3 origin = Camera.main.ScreenPointToRay(new Vector3(x + 15.1f, y, 0)).origin;
                    Vector3 dir = Camera.main.ScreenPointToRay(new Vector3(x + 15.1f, y, 0)).direction;


                    if (o == 1)
                        Physics.Raycast(origin, dir, out hitPointCamera, 5000);

                    origin = Camera.main.ScreenPointToRay(new Vector3(x + 15.1f, y, 0)).origin;
                    dir = Camera.main.ScreenPointToRay(new Vector3(x + 15.1f, y, 0)).direction;

                    if (o == 2)
                        Physics.Raycast(origin, dir, out hitPointCamera, 5000);

                    origin = Camera.main.ScreenPointToRay(new Vector3(x, y + 15.1f, 0)).origin;
                    dir = Camera.main.ScreenPointToRay(new Vector3(x, y + 15.1f, 0)).direction;


                    if (o == 3)
                        Physics.Raycast(origin, dir, out hitPointCamera, 5000);

                    origin = Camera.main.ScreenPointToRay(new Vector3(x, y - 15.1f, 0)).origin;
                    dir = Camera.main.ScreenPointToRay(new Vector3(x, y - 15.1f, 0)).direction;

                    if (o == 4)
                        Physics.Raycast(origin, dir, out hitPointCamera, 5000);
                }

                if (hitPointCamera.collider != null)
                {
                    albedo = WorldPointToLightRay(hitPointCamera.point + hitPointCamera.normal * .01f, hitPointCamera.collider.transform.GetComponent<MeshRenderer>().material.mainTexture as Texture2D, hitPointCamera.textureCoord);

                    if (i == 0 || i == steps - 1)
                        Draw(Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0)), hitPointCamera.point, albedo);

                    Vector3 albedoReflections = Vector3.zero;

                    if (hitPointCamera.collider.transform.GetComponent<IsReflective>())
                    {
                        float reflections = 0;

                        Vector3 direction = Vector3.Reflect(Camera.main.transform.forward, hitPointCamera.normal);

                        Vector3 reflectionPoint = hitPointCamera.point;

                        for (int r = 0; r < reflectionPases; r++)
                        {
                            if (r != 0)
                                direction = Vector3.Reflect(direction, hitPointCamera.normal);

                            if (hitPointCamera.collider != null && (r == 0 || hitPointCamera.collider.transform.GetComponent<IsReflective>()))
                            {
                                Physics.Raycast(hitPointCamera.point, direction, out hitPointCamera, 5000);

                                if (hitPointCamera.collider != null)
                                    albedoReflections += WorldPointToLightRay(hitPointCamera.point + hitPointCamera.normal * .01f, reflectionPoint, r, hitPointCamera.collider.transform.GetComponent<MeshRenderer>().material.mainTexture as Texture2D, hitPointCamera.textureCoord);

                                reflectionPoint = hitPointCamera.point;

                                reflections++;
                            }
                        }

                        if (reflectionPases != 0)
                        {
                            albedoReflections /= reflections;


                            albedo = (albedo + albedoReflections) / 2f;
                        }
                    }
                }

                if (rc != 1)
                {


                    albedoB.x = albedoB.x * (1f - delta) + (delta * albedo.x);
                    albedoB.y = albedoB.y * (1f - delta) + (delta * albedo.y);
                    albedoB.z = albedoB.z * (1f - delta) + (delta * albedo.z);
                }
                else
                {
                    albedoB.x = albedo.x;
                    albedoB.y = albedo.y;
                    albedoB.z = albedo.z;

                }
            


                // rayhit * worldtolocal > dir < intersection > ray localtoworld

                //albedo /= (float)rc; ab (if distance < 2f)     1f - !?  color  * distance / 2f

            }

            texture.SetPixel(x, y, new UnityEngine.Color(albedoB.x, albedoB.y, albedoB.z));


 
            x++;
            if (x == width)
            {
                x = 0;
                y++;
            }


        }

        return;
    }

    public Vector3 WorldPointToLightRay(Vector3 origin, int stepIndex, Texture2D texture, Vector2 textureUV)
    {
        Vector3 albedo = Vector3.zero;
        Vector3 albedoB = Vector3.one;
        UnityEngine.Color textureColor = texture.GetPixel((int)(textureUV.x * texture.width), (int)(textureUV.y * texture.height));

        for (int l = 0; l < lights.Length; l++)
        {
            RaycastHit hitPointLight;

            float distance = Vector3.Distance(origin, lights[l].transform.position);
            float delta = 0;

            if (distance < lights[l].distance)
            {
                Physics.Raycast(origin, lights[l].transform.position - origin, out hitPointLight, distance);

                if (hitPointLight.collider != null && (stepIndex == 0 || stepIndex == steps - 1))
                    Debug.DrawLine(origin, hitPointLight.point, UnityEngine.Color.cyan);

                if (hitPointLight.collider == null)
                {
                    

                    delta = 1f - (distance / lights[l].distance);

                    albedo.x = delta * (lights[l].color.x + textureColor.r) / 2f;
                    albedo.y = delta * (lights[l].color.y + textureColor.g) / 2f;
                    albedo.z = delta * (lights[l].color.z + textureColor.b) / 2f;
                }

                albedoB.x = albedoB.x * (1f - delta) + (delta * albedo.x);
                albedoB.y = albedoB.y * (1f - delta) + (delta * albedo.y);
                albedoB.z = albedoB.z * (1f - delta) + (delta * albedo.z);
            }
        }

        return albedoB;
    }

    public Vector3 WorldPointToLightRay(Vector3 origin, Vector3 targetDrawVector, int stepIndex, Texture2D texture, Vector2 textureUV)//canavas texture...
    {
        Vector3 albedo = Vector3.zero;
        Vector3 albedoB = Vector3.one;
        UnityEngine.Color textureColor = texture.GetPixel((int)(textureUV.x * texture.width), (int)(textureUV.y * texture.height));

        for (int l = 0; l < lights.Length; l++)
        {
            RaycastHit hitPointLight;

            float distance = Vector3.Distance(origin, lights[l].transform.position);
            float delta = 0;

            if (distance < lights[l].distance)
            {
                Physics.Raycast(origin, lights[l].transform.position - origin, out hitPointLight, distance);

                if (hitPointLight.collider == null)
                {

                    delta = 1f - (distance / lights[l].distance);

                    albedo.x = delta * (lights[l].color.x + textureColor.r) / 2f;
                    albedo.y = delta * (lights[l].color.y + textureColor.g) / 2f;
                    albedo.z = delta * (lights[l].color.z + textureColor.b) / 2f;

                    Draw(origin, targetDrawVector, albedo, 1f + delta);
                }

                albedoB.x = albedoB.x * (1f - delta) + (delta * albedo.x);
                albedoB.y = albedoB.y * (1f - delta) + (delta * albedo.y);
                albedoB.z = albedoB.z * (1f - delta) + (delta * albedo.z);
            }
        }

        return albedoB;
    }

    public Vector3 WorldPointToLightRay(Vector3 origin, Texture2D texture, Vector2 textureUV)
    {
        Vector3 albedo = Vector3.zero;
        Vector3 albedoB = Vector3.one;
        UnityEngine.Color textureColor = texture.GetPixel((int)(textureUV.x * texture.width), (int)(textureUV.y * texture.height));

        for (int l = 0; l < lights.Length; l++)
        {
            RaycastHit hitPointLight;
            float distance = Vector3.Distance(origin, lights[l].transform.position);
            float delta = 0;

            if (distance < lights[l].distance)
            {

                Physics.Raycast(origin, lights[l].transform.position - origin, out hitPointLight, distance);

                if (hitPointLight.collider == null)
                {

                    delta = 1f - (distance / lights[l].distance);

                    albedo.x = delta * (lights[l].color.x + textureColor.r) / 2f;
                    albedo.y = delta * (lights[l].color.y + textureColor.g) / 2f;
                    albedo.z = delta * (lights[l].color.z + textureColor.b) / 2f;

                    Draw(origin, lights[l].transform.position, albedo);
                }

                albedoB.x = albedoB.x * (1f - delta) + (delta * albedo.x);
                albedoB.y = albedoB.y * (1f - delta) + (delta * albedo.y);
                albedoB.z = albedoB.z * (1f - delta) + (delta * albedo.z);
            }
        }

        return albedoB;
    }


    public void Draw(Vector3 positionA, Vector3 positionB, int depth, int maxDepth)
    {
        float delta = (float)depth / (float)maxDepth;

        float r = delta + delta;
        float g = r / delta;
        float b = r + g;

        float magnitude = (r + g + b);

        r = r / magnitude;
        g = g / magnitude;
        b = b / magnitude;

        float a = (float)depth / (float)maxDepth;

        Debug.DrawLine(positionA, positionB, new UnityEngine.Color(r, g, b, a));
    }


    public void Draw(Vector3 positionA, Vector3 positionB, UnityEngine.Color color)
    {
        Debug.DrawLine(positionA, positionB, color);
    }


    public void Draw(Vector3 positionA, Vector3 positionB, Vector3 color)
    {
        Debug.DrawLine(positionA, positionB, new UnityEngine.Color(color.x, color.y, color.z));// * (1f  - ((float)depth / (float)maxDepth)));

    }

    public void Draw(Vector3 positionA, Vector3 positionB, Vector3 color, float alpha)
    {
        Debug.DrawLine(positionA, positionB, new UnityEngine.Color(color.x, color.y, color.z, alpha));// * (1f  - ((float)depth / (float)maxDepth)));

    }
}