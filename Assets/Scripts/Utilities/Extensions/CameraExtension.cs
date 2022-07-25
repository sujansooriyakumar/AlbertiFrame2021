using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class CameraExtension 
{
    [PublicAPI]
    public static bool IsLayerRendered(this Camera camera, int layer)
    {
        return (camera.cullingMask & (1 << layer)) != 0;
    }

    [PublicAPI]
    public static void RenderLayer(this Camera camera, int layer)
    {
        camera.cullingMask |= 1 << layer;
    }

    [PublicAPI]
    public static void CullLayer(this Camera camera, int layer)
    {
        camera.cullingMask &= ~(1 << layer);
    }

    [PublicAPI]
    public static void ToggleLayerCull(this Camera camera, int layer)
    {
        camera.cullingMask ^= 1 << layer;
    }

    [PublicAPI]
    public static void RenderAllLayers(this Camera camera)
    {
        camera.cullingMask = -1;
    }

    [PublicAPI]
    public static void RenderNoLayers(this Camera camera)
    {
        camera.cullingMask = 0;
    }
}
