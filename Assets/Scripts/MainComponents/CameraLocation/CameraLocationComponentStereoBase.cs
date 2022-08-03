using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraLocationComponentStereoBase : CameraLocationComponentBase
{
    float initialFrameAngle;

    protected abstract void AddOffset();

    protected override void FrameRegistered()
    {
        Frame.FrameEvents.OnUpdateMainCameraLocation += UpdateThis;
        Frame.FrameEvents.OnPictureTaken += PictureTaken;
    }

    void PictureTaken()
    {
    }

    void UpdateThis()
    {
        AddOffset();
    }
}