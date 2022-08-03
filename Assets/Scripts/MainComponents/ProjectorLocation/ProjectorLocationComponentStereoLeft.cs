using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorLocationComponentStereoLeft : ProjectorLocationComponentStereo
{

    protected override void AddOffSet()
    {
        transform.localPosition = new Vector3(-Frame.InterOcularDistance / 2f, 0, 0);
    }
}