using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MountDistanceCalculator
{
    float CanvasDistanceFromCamera { get; }

    ///  <summary>
    ///  Main form of optimization to increase performance
    /// 
    ///  Adjusts the field of view of camera and projector to absolute smallest possible
    ///  to only render what is needed.
    ///  This increases the resolution of the picture to reduce pixelation as much as possible.
    ///  May still be pixelated if picture taken when user is standing very close to the frameInterface.
    ///  Pixelation can be reduced further by increasing the max size of the pictorial
    ///  camera's render texture, but this comes at the cost of more RAM and lower FPS.
    /// 
    ///  If edges of canvas not filled (clipped at corners) then increase ExtraSizeToAvoidClip value slightly in settings
    ///  </summary>
    float GetMinimumRequiredFieldOfView(float canvasDiagonalSize);
}