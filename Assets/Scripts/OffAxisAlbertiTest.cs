using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffAxisAlbertiTest : MonoBehaviour
{
    private void Start()
    {
        Camera Cam;
        Cam = GetComponent<Camera>();
        int cullingMask = Cam.cullingMask;
        CameraClearFlags clearFlags = Cam.clearFlags;

        Cam.cullingMask = 0;
        Cam.clearFlags = CameraClearFlags.Nothing;
    }
}
