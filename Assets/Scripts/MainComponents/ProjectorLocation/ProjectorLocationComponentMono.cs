using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorLocationComponentMono : FrameComponentBase, ProjectorLocation
{

    Quaternion localRotationWhenPictureTakenInFrameCoords;

    HmdFollow hmdFollow;
    Vector3 localPositionWhenPictureTakenInFrameCords;
    float zWhenPitureTaken;
    Vector3 frameRotationWhenPictureTaken;
    Quaternion thisLocalRoation;
    float offsetAngle;
    CameraLocationComponentMono cameraLocationComponentMono;


    protected override void FrameRegistered()
    {
        FrameEvents.OnPictureTaken += PictureTaken;
        FrameEvents.OnPictureCleared += PictureCleared;
        FrameEvents.OnUpdateMainProjectorLocation += UpdateTransform;
        FrameEvents.OnFoundAlbertiFrameComponent += ListenForRequiredComponents;
    }

    void ListenForRequiredComponents(AlbertiFrameComponent frameComponent)
    {
        if (frameComponent is HmdFollow foundHmdFollow)
        {
            hmdFollow = foundHmdFollow;
        }

        if (frameComponent is CameraLocationComponentMono foundCameraLocationComponentMono)
        {
            cameraLocationComponentMono = foundCameraLocationComponentMono;
        }
    }

    public void OnDisable()
    {
        FrameEvents.OnPictureTaken -= PictureTaken;
        FrameEvents.OnPictureCleared -= PictureCleared;
        FrameEvents.OnUpdateMainProjectorLocation -= UpdateTransform;
        FrameEvents.OnFoundAlbertiFrameComponent -= ListenForRequiredComponents;

    }

    void PictureTaken()
    {

        Transform thisTransform = transform; // Cached for Optimization

        thisTransform.position = Frame.PictureLocation.CurrentPosition;
        thisTransform.LookAt(Frame.transform);

        offsetAngle = Frame.transform.localEulerAngles.z;


        localPositionWhenPictureTakenInFrameCords = Frame.transform.InverseTransformPoint(thisTransform.position);
        localRotationWhenPictureTakenInFrameCoords = Frame.transform.ConvertWorldRotationToThisLocalSpace(transform.rotation);



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

    void PictureCleared()
    {
        transform.eulerAngles = hmdFollow.transform.eulerAngles;
    }

    void UpdateTransform()
    {
        UpdatePosition();
        UpdateRotation();
    }

    void UpdatePosition()
    {
        switch (Frame.ParallaxMode)
        {
            case ParallaxMode.On:
                var hmdPosition = hmdFollow.transform.position;
                Vector3 scaledHmdPosition = ScaleByGainIfNeeded(hmdPosition);
                transform.position = scaledHmdPosition;

                break;
            case ParallaxMode.Off:
                transform.position = Frame.transform.TransformPoint(localPositionWhenPictureTakenInFrameCords);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    void UpdateRotation()
    {

        switch (Frame.ParallaxMode)
        {
            case ParallaxMode.On:
                transform.rotation = hmdFollow.transform.rotation;

                //then look in right direction for optimization so whole view doesn't need to be rendered
                transform.LookAt(Frame.transform.position, transform.up);
                break;
            case ParallaxMode.Off:
                transform.rotation = Frame.transform.ConvertLocalRotationToWorldSpace(localRotationWhenPictureTakenInFrameCoords);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    Vector3 ScaleByGainIfNeeded(Vector3 hmdPositionInWorldCoords)
    {
        if (Math.Abs(Frame.ParallaxGain - 1) < 0.001f) return hmdPositionInWorldCoords;


        Vector3 FrameLocalPictureLocationInWorldCoords = Frame.transform.TransformPoint(Frame.PictureLocation.PositionInLocalCoordsAtTimeOfPicture);
        Vector3 offsetVectorFromPictureToHmd = hmdPositionInWorldCoords - FrameLocalPictureLocationInWorldCoords;
        Vector3 scaledOffsetVector = offsetVectorFromPictureToHmd * Frame.ParallaxGain;
        Vector3 FromHMDVector = scaledOffsetVector - offsetVectorFromPictureToHmd;
        Vector3 scaledHmdPositionInWorldCoords = hmdPositionInWorldCoords + FromHMDVector;
        return scaledHmdPositionInWorldCoords;
    }

}