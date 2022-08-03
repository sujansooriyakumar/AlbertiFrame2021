using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MagicProjector : MountFindableComponent
{
    void SetMaterialTexture(RenderTexture texture);

    float FieldOfView { get; set; }

}

