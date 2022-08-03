using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MagicCamera : MountFindableComponent
{
    float FieldOfView { get; set; }

    Camera Cam { get; }

}