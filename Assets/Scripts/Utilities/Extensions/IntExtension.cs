using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntExtension
{
    public static bool IsWithin(this int value, int firstInt, int secondInt)
    {
        int min = Math.Min(firstInt, secondInt);
        int max = Math.Max(firstInt, secondInt);
        return value >= min && value <= max;
    }
}