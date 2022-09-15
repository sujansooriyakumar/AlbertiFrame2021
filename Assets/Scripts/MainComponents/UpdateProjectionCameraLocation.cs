using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProjectionCameraLocation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MagicAlbertiFrameComponent Frame;
    [SerializeField] HmdFollowComponent hmdFollow;
    Vector3 localPositionWhenPictureTakenInFrameCords;
    Quaternion localRotationWhenPictureTakenInFrameCoords;

    private void Update()
    {
        Transform thisTransform = transform;
        thisTransform.position = Frame.PictureLocation.CurrentPosition;
        thisTransform.LookAt(Frame.transform);


        localPositionWhenPictureTakenInFrameCords = Frame.transform.InverseTransformPoint(thisTransform.position);
        localRotationWhenPictureTakenInFrameCoords = Frame.transform.ConvertWorldRotationToThisLocalSpace(transform.rotation);
        UpdatePosition();
        UpdateRotation();
    }

    void UpdatePosition()
    {
        switch (Frame.ParallaxMode)
        {
            case ParallaxMode.On:
                var hmdPosition = hmdFollow.transform.position;
                transform.position = hmdPosition;

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
}
