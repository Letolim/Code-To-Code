using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static P_System;

public class P_System : MonoBehaviour
{

    public float drawDuration = 15f;
    public float drawAlpha = .01f;

    public float constant = 67.430f;
    private List<Particle> particles = new List<Particle>();

    public float spawnDistance = 200;
    public int spawnAmount = 300;

    public bool drawGraph = true;
    public bool drawTrails = true;

    public bool wrapSpace = true;
    public float spaceSize = 1500f;
    public bool clampVelocity = true;
    public float maxVelocity = 6f;
    public bool useFriction = false;
    public float friction = .99f;
    public bool useCollision = false;

    public bool drawVelocity = false;
    public bool newtonSimulation = false;

    public Vector[] staticFields;

    public bool census = false;

    private int distanceCheckIndex = 0;

    private int[][] linkedDimensions;
    //private int[][] drawOnly;

    void Start()
    {
        InitDimensions(0);

        int length = 0;

        for (int i = 0; i < linkedDimensions.Length; i++)
                length += linkedDimensions[i].Length;

        for (int i = 0; i < spawnAmount; i++)
        {
            particles.Add(new Particle(new Vector(length), i, linkedDimensions.Length));

            for (int n = 0; n < length; n++)
                particles[particles.Count - 1].position.vectors[n] = Random.Range(-spawnDistance, spawnDistance);
        }
    }

    private void InitDimensions(int index)
    {
        switch (index)
        {
            case 0:
                linkedDimensions = new int[3][];

                linkedDimensions[0] = new int[1];
                linkedDimensions[1] = new int[3];
                linkedDimensions[2] = new int[2];

                linkedDimensions[0][0] = 0;

                linkedDimensions[1][0] = 1;
                linkedDimensions[1][1] = 2;
                linkedDimensions[1][2] = 3;

                linkedDimensions[2][0] = 4;
                linkedDimensions[2][1] = 5;
                break;

            case 1:
                linkedDimensions = new int[3][];

                linkedDimensions[0] = new int[2];
                linkedDimensions[1] = new int[3];
                linkedDimensions[2] = new int[2];

                linkedDimensions[0][0] = 0;
                linkedDimensions[0][1] = 1;

                linkedDimensions[1][0] = 2;
                linkedDimensions[1][1] = 3;
                linkedDimensions[1][2] = 4;

                linkedDimensions[2][0] = 5;
                linkedDimensions[2][1] = 6;
                break;

            case 2:
                linkedDimensions = new int[1][];

                linkedDimensions[0] = new int[3];

                linkedDimensions[0][0] = 0;
                linkedDimensions[0][1] = 1;
                linkedDimensions[0][2] = 2;
                break;
            case 3:
                linkedDimensions = new int[2][];

                linkedDimensions[0] = new int[3];
                linkedDimensions[1] = new int[3];

                linkedDimensions[0][0] = 0;
                linkedDimensions[0][1] = 1;
                linkedDimensions[0][2] = 2;

                linkedDimensions[1][0] = 3;
                linkedDimensions[1][1] = 4;
                linkedDimensions[1][2] = 5;
                break;

            case 4:
                linkedDimensions = new int[3][];

                linkedDimensions[0] = new int[3];
                linkedDimensions[1] = new int[6];
                linkedDimensions[2] = new int[3];

                linkedDimensions[0][0] = 0;
                linkedDimensions[0][1] = 1;
                linkedDimensions[0][2] = 2;

                linkedDimensions[1][0] = 3;
                linkedDimensions[1][1] = 4;
                linkedDimensions[1][2] = 5;
                linkedDimensions[1][0] = 6;
                linkedDimensions[1][1] = 7;
                linkedDimensions[1][2] = 8;

                linkedDimensions[2][0] = 9;
                linkedDimensions[2][1] = 10;
                linkedDimensions[2][2] = 11;
                break;

            case 5:
                linkedDimensions = new int[3][];

                linkedDimensions[0] = new int[3];
                linkedDimensions[1] = new int[6];
                linkedDimensions[2] = new int[2];

                linkedDimensions[0][0] = 0;
                linkedDimensions[0][1] = 1;
                linkedDimensions[0][2] = 2;

                linkedDimensions[1][0] = 3;
                linkedDimensions[1][1] = 4;
                linkedDimensions[1][2] = 5;
                linkedDimensions[1][0] = 6;
                linkedDimensions[1][1] = 7;
                linkedDimensions[1][2] = 8;

                linkedDimensions[2][0] = 9;
                linkedDimensions[2][1] = 10;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
  
        ApplySettings();

        int count = particles.Count;

        int length = 0;


        float[][] sumDistance = new float[linkedDimensions.Length][];
        float[][] sumDirection = new float[linkedDimensions.Length][];
        float[][] sumForce = new float[linkedDimensions.Length][];


        int index = 0;

        for (int i = 0; i < linkedDimensions.Length; i++)
        {
            sumDistance[i] = new float[linkedDimensions[i].Length];
            sumDirection[i] = new float[linkedDimensions[i].Length];
            sumForce[i] = new float[linkedDimensions[i].Length];
        }


        for (int i = 0; i < count; i++)
        {
            particles[i].Update(particles, linkedDimensions, sumDistance, sumDirection, sumForce);

            for (int k = 0; k < linkedDimensions.Length; k++)
                for (int n = 0; n < linkedDimensions[k].Length; n++)
                {
                    sumDistance[k][n] = 0;
                    sumDirection[k][n] = 0;
                    sumForce[k][n] = 0;
                }

            count = particles.Count;

            if (i >= count)
                break;

          
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
            particles[0].mass += 1;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            particles[0].mass -= 1;

        //Wrap(linkedDimensions);
    }

    public void Wrap(int[][] dimensions)
    {
        int count = particles.Count;

        for (int d = 0; d < dimensions.Length; d++)
        {
            for (int i = 0; i < dimensions[d].Length; i++)
            {
                if (Mathf.Abs(particles[distanceCheckIndex].position.GetVectorByIndex(dimensions[d][i])) > spaceSize)
                    particles[distanceCheckIndex].position.SetVectorByIndex(dimensions[d][i], -Mathf.Sign(particles[distanceCheckIndex].position.GetVectorByIndex(dimensions[d][i])) * spaceSize);

            }
        }

        distanceCheckIndex++;

        if (distanceCheckIndex == count)
            distanceCheckIndex = 0;
    }

    int currentuID = 0;

    public int GetNextuID()
    {
        currentuID++;
        return currentuID;
    }

    public void ApplySettings()
    {
        if (particles[0].clampVelocity != clampVelocity)
            for (int i = 0; i < particles.Count; i++)
                particles[i].clampVelocity = clampVelocity;

        if (particles[0].useFriction != useFriction)
            for (int i = 0; i < particles.Count; i++)
                particles[i].useFriction = useFriction;

        if (particles[0].maxVelocity != maxVelocity)
            for (int i = 0; i < particles.Count; i++)
                particles[i].maxVelocity = maxVelocity;

        if (particles[0].friction != friction)
            for (int i = 0; i < particles.Count; i++)
                particles[i].friction = friction;

        if (particles[0].useCollision != useCollision)
            for (int i = 0; i < particles.Count; i++)
                particles[i].useCollision = useCollision;

        if (particles[0].drawTrail != drawTrails)
            for (int i = 0; i < particles.Count; i++)
                particles[i].drawTrail = drawTrails;

        if (particles[0].newtonSimulation != newtonSimulation)
            for (int i = 0; i < particles.Count; i++)
                particles[i].newtonSimulation = newtonSimulation;

        if (particles[0].constant != constant)
            for (int i = 0; i < particles.Count; i++)
                particles[i].constant = constant;

        if (particles[0].drawVelocity!= drawVelocity)
            for (int i = 0; i < particles.Count; i++)
                particles[i].drawVelocity = drawVelocity;

        if (particles[0].drawDuration != drawDuration)
            for (int i = 0; i < particles.Count; i++)
                particles[i].drawDuration = drawDuration;

        if (particles[0].color.a != drawAlpha)
            for (int i = 0; i < particles.Count; i++)
                particles[i].color.a = drawAlpha;



    }



    public class Vector
    {
        public float[] vectors;

        public Vector() { }


        public Vector(float[] vectors)
        {
            this.vectors = vectors;
        }


        public Vector(int length)
        {
            vectors = new float[length];
        }



        public float GetVectorByIndex(int index)
        {
            return vectors[index];
        }

        public float SetVectorByIndex(int index, float value)
        {
           return vectors[index] = value;
        }

        public static float Distance(Vector positionA, Vector positionB, int[] dimensions)
        {
            float distance = 0;

            for (int i = 0; i < dimensions.Length; i++)
            {
                float vector = positionA.vectors[dimensions[i]] - positionB.vectors[dimensions[i]];

                distance += vector * vector;
            }

            return distance;
        }

        public static float Distance(Vector positionA, Vector positionB)
        {
            float distance = 0;

            for (int i = 0; i < positionA.vectors.Length; i++)
            {
                float vector = positionA.vectors[i] - positionB.vectors[i];

                distance += vector * vector;
            }

            return distance;
        }

        public static float DistanceSquared(Vector positionA, Vector positionB, int[] dimensions)
        {
            float distance = 0;

            for (int i = 0; i < dimensions.Length; i++)
            {
                float vector = positionA.vectors[dimensions[i]] - positionB.vectors[dimensions[i]];

                distance += vector * vector;
            }

            return Mathf.Sqrt(distance);
        }

        public static float Magnitude(Vector vector, int[] dimensions)
        {
            float magnitude = 0;

            for (int i = 0; i < dimensions.Length; i++)
                magnitude += Mathf.Abs(vector.vectors[dimensions[i]]);

            return magnitude;
        }

        public static float Magnitude(Vector vector)
        {
            float magnitude = 0;

            for (int i = 0; i < vector.vectors.Length; i++)
                magnitude += Mathf.Abs(vector.vectors[i]);

                return magnitude;
        }

        public void ClampMagnitudeTotal(float value, int[] dimensions)
        {
            float total = Vector.Magnitude(this, dimensions);

            if (total > value)
            {
                for (int i = 0; i < dimensions.Length; i++)
                    vectors[dimensions[i]] = vectors[dimensions[i]] / total * value;
            }
        }

        public void ClampMagnitudeTotal(float value)
        {
            float total = Vector.Magnitude(this);

            if (total > value)
            {
                for (int i = 0; i < vectors.Length; i++)
                    vectors[i] = vectors[i] / total * value;
            }
        }

        public static Vector Normalize(Vector vector, int[] dimensions)
        {
            float[] values = new float[vector.vectors.Length];

            for (int i = 0; i < vector.vectors.Length; i++)
                values[i] = vector.vectors[i];

            float magnitude = Vector.Magnitude(vector, dimensions);

            for (int i = 0; i < dimensions.Length; i++)
                values[dimensions[i]] /= magnitude;

            return new Vector(values);
        }

        public static Vector Normalized(Vector vector)
        {
            float magnitude = Vector.Magnitude(vector);

            for (int i = 0; i < vector.vectors.Length; i++)
                vector.vectors[i] /= magnitude;

            return vector;
        }

        public void Multiply(float value, int[] dimensions)
        {
            for (int i = 0; i < dimensions.Length; i++)
                vectors[dimensions[i]] *= value;
        }

        public void Multiply(float value)
        {
            for(int i = 0; i < vectors.Length; i ++)
                vectors[i] *= value;
        }

        public void Add(float value, int[] dimensions)
        {
            for (int i = 0; i < dimensions.Length; i++)
                vectors[dimensions[i]] += value;
        }

        public void Add(float value)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] += value;
        }

        public void Substract(float value, int[] dimensions)
        {
            for (int i = 0; i < dimensions.Length; i++)
                vectors[dimensions[i]] = value;
        }

        public void Substract(float value)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] -= value;
        }

        public void Multiply(Vector vector)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] *= vector.vectors[i];
        }

        public void Add(Vector vector)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] *= vector.vectors[i];
        }
  
        public void Substract(Vector vector)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] *= vector.vectors[i];
        }

        public Vector Multiply(float[] values)
        {
            Vector vector = new Vector(this.vectors.Length);

            for (int i = 0; i < values.Length; i++)
                vector.vectors[i] = this.vectors[i] * values[i];

            return vector;
        }

        public void Add(Vector vector, int[] dimensions)
        {
            for (int i = 0; i < dimensions.Length; i++)
                vectors[dimensions[i]] += vector.vectors[dimensions[i]];
        }

        public void Set(Vector vector)
        {
            for (int i = 0; i < vector.vectors.Length; i++)
                vectors[i] = vector.vectors[i];
        }

    }

    public class Particle
    {
        public float constant = 67.430f;
        public float drawDuration = 15f;
        public float drawAlpha = .025f;
        public int uID = 0;
        public float mass;

        public Vector position;
        public Vector3[] drawPositions;

        private Vector velocity;
        public Color color;

        private readonly float minMass = .5f;
        private readonly float maxMass = .5f;

        public bool isStatic = false;
        public bool useFriction = false;
        public float friction = .99f;
        public bool clampVelocity = true;
        public float maxVelocity = 6f;
        public bool newtonSimulation = false;

        public bool useCollision = false;

        public bool drawVelocity = false;
        public bool drawTrail = false;

        private readonly Vector3 drawOffset1D = new Vector3(2000,0,2000);
        private readonly Vector3 drawOffset2D = new Vector3(-2000, 2500, -2000);
        private readonly Vector3 drawOffset3D = new Vector3(2000, -2500, -2000);




        float force = 0;

        public Particle(Vector position, int uID)
        {
            this.position = position;
            this.uID = uID;

            if (uID == 0)
            {
                mass = 50;

                isStatic = true;
                SetColor();
                return;
            }

            velocity = new Vector(position.vectors.Length);

            for (int i = 0; i < velocity.vectors.Length; i++)
                velocity.vectors[i] = Random.Range(-20, 20);

            mass = Random.Range(minMass, maxMass);
            SetColor();
        }

        public Particle(Vector position, int uID, int dimensions)
        {
            this.position = position;
            this.drawPositions = new Vector3[dimensions];


            for (int i = 0; i < dimensions; i++)
                drawPositions[i] = Vector3.zero;

            this.uID = uID;

            //if (uID == 0)
            //{
            //    mass = 50;

            //    isStatic = true;
            //    SetColor();
            //    return;
            //}

            velocity = new Vector(position.vectors.Length);

            for (int i = 0; i < velocity.vectors.Length; i++)
                velocity.vectors[i] = Random.Range(-20, 20);

            mass = 1;
            SetColor();
        }


        public Particle(Vector position, float mass, bool isStatic, int uID)
        {
            this.position = position;
            this.isStatic = isStatic;
            this.mass = mass;
            this.uID = uID;

            velocity = new Vector(position.vectors.Length);

            SetColor();
        }

        public Particle(float mass, Particle parentParticle, int uID)
        {
            useFriction = parentParticle.useFriction;
            friction = parentParticle.friction;
            clampVelocity = parentParticle.clampVelocity;
            clampVelocity = parentParticle.clampVelocity;
            useCollision = parentParticle.useCollision;
            drawTrail = parentParticle.drawTrail;

            this.mass = mass;
            this.uID = uID;

            SetColor();
        }

        public Particle(float mass, float friction, Vector velocity, Vector position)
        {
            this.mass = mass;
            this.friction = friction;
            this.velocity = velocity;
            this.position = position;
        }

        public void Update(List<Particle> particles, int[][] linkedDimensions, float[][] sumDistance, float[][] sumDirection, float[][] sumForce)
        {
            if (isStatic)
                return;

            for (int i = 0; i < linkedDimensions.Length; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    for (int j = 0; j < particles.Count; j++)
                        sumDistance[i][n] += Vector.DistanceSquared(this.position, particles[j].position, linkedDimensions[i]);

            for (int i = 0; i < linkedDimensions.Length; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    for (int j = 0; j < particles.Count; j++)
                        sumDirection[i][n] += particles[j].position.GetVectorByIndex(linkedDimensions[i][n]) - this.position.GetVectorByIndex(linkedDimensions[i][n]);


            if (newtonSimulation)
            {
                for (int i = 0; i < linkedDimensions.Length; i++)
                    for (int n = 0; n < linkedDimensions[i].Length; n++)
                        for (int j = 0; j < particles.Count; j++)
                        {
                            sumForce[i][n] += (this.mass + constant * particles[j].mass);
                        }

            }
            else
                {
                    for (int i = 0; i < linkedDimensions.Length; i++)
                        for (int n = 0; n < linkedDimensions[i].Length; n++)
                            for (int j = 0; j < particles.Count; j++)
                            {
                                float magnitude = constant * (this.mass * this.mass + particles[j].mass * particles[j].mass) / (Mathf.Log((float)linkedDimensions.Length, (float)linkedDimensions[i].Length));
                                sumForce[i][n] += magnitude;
                            }
                    for (int i = 0; i < linkedDimensions.Length; i++)
                        for (int n = 0; n < linkedDimensions[i].Length; n++)
                        {
                            sumDistance[i][n] *= Mathf.Log((float)linkedDimensions.Length, (float)linkedDimensions[i].Length);
                            sumDirection[i][n] *= Mathf.Log((float)linkedDimensions.Length, (float)linkedDimensions[i].Length);
                        }
            }

   

            for (int i = 0; i < linkedDimensions.Length; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    this.velocity.SetVectorByIndex(linkedDimensions[i][n], this.velocity.GetVectorByIndex(linkedDimensions[i][n]) + sumDirection[i][n] * (sumForce[i][n] / sumDistance[i][n]));


            if(useFriction)
            {
                for (int i = 0; i < linkedDimensions.Length; i++)
                    for (int n = 0; n < linkedDimensions[i].Length; n++)
                        this.velocity.SetVectorByIndex(linkedDimensions[i][n], this.velocity.GetVectorByIndex(linkedDimensions[i][n]) * friction);
            }

            if (clampVelocity)
            { 
                for (int i = 0; i < linkedDimensions.Length; i++)
                    this.velocity.ClampMagnitudeTotal(maxVelocity, linkedDimensions[i]); 
            }

            Draw(linkedDimensions);

            for (int i = 0; i < linkedDimensions.Length; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    this.position.SetVectorByIndex(linkedDimensions[i][n], this.position.GetVectorByIndex(linkedDimensions[i][n]) + this.velocity.GetVectorByIndex(linkedDimensions[i][n]));
        }

     
        public void SetColor()
        {
            color = Random.ColorHSV();
            color.a = drawAlpha;
        }

        public void Draw(int[][] linkedDimensions)
        {
            float size = .5f;

            if (isStatic)
                size = 5f;

            float oneDimensionalDraws = 1;
            float twoDimensionalDraws = 1;
            float threeDimensionalDraws = 1;


            for (int i = 0; i < linkedDimensions.Length; i++)
            {
                Vector3 position = Vector3.zero;

                if (linkedDimensions[i].Length == 1)
                {
                    position = new Vector3(drawOffset1D.x * oneDimensionalDraws, this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset1D.y * oneDimensionalDraws, drawOffset1D.z * oneDimensionalDraws);

                    DrawCross(position);
                    
                    if (drawTrail && !isStatic)
                        DrawTrail(position, drawPositions[i], linkedDimensions[i]);

                    if (drawVelocity && !isStatic)
                        DrawVelocity(position, linkedDimensions[i]);

                    oneDimensionalDraws++;
                }

                if (linkedDimensions[i].Length == 2)
                {
                    position = new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset2D.x * twoDimensionalDraws, drawOffset2D.y * twoDimensionalDraws, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset2D.z * twoDimensionalDraws);

                    DrawCross(position);

                    if (drawTrail && !isStatic)
                        DrawTrail(position, drawPositions[i], linkedDimensions[i]);

                    if (drawVelocity && !isStatic)
                        DrawVelocity(position, linkedDimensions[i]);

                    twoDimensionalDraws++;
                }

                if (linkedDimensions[i].Length >= 3)
                {
                    position = new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset3D.x * threeDimensionalDraws, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset3D.y * threeDimensionalDraws, this.position.GetVectorByIndex(linkedDimensions[i][2]) + drawOffset3D.z * threeDimensionalDraws);

                    DrawCross(position);

                    if (drawTrail && !isStatic)
                        DrawTrail(position, drawPositions[i], linkedDimensions[i]);

                    if (drawVelocity && !isStatic)
                        DrawVelocity(position, linkedDimensions[i]);

                    threeDimensionalDraws++;
                }

                if(drawTrail)
                    drawPositions[i] = position;
            }
        }

        float size = 1f;

        public void DrawCross(Vector3 position)
        {
            Debug.DrawLine(new Vector3(position.x, position.y - size, position.z), new Vector3(position.x, position.y + size, position.z), color);
            Debug.DrawLine(new Vector3(position.x - size, position.y, position.z), new Vector3(position.x + size, position.y, position.z), color);
            Debug.DrawLine(new Vector3(position.x, position.y, position.z - size), new Vector3(position.x, position.y, position.z + size), color);
        }




        public void DrawVelocity(Vector3 position, int[] indexes)
        {
            if (indexes.Length == 1)
                Debug.DrawLine(position, position + new Vector3(0, this.velocity.GetVectorByIndex(indexes[0]), 0), color, drawDuration);

            if (indexes.Length == 2)
                Debug.DrawLine(position, position + new Vector3(this.velocity.GetVectorByIndex(indexes[0]), 0, this.velocity.GetVectorByIndex(indexes[1])), color, drawDuration);

            if (indexes.Length == 3)
                Debug.DrawLine(position, position + new Vector3(this.velocity.GetVectorByIndex(indexes[0]), this.velocity.GetVectorByIndex(indexes[1]), this.velocity.GetVectorByIndex(indexes[2])), color, drawDuration);
        }


        public void DrawTrail(Vector3 position, Vector3 previewsPosition, int[] indexes)
        {

            if (indexes.Length == 1)
                Debug.DrawLine(position, previewsPosition, color, drawDuration);

            if (indexes.Length == 2)
                Debug.DrawLine(position, previewsPosition, color, drawDuration);

            if (indexes.Length == 3)
                Debug.DrawLine(position, previewsPosition, color, drawDuration);
        }

    }
}

//public void Update(List<Particle> particles, int[][] linkedDimensions)
//{
//    if (isStatic)
//        return;

//    float[] sumDistance = new float[linkedDimensions.Length];
//    float[] sumDirection = new float[linkedDimensions.Length];
//    float[] sumForce = new float[linkedDimensions.Length];

//    for (int i = 0; i < linkedDimensions.Length; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            for (int j = 0; j < particles.Count; j++)
//                sumDistance[n] += Vector.Distance(this.position, particles[j].position, linkedDimensions[i]);


//    for (int i = 0; i < linkedDimensions.Length; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            for (int j = 0; j < particles.Count; j++)
//                sumDirection[n] -= this.position.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].position.GetVectorByIndex(linkedDimensions[i][n]);



//    for (int i = 0; i < linkedDimensions.Length; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            for (int j = 0; j < particles.Count; j++)
//                sumForce[n] += (this.mass + 67.430f * particles[j].mass);





//    for (int i = 0; i < linkedDimensions.Length; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            this.velocity.SetVectorByIndex(linkedDimensions[i][n], this.velocity.GetVectorByIndex(linkedDimensions[i][n]) + sumDirection[n] * (sumForce[n] / Mathf.Sqrt(sumDistance[n])));



//    for (int i = 0; i < linkedDimensions.Length; i++)
//        this.velocity.ClampMagnitudeTotal(1f, linkedDimensions[i]);

//    Draw(linkedDimensions);

//    for (int i = 0; i < linkedDimensions.Length; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            this.position.SetVectorByIndex(linkedDimensions[i][n], this.position.GetVectorByIndex(linkedDimensions[i][n]) + this.velocity.GetVectorByIndex(linkedDimensions[i][n]));
//}

//public void DrawTrail(int[][] linkedDimensions)
//{
//    for (int i = 0; i < linkedDimensions.Length; i++)
//    {
//        if (linkedDimensions[i].Length == 1)
//        {
//            Vector3 position = new Vector3(drawOffset1D.x, this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset1D.y, drawOffset1D.z);
//            Debug.DrawLine(position, position + new Vector3(0, this.velocity.GetVectorByIndex(linkedDimensions[i][0]), 0), color, 15f);
//        }

//        if (linkedDimensions[i].Length == 2)
//        {
//            Vector3 position = new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset2D.x, drawOffset2D.y, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset2D.z);
//            Debug.DrawLine(position, position + new Vector3(this.velocity.GetVectorByIndex(linkedDimensions[i][0]), 0, this.velocity.GetVectorByIndex(linkedDimensions[i][1])), color, 15f);
//        }

//        if (linkedDimensions[i].Length == 3)
//        {
//            Vector3 position = new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset3D.x, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset3D.y, this.position.GetVectorByIndex(linkedDimensions[i][2]) + drawOffset3D.z);
//            Debug.DrawLine(position, position + new Vector3(this.velocity.GetVectorByIndex(linkedDimensions[i][0]), this.velocity.GetVectorByIndex(linkedDimensions[i][1]), this.velocity.GetVectorByIndex(linkedDimensions[i][2])), color, 15f);
//        }
//    }
//}

//public void Update(List<Particle5> particles, int[][] linkedDimensions, bool hmmm)
//{
//    if (isStatic)
//        return;

//    float[] sumDistance = new float[linkedDimensions.Length];
//    float[] sumForce = new float[linkedDimensions.Length];

//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//    {
//        float vector = 0;

//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            for (int j = 0; j < particles.Count; j++)
//            {
//                float value = this.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//                vector += value * value;
//            }

//        sumDistance[i] = vector;
//    }

//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            for (int j = 0; j < particles.Count; j++)
//                sumForce[i] += (this.mass + 67.430f * particles[j].mass) / Math.Sqrt(sumDistance[n]);

//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            for (int j = 0; j < particles.Count; j++)
//                this.velocity.SetVectorByIndex(linkedDimensions[i][n], this.velocity.GetVectorByIndex(linkedDimensions[i][n]) + (this.position.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].position.GetVectorByIndex(linkedDimensions[i][n])) * sumForce[n]);


//    Draw(linkedDimensions);

//    if (useFriction)
//        velocity.Multiply(friction);

//    if (clampVelocity)
//        velocity.ClampMagnitude(maxVelocity);


//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//            this.position.SetVectorByIndex(linkedDimensions[i][n], this.position.GetVectorByIndex(linkedDimensions[i][n]) + this.position.GetVectorByIndex(linkedDimensions[i][n]));


//    WEIRDO ZONE----------------------------------------------------------------------------------------------------------------https://en.wikipedia.org/wiki/Calabi%E2%80%93Yau_manifold
//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//          for (int n = 0; n < linkedDimensions[i].Length; n++)
//     mass = mass * (massj * massj + massJ * massJ - 1) / (Mathf.Exp(linkedDimensions[i],linkedDimensions[n].Length);
//        V = V(x ^{ 2} +y ^{ 2} -1) \subseteq \mathbf { C} ^{ 2},}
//    ---------------------------------------------------------------------------------------------------------------------------https://en.wikipedia.org/wiki/Noncommutative_algebraic_geometry >// https://en.wikipedia.org/wiki/Hilbert%27s_Nullstellensatz > //https://en.wikipedia.org/wiki/Affine_variety 
//}{\displaystyle V=V(x^{2}+y^{2}-1)\subseteq \ {C} ^{2},}

//public void Collide(Particle collider, List<Particle> particles, P_System P_system)
//{
//    float totalMass = mass + collider.mass;

//    if (totalMass > maxMass)
//    {
//        mass = maxMass;
//        velocity = (collider.velocity + velocity) / 2f;

//        float leftMass = totalMass - maxMass;
//        float maxNewObjects = leftMass / minMass;

//        int count = Random.Range(1, (int)maxNewObjects);

//        for (int i = 0; i < count; i++)
//            particles.Add(new Particle(leftMass / (float)count, this, (collider.position - position).normalized, P_system.GetNextuID()));

//    }
//    else
//    {
//        mass += collider.mass;
//        velocity = (collider.velocity + velocity) / 2f;
//    }
//    SetColor();
//}


























//dir.Multiply(((mass + 67.430f * particles[i].mass) / distance));
//dimensionalDirections[i] = new float[linkedDimensions[i].Length]
//float[][] dimensionalDirections = new float[linkedDimensions.Length];
//dimensionalDirections[i][n] = this.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//square(dimensionalDistances[i];

//float[] dimensionalDistances = new float[linkedDimensions.Length];

//for (int i = 0; i < linkedDimensions.GetLength; i++)
//{
//    sumDistance = new float[linkedDimensions[i].Length];
//    for (int n = 0; n < linkedDimensions[i].Length; n++)
//    {
//        for (int j = 0; j < particles.Count; j++)
//        {
//            sumDistance[i] = this.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//            sumDistance[i] *= sumDistance[i];
//        }
//    }
//}


//for (int i = 0; i < linkedDimensions.GetLength; i++)
//{
//    float distance = 0;
//    for (int n = 1; n < linkedDimensions[i].Length; n++)
//        distance += sumDistance[n - 1] + sumDistance[n];
//    dimensionalDistances = distance;
//}

//float[] distances = new float[particles.Count];

//for (int j = 0; j < particles.Count; j++)
//{

//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//    {
//        float[] sumDistance = new float[linkedDimensions[i].Length];
//        float vector = 0;
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//        {
//            float value = this.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//            vector += value * value;
//        }
//        sumDistance[n] += vector;
//        for (int i = 0; i < sumDistance.Length; i++)
//            vector += sumDistance[i];
//        distances[j] = vector;
//    }
//}



//float[][] distances = new float[particles.Count];

//for (int j = 0; j < particles.Count; j++)
//    distances[j] = new float[linkedDimensions.Length];

//for (int j = 0; j < particles.Count; j++)
//{
//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//    {
//        float[] sumDistance = new float[linkedDimensions[i].Length];
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//        {
//            float vector += this.GetVectorByIndex(linkedDimensions[i][o]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//            sumDistance[o] = vector * vector;
//            for (int o = 0; o < linkedDimensions[i].Length; o++)
//            {
//                float vector += this.GetVectorByIndex(linkedDimensions[i][o]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//                sumDistance[o] = vector * vector;
//            }
//            float vector = 0;
//            for (int i = 0; i < sumDistance.Length; i++)
//                vector += sumDistance[i];
//            distances[j][n] = vector;
//        }
//    }
//}

//float[] distances = new float[particles.Count];

//for (int j = 0; j < particles.Count; j++)
//{
//    for (int i = 0; i < linkedDimensions.GetLength; i++)
//    {
//        float[] sumDistance = new float[linkedDimensions[i].Length];
//        for (int n = 0; n < linkedDimensions[i].Length; n++)
//        {
//            for (int o = 0; o < linkedDimensions[i].Length; o++)
//            {
//                float vector += this.GetVectorByIndex(linkedDimensions[i][o]) - particles[j].GetVectorByIndex(linkedDimensions[i][n]);
//                sumDistance[o] = vector * vector;
//            }
//            float vector = 0;
//            for (int i = 0; i < sumDistance.Length; i++)
//                vector += sumDistance[i];
//            distances[j] = Mathf.Sqrt(vector);
//        }
//    }
//}



//public static Vector Normalize(Vector vector, int[] dimensions)
//{
//    float[] values = new float[vector.vectors.Length];

//    //---------------------------------------------
//    for (int i = 0; i < values.Length; i++)
//    {
//        // Use a multiplication trick: if i is part of dimensions, assign normalized value;
//        // otherwise, preserve the original value.

//        for (int j = 0; j < dimensions.Length; j++)
//            (i == dimensions[j] ? values[dimensions[Array.IndexOf(dimensions, i)]] = vector.vectors[dimensions[Array.IndexOf(dimensions, i)]] : values[i] = vector.vectors[i]);

//        for (int i = 0; i < values.Length; i++)
//        {
//            int index = Array.IndexOf(dimensions, i); // Check if `i` exists in dimensions
//            values[i] = (index != -1) ? vector.vectors[dimensions[index]] : vector.vectors[i];
//        }



//    }
//    //---------------------------------------------

//    float magnitude = Vector.Magnitude(vector, dimensions);

//    for (int i = 0; i < dimensions.Length; i++)
//        values[dimensions[i]] /= magnitude;

//    return new Vector(values);
//}