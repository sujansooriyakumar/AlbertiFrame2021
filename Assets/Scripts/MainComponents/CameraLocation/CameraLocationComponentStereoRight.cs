using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocationComponentStereoRight : CameraLocationComponentStereoBase
{
    protected override void AddOffset()
    {
        transform.localPosition = new Vector3(Frame.InterOcularDistance / 2, 0, 0);
    }


}