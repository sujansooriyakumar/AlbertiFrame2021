using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LayerUtil
{
    const int NumberOfLayers = 32;

    public static int[] AllLayers => Enumerable.Range(0, NumberOfLayers).ToArray();


    public static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }

    }
}