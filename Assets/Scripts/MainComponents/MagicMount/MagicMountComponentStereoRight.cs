using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMountComponentStereoRight : MagicMountComponentBase
{

    protected override void ListenForRequiredFrameComponents(AlbertiFrameComponent foundFrameComponent)
    {
        switch (foundFrameComponent)
        {
            case CameraLocationComponentStereoRight foundRightCameraLocation:
                CameraLocation = foundRightCameraLocation;
                break;
            case ProjectorLocationComponentStereoRight foundRightProjectorLocation:
                ProjectorLocation = foundRightProjectorLocation;
                break;
            case AnchorLocation foundAnchorLocation:
                anchorLocation = foundAnchorLocation;
                break;
        }
    }

    public override int TargetEyeLayer => Frame.StereoRightLayer;



    protected override bool ShouldBeRendering => Frame.IsOn && Frame.StereoMode == StereoMode.Stereo;
}