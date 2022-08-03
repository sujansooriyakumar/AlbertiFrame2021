using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbertiCameraComponentStereoRight : AlbertiCameraComponentBase
{

    bool IAmNotTheOnlyStereoRightCamera => AlbertiCameraManager.StereoRightCameraInstance != this;

    protected override void SetupSpecificLayers()
    {
        AddCullingSettingsToCamera(Cam);
        if (Frame.DebugMode)
        {
            Cam.clearFlags = CameraClearFlags.SolidColor;
            Cam.backgroundColor = Color.red;
        }
    }

    protected override void CheckIfCameraAlreadyExistsInScene()
    {

        if (AlbertiCameraManager.StereoRightNotYetSet)
        {
            AlbertiCameraManager.StereoRightCameraInstance = this;
        }
        else
        {
            if (IAmNotTheOnlyStereoRightCamera)
            {
                DestroySelfBecauseThereCanOnlyBeOne();
            }
        }
    }

    void DestroySelfBecauseThereCanOnlyBeOne()
    {
        //pass settings on to the only camera before it kills itself
        AddCullingSettingsToCamera(AlbertiCameraManager.StereoRightCameraInstance.Cam);
        DestroySelf();
    }

    void AddCullingSettingsToCamera(Camera rightCamera)
    {
        rightCamera.CullLayer(Frame.StereoLeftLayer);
        rightCamera.RenderLayer(Frame.StereoRightLayer);
        rightCamera.RenderLayer(Frame.MonoEyeLayer);
    }

}