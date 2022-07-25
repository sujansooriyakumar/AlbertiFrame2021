using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class AlbertiCameraComponentBase : FrameComponentBase
{
    Camera vrCamera;
    Camera cam;

    public Camera VrCamera
    {
        get
        {
            if(vrCamera == null)
            {
                vrCamera = Frame.MainCamera;
                if(vrCamera == null)
                {
                    throw new MissingReferenceException("Base frame has null vrCamera object");
                }
            }
            return vrCamera;
        }
        set => vrCamera = value;
    }

    public Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = GetComponent<Camera>();
                if (cam == null)
                    throw new MissingReferenceException("Base frame has null vrCamera Object");
            }
            return cam;
        }
        set => cam = value;
    }

    protected abstract void SetupSpecificLayers();
    protected abstract void CheckIfCameraAlreadyExistsInScene();

    protected override void FrameRegistered()
    {
        transform.SetParent(Frame.MainCamera.transform.parent);

        VrCamera.enabled = false;
        Cam.enabled = true;

        Cam.cullingMask = vrCamera.cullingMask;
        SetupSpecificLayers();

        CheckIfCameraAlreadyExistsInScene();
    }

    private void Update()
    {
        transform.localScale = Vector3.one;
    }

    protected void DestroySelf()
    {
        if (!Application.isEditor)
        {
            Destroy(gameObject);
        }
    }
}