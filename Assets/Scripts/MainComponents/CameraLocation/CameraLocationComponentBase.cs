using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraLocationComponentBase : FrameComponentBase, CameraLocation
{
    void OnDrawGizmos()
    {
        if (Frame == null) return;
        if (!Frame.DebugMode) return;
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, 5, .2f, 0, 1);
        Gizmos.DrawSphere(Vector3.zero, 0.005f);
    }
}