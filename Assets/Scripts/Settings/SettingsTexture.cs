using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingsTexture : ScriptableObject
{


    [Header("You should probably not adjust these:")]

    public int RenderDepth = 2000;

    public Material DefaultProjectorMaterial;

    [Range(1, 1.5f)]
    public float FovMultiplierToAvoidEdgeClip = 1;

    [Range(0, 1)]
    public float DotMultiplier;

}