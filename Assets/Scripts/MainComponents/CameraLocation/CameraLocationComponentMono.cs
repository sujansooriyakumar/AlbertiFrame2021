using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocationComponentMono : CameraLocationComponentBase
{
    Vector3 lastPosition;
    Quaternion lastRotation;
    Vector3 eulersOfPicture;
    AnchorLocation anchorLocation;
    Transform hmdFollowTransform;
    bool needToFinishPicture;

    protected override void FrameRegistered()
    {
        FrameEvents.OnPictureTaken += PictureTaken;
        FrameEvents.OnUpdateMainCameraLocation += UpdateTransform;
        FrameEvents.OnFoundAlbertiFrameComponent += ListenForRequiredComponents;
    }
    void ListenForRequiredComponents(AlbertiFrameComponent frameComponent)
    {
        switch (frameComponent)
        {
            case AnchorLocation foundAnchorLocation:
                anchorLocation = foundAnchorLocation;
                break;
            case HmdFollow foundHmdFollow:
                hmdFollowTransform = foundHmdFollow.transform;
                break;
        }
    }
    public void OnDisable()
    {
        FrameEvents.OnPictureTaken -= PictureTaken;
        FrameEvents.OnUpdateMainCameraLocation -= UpdateTransform;
        FrameEvents.OnFoundAlbertiFrameComponent -= ListenForRequiredComponents;
    }

    void PictureTaken()
    {
        needToFinishPicture = true;
    }
    void UpdateTransform()
    {

        if (needToFinishPicture)
        {
            Transform thisTransform = transform; //for optimization

            thisTransform.position = Frame.PictureLocation.CurrentPosition;
            thisTransform.LookAt(anchorLocation.transform);

            eulersOfPicture = thisTransform.eulerAngles;
        }

        UpdatePosition();
        UpdateRotation();
    }
    void UpdatePosition()
    {
        switch (Frame.ParallaxMode)
        {
            case ParallaxMode.On:
                {

                    Vector3 hmdPositionInFrameCoords = Frame.transform.InverseTransformPoint(hmdFollowTransform.position);
                    Vector3 hmdPositionScaledByGain = ScaleByGainIfNeeded(hmdPositionInFrameCoords);
                    transform.position = anchorLocation.transform.TransformPoint(hmdPositionScaledByGain);

                    break;
                }
            case ParallaxMode.Off:
                transform.position = Frame.PictureLocation.CurrentPosition;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    Vector3 ScaleByGainIfNeeded(Vector3 hmdPositionInFrameCoords)
    {
        if (Math.Abs(Frame.ParallaxGain - 1) < 0.001f) return hmdPositionInFrameCoords;

        Vector3 pictureLocationInFrameCoords =
            Frame.transform.InverseTransformPoint(Frame.PictureLocation.Position);
        Vector3 offsetVectorFromPictureToHmd = hmdPositionInFrameCoords - pictureLocationInFrameCoords;
        Vector3 scaledOffsetVector = offsetVectorFromPictureToHmd * Frame.ParallaxGain;
        Vector3 scaledHmdPositionByGain = pictureLocationInFrameCoords + scaledOffsetVector;
        return scaledHmdPositionByGain;
    }
    void UpdateRotation()
    {
        Transform thisTransformOptimized = transform;
        switch (Frame.ParallaxMode)
        {
            case ParallaxMode.On:
                {

                    //this will transform hm world rotation to Frame's local space
                    Quaternion localRotation = Quaternion.Inverse(Frame.transform.rotation) * hmdFollowTransform.rotation;
                    //Uses local rotation relative to anchor Location and puts it back to world space.
                    Quaternion worldRotation = anchorLocation.transform.rotation * localRotation;

                    transform.rotation = worldRotation;


                    //then look in right direction for optimization so whole view doesn't need to be rendered
                    transform.LookAt(anchorLocation.transform.position, transform.up);

                    break;
                }
            case ParallaxMode.Off:
                thisTransformOptimized.eulerAngles = eulersOfPicture;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
