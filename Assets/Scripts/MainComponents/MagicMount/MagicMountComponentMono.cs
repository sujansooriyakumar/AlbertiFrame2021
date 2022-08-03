using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMountComponentMono : MagicMountComponentBase
{
    protected override void ListenForRequiredFrameComponents(AlbertiFrameComponent foundFrameComponent)
    {
        switch (foundFrameComponent)
        {
            case CameraLocationComponentMono foundMonoCamera:
                CameraLocation = foundMonoCamera;
                break;
            case ProjectorLocationComponentMono foundMonoProjector:
                ProjectorLocation = foundMonoProjector;
                break;
            case AnchorLocation foundAnchorLocation:
                anchorLocation = foundAnchorLocation;
                break;
        }
    }

    public override int TargetEyeLayer => Frame.MonoEyeLayer;

    protected override bool ShouldBeRendering => Frame.IsOn && Frame.StereoMode == StereoMode.Mono;
}