using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MagicCameraComponent : MountComponentBase, MagicCamera
{
    void OnValidate()
    {
        Assert.IsNotNull(Cam);
    }

    public float FieldOfView
    {
        get => Cam.fieldOfView;
        set => Cam.fieldOfView = value;
    }

    Camera cam;

    public Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = GetComponentInChildren<Camera>();
                if (cam == null) throw new MissingComponentException("MagicCamera can't find child Camera object");
            }
            return cam;
        }
        set => cam = value;
    }


    void UpdateWith(CameraLocation cameraLocation)
    {
        transform.position = cameraLocation.transform.position;
        transform.rotation = cameraLocation.transform.rotation;
    }

    void MountActivated()
    {
        Cam.gameObject.SetActive(true);
    }

    void MountDeactivated()
    {
        Cam.gameObject.SetActive(false);
    }

    protected override void MountRegistered()
    {
        Cam.aspect = 1;
        Cam.enabled = false;
        MountDeactivated();
        SetupCameraLayers();

        MountEvents.OnUpdateCamera += UpdateWith;
        MountEvents.OnMountActivated += MountActivated;
        MountEvents.OnMountDeactivated += MountDeactivated;

        FrameEvents.OnTurnOff += MountDeactivated;
    }

    public void OnDisable()
    {
        MountEvents.OnUpdateCamera -= UpdateWith;
        MountEvents.OnMountActivated -= MountActivated;
        MountEvents.OnMountDeactivated -= MountDeactivated;

        FrameEvents.OnTurnOff -= MountDeactivated;
    }


    void SetupCameraLayers()
    {
        CheckLayersSetProperly();

        Cam.CullLayer(Frame.FrameLayer);
        Cam.CullLayer(Frame.MonoEyeLayer);
        Cam.CullLayer(Frame.StereoLeftLayer);
        Cam.CullLayer(Frame.StereoRightLayer);
        Cam.CullLayer(Frame.VrObjectLayer);

        CullIgnoredLayers();
    }

    void CheckLayersSetProperly()
    {
        if (Frame.FrameLayer == 0) throw new LayerNotSetException("FrameLayer not set");
        if (Frame.MonoEyeLayer == 0) throw new LayerNotSetException("MonoEyeLayer not set");
        if (Frame.StereoLeftLayer == 0) throw new LayerNotSetException("StereoLeftLayer not set");
        if (Frame.StereoRightLayer == 0) throw new LayerNotSetException("StereoRightLayer not set");
        if (Frame.VrObjectLayer == 0) throw new LayerNotSetException("VrObjectLayer not set");
    }

    void CullIgnoredLayers()
    {
        if (Frame.IgnoreLayers == null || Frame.IgnoreLayers.Count <= 0) return;
        foreach (int layerToCull in Frame.IgnoreLayers)
        {
            Cam.CullLayer(layerToCull);
        }
    }
}