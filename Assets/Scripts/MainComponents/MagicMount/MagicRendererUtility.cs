using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRendererUtility : MagicRenderer
{

    readonly Camera cam;
    readonly SettingsMain settings;

    public MagicRendererUtility(Camera cam, SettingsMain settings)
    {
        this.cam = cam;
        if (this.cam == null) throw new ArgumentException(NullErrorString("Camera"));

        this.settings = settings;
        if (this.settings == null) throw new NullReferenceException(NullErrorString("Settings"));
    }

    static string NullErrorString(string missingObject)
    {
        return $"MagicRendererUtility given null {missingObject} in constructor";
    }

    int GetMinimumPossibleTextureSize(float canvasDistance, float fov, float dotProduct, int maxTextureSize, float resolution)
    {

        float desiredTextureSize = resolution * (fov / canvasDistance);
        desiredTextureSize += desiredTextureSize * dotProduct * Math.Abs(settings.Texture.DotMultiplier);

        desiredTextureSize = (int)Mathf.Min(desiredTextureSize, maxTextureSize);

        desiredTextureSize = (int)Mathf.Max(10, desiredTextureSize);
        int roundedTextureSize = (int)desiredTextureSize;

        if (roundedTextureSize == maxTextureSize) Debug.LogWarning($"{AlbertiLog.Prefix} Reached max texture size {maxTextureSize}");
        return roundedTextureSize;
    }

    public RenderTexture GetRenderTexture(float canvasDistance, float fov, float dotProduct, int maxTextureSize, float resolution)
    {

        int textureSize = GetMinimumPossibleTextureSize(canvasDistance, fov, dotProduct, maxTextureSize, resolution);

        if (cam.targetTexture != null) cam.targetTexture.Release(); //to prevent memory leak
        RenderTexture texture = new RenderTexture(textureSize, textureSize, settings.Texture.RenderDepth);

        cam.targetTexture = texture;
        cam.Render();
        return texture;
    }


}