using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureLocation : MonoBehaviour
{
    readonly Vector3 locationOfPictureInParentSpace;
    readonly MagicAlbertiFrame frame;
    public Vector3 Position { get; }

    public Vector3 PositionInLocalCoordsAtTimeOfPicture { get; private set; }

    public PictureLocation(MagicAlbertiFrame frame, Vector3 pictureLocation)
    {
        this.frame = frame;
        Position = pictureLocation;
        Transform parentTransform = frame.transform.parent;

        locationOfPictureInParentSpace = parentTransform != null ?
            parentTransform.InverseTransformPoint(Position) : Position;
    }

    public Vector3 CurrentPosition =>
        frame.transform.parent != null ?
        frame.transform.parent.TransformPoint(locationOfPictureInParentSpace) :
        locationOfPictureInParentSpace;

    public void UpdateReferenceViewPointOfHmd(Transform hmdTransform)
    {
        PositionInLocalCoordsAtTimeOfPicture = frame.transform.InverseTransformPoint(hmdTransform.position);
    }
}

