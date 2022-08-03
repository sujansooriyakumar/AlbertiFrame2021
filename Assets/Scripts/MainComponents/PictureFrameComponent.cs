using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureFrameComponent : FrameComponentBase
{
    protected override void FrameRegistered()
    {
        if (Frame.FrameLayer == 0) throw new LayerNotSetException("FrameLayer is still set to default");
        gameObject.SetLayerRecursively(Frame.FrameLayer);
    }

}
