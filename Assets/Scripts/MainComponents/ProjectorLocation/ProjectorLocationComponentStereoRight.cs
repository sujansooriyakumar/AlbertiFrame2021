using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorLocationComponentStereoRight : ProjectorLocationComponentStereo
{

    protected override void AddOffSet()
    {
        transform.localPosition = new Vector3(Frame.InterOcularDistance / 2f, 0, 0);
    }
}