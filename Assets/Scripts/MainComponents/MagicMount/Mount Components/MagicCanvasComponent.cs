using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCanvasComponent : MountComponentBase, MagicCanvas
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    void SetTargetLayer()
    {
        gameObject.SetLayerRecursively(Mount.TargetEyeLayer);
    }

    void Hide()
    {
        meshRenderer.gameObject.SetActive(false);
    }

    void Show()
    {
        meshRenderer.gameObject.SetActive(true);
    }

    protected override void MountRegistered()
    {
        MountEvents.OnMountActivated += Show;
        MountEvents.OnMountDeactivated += Hide;

        if (meshRenderer == null) meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshFilter == null) meshFilter = GetComponentInChildren<MeshFilter>();

        Hide();

        SetTargetLayer();
    }

    public void OnDisable()
    {
        MountEvents.OnMountActivated -= Show;
        MountEvents.OnMountDeactivated -= Hide;
    }

    public float DiagonalSize
    {
        get
        {
            float size = meshRenderer.bounds.size.magnitude;
            return size;
        }
    }
}
