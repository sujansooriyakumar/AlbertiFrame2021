using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectorLocationComponentStereo : FrameComponentBase, ProjectorLocation
{
    protected abstract void AddOffSet();

    protected override void FrameRegistered()
    {
        Frame.FrameEvents.OnUpdateMainProjectorLocation += UpdateThis;
    }

    void OnDisable()
    {
        Frame.FrameEvents.OnUpdateMainProjectorLocation -= UpdateThis;
    }

    void UpdateThis()
    {
        AddOffSet();
        Vector3 oldEulers = transform.localEulerAngles;
        //transform.localEulerAngles = new Vector3(oldEulers.x, oldEulers.y, -Frame.transform.localEulerAngles.z);
    }

    void OnDrawGizmos()
    {
        if (Frame == null) return;
        if (!Frame.DebugMode) return;
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, 7, .2f, 0, 1);
        Gizmos.DrawSphere(Vector3.zero, .0075f);
    }
}