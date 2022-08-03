using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicMountComponentBase : FrameComponentBase, MagicMount
{

    public MountEvents MountEvents { get; private set; }

    protected CameraLocation CameraLocation;
    protected ProjectorLocation ProjectorLocation;

    GameObject attachedTo;
    Transform hmd;

    public MagicRenderer MagicRenderer { get; set; }
    public MountDistanceCalculator MountDistanceCalculator { get; set; }

    bool renderOnce = false;
    MagicCamera magicCamera;
    MagicProjector magicProjector;
    MagicCanvas magicCanvas;
    protected AnchorLocation anchorLocation;
    float minRequiredFov;


    public abstract int TargetEyeLayer { get; }
    MagicAlbertiFrame MagicMount.Frame => Frame;


    protected override void FrameRegistered()
    {

        if (MountEvents == null) MountEvents = new MountEvents();

        FrameEvents.OnPictureTaken += PictureTaken;
        FrameEvents.OnUpdateMounts += UpdateMount;
        FrameEvents.OnFoundAlbertiFrameComponent += ListenForRequiredFrameComponents;

        MountEvents.OnFoundMountComponent += ListenForRequiredMountComponents;
        ComponentFinder<MountFindableComponent> mountComponentFinder = new ComponentFinder<MountFindableComponent>(this);
        mountComponentFinder.FindAndBroadcastAllComponents();
    }

    void ListenForRequiredMountComponents(MountFindableComponent mountFindableComponent)
    {
        switch (mountFindableComponent)
        {
            case MagicCamera foundMagicCamera:
                magicCamera = foundMagicCamera;
                break;
            case MagicProjector foundMagicProjector:
                magicProjector = foundMagicProjector;
                break;
            case MagicCanvas foundMagicCanvas:
                magicCanvas = foundMagicCanvas;
                break;
        }

        if (AllRequiredComponentsFound) FinalizeSetup();
    }

    bool AllRequiredComponentsFound => magicCamera != null && magicCanvas != null && magicProjector != null;

    void FinalizeSetup()
    {
        if (MountDistanceCalculator == null)
            MountDistanceCalculator = new MountDistanceCalculatorUtility(magicCamera.transform, magicCanvas.transform, Frame.Settings.Texture);
        if (MagicRenderer == null)
            MagicRenderer = new MagicRendererUtility(magicCamera.Cam, Frame.Settings);
    }

    protected abstract void ListenForRequiredFrameComponents(AlbertiFrameComponent foundFrameComponent);

    public void OnDrawGizmos()
    {
        if (Frame == null) return;
        if (!Frame.DebugMode) return;
        if (magicCamera == null ||
            magicProjector == null ||
            magicCanvas == null ||
            anchorLocation == null)
            return;

        float duration = Frame.ParallaxMode == ParallaxMode.On ? 0.01f : 5f;

        Debug.DrawLine(magicProjector.transform.position,
                           magicCanvas.transform.position, Color.green, duration, false);
        Debug.DrawLine(magicCamera.transform.position,
                       anchorLocation.transform.position, Color.yellow, duration, false);
    }

    public void OnDisable()
    {
        FrameEvents.OnPictureTaken -= PictureTaken;
        FrameEvents.OnUpdateMounts -= UpdateMount;
        FrameEvents.OnFoundAlbertiFrameComponent -= ListenForRequiredFrameComponents;
    }

    void PictureTaken()
    {
        renderOnce = true;

    }

    void UpdateMount()
    {
        if (!ShouldBeRendering)
        {
            MountEvents.MountDeactivated();
            return;
        }

        MountEvents.MountActivated();
        MountEvents.UpdateCamera(CameraLocation);
        MountEvents.UpdateProjector(ProjectorLocation);

        // ReSharper disable once InvertIf
        if (Frame.UpdateMode == UpdateMode.Live || renderOnce)
        {
            RenderPicture();
            renderOnce = false;
        }
    }

    protected abstract bool ShouldBeRendering { get; }

    public void RenderPicture()
    {

        if (renderOnce || Frame.ParallaxMode == ParallaxMode.On)
        {
            minRequiredFov = MountDistanceCalculator.GetMinimumRequiredFieldOfView(magicCanvas.DiagonalSize);
        }

        magicCamera.FieldOfView = minRequiredFov;
        magicProjector.FieldOfView = minRequiredFov;

        Vector3 magicCanvasPosition = magicCanvas.transform.position;
        Vector3 currentCameraLocation = AlbertiCameraManager.ActiveCamera.transform.position;

        Vector3 canvasNormal = Frame.transform.forward;
        Vector3 hmdToCameraVector = currentCameraLocation - Frame.transform.position;
        float dotProduct = Vector3.Dot(canvasNormal, hmdToCameraVector);
        float canvasDistance = Vector3.Distance(currentCameraLocation, magicCanvasPosition);

        int maxTextureSize = FindMaxTextureSize();

        RenderTexture texture =
            MagicRenderer.GetRenderTexture(canvasDistance, minRequiredFov, dotProduct, maxTextureSize, Frame.Resolution);

        SendTextureToProjector(texture);

    }

    int FindMaxTextureSize()
    {
        int maxTextureSize;
        switch (Frame.StereoMode)
        {
            case StereoMode.Stereo:
                maxTextureSize = Frame.MaxStereoTextureSize;
                break;
            case StereoMode.Mono:
                maxTextureSize = Frame.MaxMonoTextureSize;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Frame.StereoMode), Frame.StereoMode, null);
        }

        return maxTextureSize;
    }


    void SendTextureToProjector(RenderTexture texture)
    {
        magicProjector.SetMaterialTexture(texture);
    }


    public void RaiseFoundComponentEvent(FindableComponent findableComponent)
    {
        MountEvents.FoundMountComponent(findableComponent as MountFindableComponent);
    }
}