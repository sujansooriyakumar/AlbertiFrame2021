using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMountComponentStereoLeft : MagicMountComponentBase
{
    protected override void ListenForRequiredFrameComponents(AlbertiFrameComponent foundFrameComponent)
    {
        switch (foundFrameComponent)
        {
            case CameraLocationComponentStereoLeft foundLeftCameraLocation:
                CameraLocation = foundLeftCameraLocation;
                break;
            case ProjectorLocationComponentStereoLeft foundLeftProjectorLocation:
                ProjectorLocation = foundLeftProjectorLocation;
                break;
            case AnchorLocation foundAnchorLocation:
                anchorLocation = foundAnchorLocation;
                break;
        }
    }

    public override int TargetEyeLayer => Frame.StereoLeftLayer;


    protected override bool ShouldBeRendering => Frame.IsOn && Frame.StereoMode == StereoMode.Stereo;


}