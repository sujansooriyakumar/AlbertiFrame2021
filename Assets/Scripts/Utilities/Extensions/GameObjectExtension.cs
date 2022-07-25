using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtension 
{
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        SetLayerOfChildren(gameObject.transform, layer);

    }

    static void SetLayerOfChildren(Transform baseTransform, int layer)
    {
        List<Transform> children = baseTransform.GetComponentsInChildren<Transform>(true)
            .Where(child => child != baseTransform.parent && child != baseTransform).ToList();

        foreach (Transform transform in children)
        {
            transform.gameObject.layer = layer;
            SetLayerOfChildren(transform, layer);
        }
    }

}
