using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MagicOffAxisCameraRig : MountFindableComponent
{
    void SetMaterialTexture(RenderTexture texture);

    float FieldOfView { get; set; }
}

