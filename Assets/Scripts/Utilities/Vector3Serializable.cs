using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Vector3Serializable
{

    public float X;
    public float Y;
    public float Z;

    public Vector3Serializable(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3Serializable(Vector3 vector) : this(vector.x, vector.y, vector.z)
    {
    }

    public override string ToString()
    {
        return $"{X}, {Y}, {Z}";
    }


    public static implicit operator Vector3(Vector3Serializable value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }

    public static implicit operator Vector3Serializable(Vector3 value)
    {
        return new Vector3Serializable(value.x, value.y, value.z);
    }
}
