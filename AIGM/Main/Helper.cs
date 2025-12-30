using System;
using System.Linq;

public class Helper
{
    // Helper to create a random intArray.
    public static int[] CreateRandomIntArray(int length, System.Random rnd)
    {
        var indices = Enumerable.Range(0, length).ToArray();
        for (int i = length - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }
        return indices;
    }

    public struct IntVector2 : IEquatable<IntVector2>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntVector2(int x, int y) => (X, Y) = (x, y);

        // Optional arithmetic helpers
        public IntVector2 Add(IntVector2 other) => new IntVector2(X + other.X, Y + other.Y);
        public IntVector2 Sub(IntVector2 other) => new IntVector2(X - other.X, Y - other.Y);

        //dot

        public override string ToString() => $"({X}, {Y})";
        public bool Equals(IntVector2 other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is IntVector2 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }

    public struct IntVector3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public IntVector3(int x, int y, int z) => (X, Y, Z) = (x, y, z);
        public IntVector3 Add(IntVector3 other) => new IntVector3(X + other.X, Y + other.Y, Z + other.Z);
        public IntVector3 Sub(IntVector3 other) => new IntVector3(X - other.X, Y - other.Y, Z - other.Z);
        public int Dot(IntVector3 other) { return X * other.X + Y * other.Y + Z * other.Z; }
        public IntVector3 Cross(IntVector3 other) { return new IntVector3(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X); }
        public int Distance(IntVector3 other) { return (int)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2)); }
    }

    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y) => (X, Y) = (x, y);
        public Vector2 Add(Vector2 other) => new Vector2(X + other.X, Y + other.Y);
        public Vector2 Sub(Vector2 other) => new Vector2(X - other.X, Y - other.Y);
        public float Dot(Vector2 other) { return X * other.X + Y * other.Y; }
        public static float Dot(Vector2 vectorA, Vector2 vectorB) { return vectorA.X * vectorB.X + vectorA.Y * vectorB.Y; }
        public Vector2 Cross(Vector2 other) { return new Vector2(0, X * other.Y - Y * other.X); }
        public float Distance(Vector2 other) { return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2)); }

        public void Set(float x, float y) { X = x; Y = y; }
        public void SetX(float x) { X = x; }
        public void SetY(float y) { Y = y; }

        public static Vector2 operator -(Vector2 otherA, Vector2 otherB) { return new Vector2(otherA.X - otherB.X, otherA.Y - otherB.Y); }
        public static Vector2 operator +(Vector2 otherA, Vector2 otherB) { return new Vector2(otherA.X + otherB.X, otherA.Y + otherB.Y); }
        public static Vector2 operator *(Vector2 otherA, Vector2 otherB) { return new Vector2(otherA.X * otherB.X, otherA.Y * otherB.Y); }
        public static Vector2 operator /(Vector2 otherA, Vector2 otherB) { return new Vector2(otherA.X / otherB.X, otherA.Y / otherB.Y); }
    }

    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z) => (X, Y, Z) = (x, y, z);
        public Vector3 Add(Vector3 other) => new Vector3(X + other.X, Y + other.Y, Z + other.Z);
        public Vector3 Sub(Vector3 other) => new Vector3(X - other.X, Y - other.Y, Z - other.Z);
        public float Dot(Vector3 other) { return X * other.X + Y * other.Y + Z * other.Z; }
        public static float Dot(Vector3 vectorA, Vector3 vectorB) { return vectorA.X * vectorB.X + vectorA.Y * vectorB.Y + vectorA.Z * vectorB.Z; }
        public Vector3 Cross(Vector3 other) { return new Vector3(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X); }
        public float Distance(Vector3 other) { return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2)); }
        public float SqrtMagnitude() { return (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }

        public Vector3 Normalized()
        {
            float magnitude = SqrtMagnitude();
            return new Vector3(X / magnitude, Y / magnitude, Z / magnitude);
        }

        public void Set(float x, float y, float z) { X = x; Y = y; Z = z; }
        public void SetX(float x) { X = x; }
        public void SetY(float y) { Y = y; }
        public void SetZ(float z) { Z = z; }

        public static Vector3 operator -(Vector3 otherA, Vector3 otherB) { return new Vector3(otherA.X - otherB.X, otherA.Y - otherB.Y, otherA.Z - otherB.Z); }
        public static Vector3 operator +(Vector3 otherA, Vector3 otherB) { return new Vector3(otherA.X + otherB.X, otherA.Y + otherB.Y, otherA.Z + otherB.Z); }
        public static Vector3 operator *(Vector3 otherA, Vector3 otherB) { return new Vector3(otherA.X * otherB.X, otherA.Y * otherB.Y, otherA.Z * otherB.Z); }
        public static Vector3 operator /(Vector3 otherA, Vector3 otherB) { return new Vector3(otherA.X / otherB.X, otherA.Y / otherB.Y, otherA.Z / otherB.Z); }
    }

    public struct Vector4
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector4(float x, float y, float z, float w) => (X, Y, Z, W) = (x, y, z, w);
        public Vector4 Add(Vector4 other) => new Vector4(X + other.X, Y + other.Y, Z + other.Z, W + other.W);
        public Vector4 Sub(Vector4 other) => new Vector4(X - other.X, Y - other.Y, Z - other.Z, W - other.W);
        public float Dot(Vector4 other) { return X * other.X + Y * other.Y + Z * other.Z + W * other.W; }
        public Vector4 Cross(Vector4 other) { return new Vector4(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X, Y * other.W - Y * other.W); }
        public float Distance(Vector4 other) { return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2) + Math.Pow(W - other.W, 2)); }

        public void Set(float x, float y, float z, float w) { X = x; Y = y; Z = z; W = w; }
        public void SetX(float x) { X = x; }
        public void SetY(float y) { Y = y; }
        public void SetZ(float z) { Z = z; }
        public void SetW(float w) { W = w; }

        public static Vector4 operator -(Vector4 otherA, Vector4 otherB) { return new Vector4(otherA.X - otherB.X, otherA.Y - otherB.Y, otherA.Z - otherB.Z, otherA.W - otherB.W); }
        public static Vector4 operator +(Vector4 otherA, Vector4 otherB) { return new Vector4(otherA.X + otherB.X, otherA.Y + otherB.Y, otherA.Z + otherB.Z, otherA.W + otherB.W); }
        public static Vector4 operator *(Vector4 otherA, Vector4 otherB) { return new Vector4(otherA.X * otherB.X, otherA.Y * otherB.Y, otherA.Z * otherB.Z, otherA.W * otherB.W); }
        public static Vector4 operator /(Vector4 otherA, Vector4 otherB) { return new Vector4(otherA.X / otherB.X, otherA.Y / otherB.Y, otherA.Z / otherB.Z, otherA.W / otherB.W); }
    }

    public static float RayCastLine(float positionX, float positionY, float dirX, float dirY, float wallAX, float wallAY, float wallBX, float wallBY, float distance)
    {
        Vector2 v1 = new Vector2(positionX - wallAX, positionY - wallAY);// - new Vector2(wallAX, wallAY);
        Vector2 v2 = new Vector2(wallBX - wallAX, wallBY - wallAY);// - new Vector2(wallAX, wallAY);
        Vector2 v3 = new Vector2(-dirY, dirX);

        float dot = Vector2.Dot(v2, v3);
        float t1 = ((v2.X * v1.Y) - (v2.Y * v1.X)) / dot;

        if (t1 > distance)
        {
            return 0;
        }

        float t2 = Vector2.Dot(v1, v3) / dot;

        if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
        {
            return t1;
        }

        return 0;
    }

    public static float RaycastCircle(float dirX, float dirZ, Vector3 targetPosition, float radius)
    {
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 d = new Vector3(dirX, 0, dirZ);
        float r = radius;

        Vector3 e = targetPosition - position;

        float Esq = e.SqrtMagnitude();
        float a = Vector3.Dot(e, d);
        float b = (float)System.Math.Sqrt(Esq - (a * a));
        float f = (float)System.Math.Sqrt((r * r) - (b * b));

        if (r * r - Esq + a * a < 0f)
        {
            return -1;
        }

        else if (Esq < r * r)
        {
            return a + f;
        }


        return a - f;
    }
}
