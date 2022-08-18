using OffAxisCamera;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MagicOffAxisCameraRigComponent : MountComponentBase, MagicOffAxisCameraRig
{
    int shadowTex;

    OffAxisCameraRig offAxisCameraRig;
    Camera offAxisCamera;

    public OffAxisCameraRig OffAxisCameraRig
    {
        get
        {
            if (offAxisCameraRig == null) offAxisCameraRig = GetComponentInChildren<OffAxisCameraRig>();
            return offAxisCameraRig;
        }

        set => offAxisCameraRig = value;
    }

    public Camera OffAxisCamera
    {
        get
        {
            if (offAxisCamera == null) offAxisCamera = offAxisCameraRig.GetComponentInChildren<Camera>();
            return offAxisCamera;
        }

        set => offAxisCamera = value;
    }

    public float FieldOfView
    {
        get => OffAxisCamera.fieldOfView;
        set => OffAxisCamera.fieldOfView = value;
    }

    void OnValidate()
    {
        Assert.IsNotNull(OffAxisCameraRig);
    }

    void SetTargetLayer()
    {
        OffAxisCamera.cullingMask = -1;
        OffAxisCamera.cullingMask &= ~(1 << Mount.TargetEyeLayer);
    }

    void UpdateWith(ProjectorLocation projectorLocation)
    {
        Transform projectorLocationTransform = projectorLocation.transform;
        var thisTransfromOptimized = transform;
        thisTransfromOptimized.position = projectorLocationTransform.position;
        thisTransfromOptimized.eulerAngles = projectorLocationTransform.eulerAngles;
    }

    void MountActivated()
    {
        OffAxisCamera.enabled = true;
    }

    void MountDeactivated()
    {
        OffAxisCamera.enabled = false;
    }

    protected override void MountRegistered()
    {
        MountEvents.OnUpdateProjector += UpdateWith;
        MountEvents.OnMountActivated += MountActivated;
        MountEvents.OnMountDeactivated += MountDeactivated;
        FrameEvents.OnTurnOff += MountDeactivated;

        shadowTex = Shader.PropertyToID("_ShadowTex");

        if(OffAxisCameraRig == null)
        {
            throw new MissingComponentException("Missing Off Axis Camera Rig component");
        }
        // unsure if this is neccessary 
        // Projectior.material = new Material(Shader.Find("Projector/Multiply"));
        SetTargetLayer();
    }

    void OnDisable()
    {
        MountEvents.OnUpdateProjector -= UpdateWith;

        MountEvents.OnMountActivated -= MountActivated;
        MountEvents.OnMountDeactivated -= MountDeactivated;
        FrameEvents.OnTurnOff -= MountDeactivated;
    }

    public void SetMaterialTexture(RenderTexture texture)
    {
        if (OffAxisCamera.targetTexture == null) throw new NullReferenceException("Render material is null");
        // we should set the target texture here but i dont think its neccessary. we've set it in the editor.
    }
}
