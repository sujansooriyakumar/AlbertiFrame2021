using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuaternionSerializable
{
    public float X, Y, Z, W;

    public QuaternionSerializable(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public QuaternionSerializable(Quaternion quaternion)
    {
        X = quaternion.x;
        Y = quaternion.y;
        Z = quaternion.z;
        W = quaternion.w;
    }

    public override string ToString()
    {
        return $"{X}, {Y}, {Z}, {W}";
    }

    public static implicit operator Quaternion(QuaternionSerializable value)
    {
        return new Quaternion(value.X, value.Y, value.Z, value.W);
    }

    public static implicit operator QuaternionSerializable(Quaternion value)
    {
        return new QuaternionSerializable(value.x, value.y, value.z, value.w);
    }
}