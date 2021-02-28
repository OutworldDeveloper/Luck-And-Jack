using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FlatVector 
{

    public static readonly FlatVector zero = new FlatVector(0f, 0f);
    public static readonly FlatVector forward = new FlatVector(0f, 1f);
    public static readonly FlatVector back = new FlatVector(0f, -1f);
    public static readonly FlatVector right = new FlatVector(1f, 0f);
    public static readonly FlatVector left = new FlatVector(-1f, -0f);

    public static float Distance(FlatVector a, FlatVector b)
    {
        return Vector3.Distance(a.Vector3, b.Vector3);
    }

    public static float Angle(FlatVector from, FlatVector to)
    {
        return Vector3.Angle(from.Vector3, to.Vector3);
    }

    public static float Angle(Vector3 from, FlatVector to)
    {
        return Angle(new FlatVector(from), to);
    }

    public static float Angle(FlatVector from, Vector3 to)
    {
        return Angle(from, new FlatVector(to));
    }

    public static float Angle(Vector3 from, Vector3 to)
    {
        return Angle(new FlatVector(from), new FlatVector(to));
    }

    // Operators

    public static FlatVector operator -(FlatVector a, FlatVector b)
    {
        return new FlatVector(a.x - b.x, a.z - b.z);
    }

    public static FlatVector operator -(FlatVector a)
    {
        return new FlatVector(-a.x, -a.z);
    }

    public static bool operator !=(FlatVector lhs, FlatVector rhs)
    {
        return lhs.Vector3 != rhs.Vector3;
    }

    public static FlatVector operator *(FlatVector a, float d)
    {
        return new FlatVector(a.x * d, a.z * d);
    }

    public static FlatVector operator *(float d, FlatVector a)
    {
        return a * d;
    }

    public static FlatVector operator /(FlatVector a, float d)
    {
        return new FlatVector(a.x / d, a.z / d);
    }

    public static FlatVector operator +(FlatVector a, FlatVector b)
    {
        return new FlatVector(a.x + b.x, a.z + b.z);
    }

    public static bool operator ==(FlatVector lhs, FlatVector rhs)
    {
        return lhs.Vector3 == rhs.Vector3;
    }

    public static implicit operator Vector3(FlatVector target)
    {
        return new Vector3(target.x, 0f, target.z);
    }

    public static implicit operator FlatVector(Vector3 target)
    {
        return new FlatVector(target.x, target.z);
    }

    public float x;
    public float z;

    public Vector3 Vector3 => new Vector3(x, 0f, z);
    public FlatVector normalized => Vector3.Normalize(Vector3).FlatVector();

    public FlatVector(float x, float z)
    {
        this.x = x;
        this.z = z;
    }

    public FlatVector(Vector3 vector3)
    {
        this.x = vector3.x;
        this.z = vector3.z;
    }

    public override bool Equals(object obj) => obj is FlatVector other && x == other.x && z == other.z;

    public override int GetHashCode() => Vector3.GetHashCode();

    public override string ToString() => Vector3.ToString();

}

public static class FlatVectorExtension
{

    public static FlatVector FlatPosition(this Transform transform)
    {
        return new FlatVector(transform.position);
    }

    public static FlatVector FlatVector(this Vector3 vector)
    {
        return new FlatVector(vector);
    }

}