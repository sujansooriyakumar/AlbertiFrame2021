using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorLocationComponent : FrameComponentBase, AnchorLocation
{
    Vector3 anchoredPositionRelativeToBaseParent;

    public Vector3 AnchoredPositionRelativeToBaseParent => anchoredPositionRelativeToBaseParent;

    bool anchored;

    HmdFollow hmdFollow;
    CameraLocationComponentMono monoCameraLocation;
    Vector3 anchoredWorldPosition;
    Quaternion anchoredWorldRotation;

    public bool Anchored => anchored;

    protected override void FrameRegistered()
    {
        Frame.FrameEvents.OnPictureTaken += SetAnchored;
        FrameEvents.OnPictureCleared += Clear;
        FrameEvents.OnUpdateMainAnchorLocation += UpdateTransform;
        FrameEvents.OnFoundAlbertiFrameComponent += ListenForComponents;
    }

    void ListenForComponents(AlbertiFrameComponent frameComponent)
    {
        if(frameComponent is HmdFollow foundHmdFollow)
        {
            hmdFollow = foundHmdFollow;
        }

        if(frameComponent is CameraLocationComponentMono foundMonoCameraLocation)
        {
            monoCameraLocation = foundMonoCameraLocation;
        }
    }

    public void OnDisable()
    {
        Frame.FrameEvents.OnPictureTaken -= SetAnchored;
        Frame.FrameEvents.OnPictureCleared -= Clear;
        Frame.FrameEvents.OnUpdateMainAnchorLocation -= UpdateTransform;
        Frame.FrameEvents.OnFoundAlbertiFrameComponent -= ListenForComponents;
    }

    void OnDrawGizmos()
    {
        if (Frame == null) return;
        if (!Frame.DebugMode) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(.5f, .5f, .01f));
    }

    void SetAnchored()
    {
        anchoredPositionRelativeToBaseParent = Frame.transform.ConvertPositionToParentLocalSpace();
        anchoredWorldPosition = Frame.transform.position;
        anchoredWorldRotation = Frame.transform.rotation;
        anchored = true;
    }
    void UpdateTransform()
    {
        if (anchored)
        {
            transform.position = anchoredWorldPosition;
            transform.rotation = anchoredWorldRotation;
        }
        else
        {
            transform.ResetLocal();
        }
    }
    void Clear()
    {
        anchored = false;
    }
}
