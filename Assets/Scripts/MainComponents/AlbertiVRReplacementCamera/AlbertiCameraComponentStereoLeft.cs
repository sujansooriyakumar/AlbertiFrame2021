using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbertiCameraComponentStereoLeft : AlbertiCameraComponentBase
{

    bool IAmNotTheOnlyStereoLeftCamera => AlbertiCameraManager.StereoLeftCameraInstance != this;

    protected override void SetupSpecificLayers()
    {
        AddCullingSettingsToCamera(Cam);
        if (Frame.DebugMode)
        {
            Cam.clearFlags = CameraClearFlags.SolidColor;
            Cam.backgroundColor = Color.blue;
        }
    }

    protected override void CheckIfCameraAlreadyExistsInScene()
    {

        if (AlbertiCameraManager.StereoLeftNotYetSet)
        {
            AlbertiCameraManager.StereoLeftCameraInstance = this;
        }
        else
        {
            if (IAmNotTheOnlyStereoLeftCamera)
            {
                Debug.Log(AlbertiCameraManager.StereoLeftCameraInstance.gameObject.name);
                DestroySelfBecauseThereCanOnlyBeOne();
            }
        }

    }

    void DestroySelfBecauseThereCanOnlyBeOne()
    {
        //pass settings on to the only camera before it kills itself
        AddCullingSettingsToCamera(AlbertiCameraManager.StereoLeftCameraInstance.Cam);
        DestroySelf();
    }

    void AddCullingSettingsToCamera(Camera leftCamera)
    {
        leftCamera.RenderLayer(Frame.StereoLeftLayer);
        leftCamera.CullLayer(Frame.StereoRightLayer);
        leftCamera.RenderLayer(Frame.MonoEyeLayer);
    }
}