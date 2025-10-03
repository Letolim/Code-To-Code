using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SystemA : MonoBehaviour
{
    private List<Particle> particles = new List<Particle>();
    public float spawnDistance = 200f;
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

    //public Vector[] staticFields;

    public bool census = false;

    private int distanceCheckIndex = -1;


    private int[][] linkedDimensions;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < staticFields.Length; i++)
        //particles.Add(new Particle5(new Vector3(staticFields[i].x, staticFields[i].y, staticFields[i].z), staticFields[i].w, true, GetNextuID()));
        linkedDimensions = new int[3];

        linkedDimensions[0] = new int[2];
        linkedDimensions[1] = new int[1];
        linkedDimensions[2] = new int[2];

        linkedDimensions[0][0] = 0;
        linkedDimensions[0][1] = 1;

        linkedDimensions[1][0] = 2;

        linkedDimensions[2][0] = 3;
        linkedDimensions[2][1] = 4;
        for (int i = 0; i < spawnAmount; i++)
            particles.Add(new Particle5(new Vector5(Random.Range(-spawnDistance, spawnDistance), Random.Range(-spawnDistance, spawnDistance), Random.Range(-spawnDistance, spawnDistance), Random.Range(-spawnDistance, spawnDistance), Random.Range(-spawnDistance, spawnDistance)), GetNextuID()));
    }

    
    // Update is called once per frame
    void Update()
    {
        //if (census)
        //{
        //    float mass = 0;
        //    for (int i = 0; i < particles.Count; i++)
        //        mass += particles[i].mass;

        //    Debug.Log("total mass = " + mass + "\n count = " + particles.Count);

        //    census = false;
        //}


        ApplySettings();

        int count = particles.Count;

        for (int i = 0; i < count; i++)
        {
            particles[i].Update(particles, linkedDimensions);

            count = particles.Count;

            if (i >= count)
                break;

          
        }

        Wrap(linkedDimensions);
    }

    public void Wrap(int[] dimensions)
    {

        for (int d = 0; d < dimensions.Length; d++)
        {
            float vector;

            if (Math.Abs(particles[distanceCheckIndex].GetVectorByIndex(dimensions[d])) > spaceSize)
                vector = -Math.Sign(particles[distanceCheckIndex].GetVectorByIndex(dimensions[d])) * spaceSize;

            particles[distanceCheckIndex].SetVecorByIndex(dimensions[d], vector);

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

    }



    public class Vector
    {
        public float[] vectors;

        public Vector(float[] vectors)
        {
                
        }


        public Vector(int length)
        {

        }



        public float GetVectorByIndex(int index)
        {
            if (index == 0)
                return x;
            if (index == 1)
                return y;
            if (index == 2)
                return z;
            if (index == 3)
                return w;
            if (index == 4)
                return a;

            return -1;
        }

        public float SetVectorByIndex(int index, float value)
        {
            if (index == 0)
                x = value;
            if (index == 1)
                y = value;
            if (index == 2)
                z = value;
            if (index == 3)
                w = value;
            if (index == 4)
                a = value;
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

            for (int i = 0; i < vectors.Length; i++)
            {
                float vector = positionA.vectors[i] - positionB.vectors[i];

                distance += vector * vector;
            }

            return distance;
        }

        public static float DistanceSquared(Vector positionA, Vector positionB, int[] dimensions)
        {
            float distance = 0;

            for (int i = 0; i < vectors.Length; i++)
            {
                float vector = positionA.vectors[i] - positionB.vectors[i];

                distance += vector * vector;
            }

            return Math.Sqrt(distance);
        }

        public static float Magnitude(Vector vector, int[] dimensions)
        {
            float magnitude;

            for (int i = 0; i < dimensions.Length; i++)
                magnitude += Mathf.Abs(vector.vectors[dimensions[i]]);

            return magnitude;
        }

        public static float Magnitude(Vector vector)
        {
            float magnitude;

            for (int i = 0; i < vectors.Length; i++)
                magnitude += Mathf.Abs(vectors[i]);

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
            for (int i = 0; dimensions.Length; i++)
                vectors[dimensions[i]] *= value;
        }

        public void Multiply(float value)
        {
            for(int i = 0; i < vectors.Length; i ++)
                vectors[i] *= value;
        }

        public void Add(float value, int[] dimensions)
        {
            for (int i = 0; dimensions.Length; i++)
                vectors[dimensions[i]] += value;
        }

        public void Add(float value)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] += value;
        }

        public void Substract(float value, int[] dimensions)
        {
            for (int i = 0; dimensions.Length; i++)
                vectors[dimensions[i]] = value;
        }

        public void Substract(float value)
        {
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] -= value;
        }

        public void Multiply(Vector vector)
        {
            for (int i = 0; vectors.Length; i++)
                vectors[i] *= vector.vectors[i];
        }

        public void Add(Vector vector)
        {
            for (int i = 0; vectors.Length; i++)
                vectors[i] *= vector.vectors[i];
        }
  
        public void Substract(Vector vector)
        {
            for (int i = 0; vectors.Length; i++)
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
            for (int i = 0; dimensions.Length; i++)
                vectors[dimensions[i]] += vector.vectors[dimensions[i]];
        }

        public static Vector GetNormalizedDirection(Vector from, Vector to)
        {
            Vector5 dir = new Vector5();

            dir.x = to.x - from.x;
            dir.y = to.y - from.y;
            dir.z = to.z - from.z;
            dir.w = to.w - from.w;
            dir.a = to.a - from.a;

            float magnitude = Vector5.Magnitude(dir);

            dir.x /= magnitude;
            dir.y /= magnitude;
            dir.z /= magnitude;
            dir.w /= magnitude;
            dir.a /= magnitude;

            return dir;
        }

    }

    public class Particle
    {
        public int uID = 0;
        public float mass;
        public Vector position;

        private Vector velocity;
        private Color color;

        private readonly float minMass = .5f;
        private readonly float maxMass = 25f;

        public bool isStatic = false;
        public bool useFriction = false;
        public float friction = .99f;
        public bool clampVelocity = true;
        public float maxVelocity = 6f;
        public bool useCollision = false;

        public bool drawTrail = false;

        private readonly Vector3 drawOffset1D = new Vector3(0,0,0);
        private readonly Vector3 drawOffset2D = new Vector3(0, 1500, 0);
        private readonly Vector3 drawOffset3D = new Vector3(0, -1500, 0);




        float force = 0;

        public Particle(Vector position, int uID)
        {
            this.position = position;
            this.uID = uID;

            velocity = new Vector5();
            mass = Random.Range(minMass, maxMass);

            SetColor();
        }

        public Particle(Vector position, float mass, bool isStatic, int uID)
        {
            this.position = position;
            this.isStatic = isStatic;
            this.mass = mass;
            this.uID = uID;

            velocity = new Vector5();

            SetColor();
        }

        public Particle(float mass, Particle parentParticle, int uID)
        {
            //position = parentParticle.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            useFriction = parentParticle.useFriction;
            friction = parentParticle.friction;
            clampVelocity = parentParticle.clampVelocity;
            clampVelocity = parentParticle.clampVelocity;
            useCollision = parentParticle.useCollision;
            drawTrail = parentParticle.drawTrail;
            //velocity = (parentParticle.position - position).normalized * parentParticle.velocity.magnitude * 5f;

            this.mass = mass;
            this.uID = uID;

            SetColor();
        }

        //public Particle5(float mass, Particle5 parentParticle, Vector3 impactDir, int uID)
        //{
        //    impactDir.x = Mathf.Abs(impactDir.x);
        //    impactDir.y = Mathf.Abs(impactDir.y);
        //    impactDir.z = Mathf.Abs(impactDir.z);
        //    impactDir = Vector3.one - impactDir;


        //    //position = parentParticle.position + new Vector3(Random.Range(-10, 10) * impactDir.x, Random.Range(-10, 10) * impactDir.y, Random.Range(-10, 10) * impactDir.z);
        //    useFriction = parentParticle.useFriction;
        //    friction = parentParticle.friction;
        //    clampVelocity = parentParticle.clampVelocity;
        //    clampVelocity = parentParticle.clampVelocity;
        //    useCollision = parentParticle.useCollision;
        //    drawTrail = parentParticle.drawTrail;
        //    //velocity = (parentParticle.position - position).normalized * parentParticle.velocity.magnitude * 5f;

        //    this.mass = mass;
        //    this.uID = uID;

        //    SetColor();
        //}

        public Particle(float mass, float friction, Vector velocity, Vector position)
        {
            this.mass = mass;
            this.friction = friction;
            this.velocity = velocity;
            this.position = position;
        }



        public void Update(List<Particle> particles, int[][] linkedDimensions)
        {
            if (isStatic)
                return;

            float[] sumDistance = new float[linkedDimensions.Length];
            float[] sumForce = new float[linkedDimensions.Length];

            for (int j = 0; j < particles.Count; j++)
                sumDistance += Vector.Distance(this.position, particles[j].position);


            for (int i = 0; i < linkedDimensions.GetLength; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    for (int j = 0; j < particles.Count; j++)
                        sumForce[i] += (this.mass + 67.430f * particles[j].mass) / Math.Sqrt(sumDistance[n]);

            for (int i = 0; i < linkedDimensions.GetLength; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    for (int j = 0; j < particles.Count; j++)
                        this.velocity.SetVectorByIndex(linkedDimensions[i][n], this.velocity.GetVectorByIndex(linkedDimensions[i][n]) + (this.position.GetVectorByIndex(linkedDimensions[i][n]) - particles[j].position.GetVectorByIndex(linkedDimensions[i][n])) * sumForce[n]);


            Draw(linkedDimensions);

            for (int i = 0; i < linkedDimensions.GetLength; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    this.position.SetVectorByIndex(linkedDimensions[i][n], this.position.GetVectorByIndex(linkedDimensions[i][n]) + this.position.GetVectorByIndex(linkedDimensions[i][n]));
        }


   



        public void SetColor()
        {
            color = Random.ColorHSV();
            color.a = .25f;
        }

        public void Draw(int[][] linkedDimensions)
        {
            float size = .5f;

            if (isStatic)
                size = 5f;

            for (int i = 0; i < linkedDimensions.GetLength; i++)
            {
                if (linkedDimensions[i].Length == 1)
                {
                    DrawCross(new Vector3(drawOffset1D.x, this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset1D.y, drawOffset1D.z));
                }

                if (linkedDimensions[i].Length == 2)
                {
                    DrawCross(new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset2D.y, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset2D.y, drawOffset2D.z)); 
                }

                if (linkedDimensions[i].Length == 3)
                {
                    DrawCross(new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset3D.y, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset3D.y, this.position.GetVectorByIndex(linkedDimensions[i][2]) + drawOffset3D.z));
                }
            }

            if (drawTrail && !isStatic)
                DrawTrail(linkedDimensions);
        }


        public void DrawCross(Vector3 position)
        {

            Debug.DrawLine(new Vector3(position.x, position.y - size, position.z), new Vector3(position.x, position.y + size, position.z), Color.cyan);
            Debug.DrawLine(new Vector3(position.x - size, position.y, position.z), new Vector3(position.x + size, position.y, position.z), Color.cyan);
            Debug.DrawLine(new Vector3(position.x, position.y, position.z - size), new Vector3(position.x, position.y, position.z + size), Color.cyan);
        }


        public void DrawTrail(int[][] linkedDimensions)
        {
            for (int i = 0; i < linkedDimensions.GetLength; i++)
                for (int n = 0; n < linkedDimensions[i].Length; n++)
                    this.position.SetVectorByIndex(linkedDimensions[i][n], this.position.GetVectorByIndex(linkedDimensions[i][n]) + this.position.GetVectorByIndex(linkedDimensions[i][n]));

            for (int i = 0; i < linkedDimensions.GetLength; i++)
            {

                if (linkedDimensions[i].Length == 1)
                {
                    Vector3 position = new Vector3(drawOffset1D.x, this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset1D.y, drawOffset1D.z);
                    Debug.DrawLine(position, position + new Vector3(drawOffset1D.x, this.velocity.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset1D.y, drawOffset1D.z), 15f);
                }

                if (linkedDimensions[i].Length == 2)
                {
                    Vector3 position = new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset2D.y, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset2D.y, drawOffset2D.z);
                    Debug.DrawLine(position, position +  new Vector3(this.velocity.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset2D.y, this.velocity.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset2D.y, drawOffset2D.z), 15f);
                }

                if (linkedDimensions[i].Length == 3)
                {
                    Vector3 position = new Vector3(this.position.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset3D.y, this.position.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset3D.y, this.position.GetVectorByIndex(linkedDimensions[i][2]) + drawOffset3D.z);
                    Debug.DrawLine(position, position + new Vector3(this.velocity.GetVectorByIndex(linkedDimensions[i][0]) + drawOffset3D.y, this.velocity.GetVectorByIndex(linkedDimensions[i][1]) + drawOffset3D.y, this.velocity.GetVectorByIndex(linkedDimensions[i][2]) + drawOffset3D.z), 15f);
                }
            }
        }
    }
}

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
//}

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