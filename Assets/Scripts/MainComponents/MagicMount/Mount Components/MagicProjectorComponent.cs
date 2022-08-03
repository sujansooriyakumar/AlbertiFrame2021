using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MagicProjectorComponent : MountComponentBase, MagicProjector
{
    int shadowTex;

    Projector projector;

    public Projector Projector
    {
        get
        {
            if (projector == null) projector = GetComponentInChildren<Projector>();
            return projector;
        }
        set => projector = value;
    }

    public float FieldOfView
    {
        get => Projector.fieldOfView;
        set => Projector.fieldOfView = value;
    }
    void OnValidate()
    {
        Assert.IsNotNull(Projector);
    }

    void SetTargetLayer()
    {
        Projector.IgnoreAllLayers();
        Projector.TargetLayer(Mount.TargetEyeLayer);

    }
    void UpdateWith(ProjectorLocation projectorLocation)
    {
        Transform projectorLocationTransform = projectorLocation.transform;
        var thisTransformOptimized = transform;
        thisTransformOptimized.position = projectorLocationTransform.position;
        thisTransformOptimized.eulerAngles = projectorLocationTransform.eulerAngles;
    }
    void MountActivated()
    {
        Projector.enabled = true;
    }

    void MountDeactivated()
    {
        Projector.enabled = false;
    }
    protected override void MountRegistered()
    {

        MountEvents.OnUpdateProjector += UpdateWith;

        MountEvents.OnMountActivated += MountActivated;
        MountEvents.OnMountDeactivated += MountDeactivated;
        FrameEvents.OnTurnOff += MountDeactivated;

        shadowTex = Shader.PropertyToID("_ShadowTex");

        if (Projector == null) throw new MissingComponentException("missing projector object");

        Projector.material = new Material(Shader.Find("Projector/Multiply"));

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
        if (Projector.material == null) throw new NullReferenceException("projector material null");
        Projector.material.SetTexture(shadowTex, texture);
    }
}
