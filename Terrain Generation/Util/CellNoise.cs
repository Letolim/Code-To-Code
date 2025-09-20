using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellNoise
{
    public Vector2 HashToVector2(Vector2 position)
    {
        Vector2 hashValue = new Vector2(Vector2.Dot(position, new Vector2(127.1f, 311.7f)), Vector2.Dot(position, new Vector2(269.5f, 183.3f)));

        hashValue.x = Mathf.Sin(hashValue.x) * 43758.5453f;
        hashValue.x = (hashValue.x - Mathf.Floor(hashValue.x));
        hashValue.y = Mathf.Sin(hashValue.y) * 43758.5453f;
        hashValue.y = (hashValue.y - Mathf.Floor(hashValue.y));
        return hashValue;
    }

    public float HashToFloat(Vector2 position)
    {
        float hashValue = Vector2.Dot(position, new Vector2(7f, 113f));

        hashValue = Mathf.Sin(hashValue) * 43758.5453f;
        return hashValue - Mathf.Floor(hashValue);
    }

    //return fract(sin(n)*43758.5453); // n = n + g
    //sin( hash1(dot(n+g,vec2(7.0,113.0)))
    //fract(sin(dot(n+g,vec2(7.0,113.0)))*43758.5453


    public float VoronoiNoise(float positionX, float positionY, float smoothness)
    {
        Vector2 a = new Vector2(Mathf.Floor(positionX), Mathf.Floor(positionY));
        Vector2 b = new Vector2(positionX - Mathf.Floor(positionX), positionY - Mathf.Floor(positionY));
        float value = 8f;
        for (int i = -2; i <= 2; i++)
            for (int n = -2; n <= 2; n++)
            {
                Vector2 g = new Vector2(i, n);
                Vector2 o = HashToVector2(a + g);

                float bias = 0.5f + 0.5f * Mathf.Sin(HashToFloat(a + g) * 2.5f + 3.5f);
                bias = bias * bias;

                float distance = Vector2.Distance(g - b, o);
                float h = Mathf.SmoothStep(-1.0f, 1.0f, (value - distance) / .3f);

                value = Mathf.Lerp(value, distance, h);
            }

        return value;
    }

    public float VoronoiCellNoise(float positionX, float positionY, float smoothness)
    {
        Vector2 a = new Vector2(Mathf.Floor(positionX), Mathf.Floor(positionY));
        Vector2 b = new Vector2(positionX - Mathf.Floor(positionX), positionY - Mathf.Floor(positionY));
        float c = 0;// c = 0 ?
        float value = 8f;
        for (int i = -2; i <= 2; i++)
            for (int n = -2; n <= 2; n++)
            {
                Vector2 g = new Vector2(i, n);
                Vector2 o = HashToVector2(a + g);

                float bias = 0.5f + 0.5f * Mathf.Sin(HashToFloat(a + g) * 2.5f + 3.5f);
                bias = bias * bias;

                float distance = Vector2.Distance(g - b, o);
                float h = Mathf.SmoothStep(-1.0f, 1.0f, (value - distance) / .3f);

                value = Mathf.Lerp(value, distance, h);
                c = Mathf.Lerp(c, bias, h);
            }

        return c;
    }
}
