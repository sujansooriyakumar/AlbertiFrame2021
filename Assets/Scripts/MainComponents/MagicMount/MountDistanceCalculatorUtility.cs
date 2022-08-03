using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountDistanceCalculatorUtility : MountDistanceCalculator
{
    readonly Transform camera;
    readonly Transform canvas;
    SettingsTexture settings;

    public MountDistanceCalculatorUtility(Transform camera, Transform canvas, SettingsTexture settings)
    {
        this.camera = camera;
        this.canvas = canvas;
        this.settings = settings;
    }

    public float CanvasDistanceFromCamera
    {

        get
        {

            Vector3 canvasPosition = canvas.position;
            Vector3 cameraPosition = camera.position;

            //TODO account for oblique angles. Possibly base it on the farthest distance of a corner?

            Vector3 canvasVector = cameraPosition - canvasPosition;
            float canvasDistance = canvasVector.magnitude;
            return canvasDistance;
        }


    }


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
    public float GetMinimumRequiredFieldOfView(float canvasDiagonalSize)
    {
        float canvasDistance = CanvasDistanceFromCamera;
        float minRequiredFov = 2f * Mathf.Rad2Deg * Mathf.Atan(0.5f * canvasDiagonalSize / canvasDistance);
        minRequiredFov *= settings.FovMultiplierToAvoidEdgeClip;

        minRequiredFov = Mathf.Clamp(minRequiredFov, 10, 170);
        return minRequiredFov;
    }


}