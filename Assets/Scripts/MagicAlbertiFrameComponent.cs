using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class MagicAlbertiFrameComponent : MonoBehaviour, MagicAlbertiFrame
{
   public bool IsOn { get; private set; }

    [Tooltip("The Main Camera of the scene")]
    [SerializeField]

    public Camera mainVrCamera = default;
    public Camera MainCamera => mainVrCamera;

    [Tooltip("A transform from which pictures can be optionally captured. Think of this as a camera in the scene (no parallax mode")]
    [SerializeField]
    public Transform ViewPoint;

    [SerializeField]
    [Tooltip("Manually controls whether pictures are taken from HMD or from ViewPoint. Parallax mode overrides this")]
    public TakePictureFromObject defaultPictureFrom = TakePictureFromObject.Hmd;
    public TakePictureFromObject DefaultPictureFrom
    {
        get => defaultPictureFrom;
        set => defaultPictureFrom = value;
    }
    HmdFollow hmdFollow;
    public Transform HmdTransform => hmdFollow.transform;

    public float InterOcularDistance
    {
        get
        {
#pragma warning disable 618
            Vector3 positionRight = InputTracking.GetLocalPosition(XRNode.RightEye);
            Vector3 positionLeft = InputTracking.GetLocalPosition(XRNode.LeftEye);
#pragma warning restore 618
            float distance = Vector3.Distance(positionRight, positionLeft);
            AlbertiCameraManager.StereoLeftCameraInstance.Cam.stereoSeparation = distance / 2f;
            AlbertiCameraManager.StereoRightCameraInstance.Cam.stereoSeparation = distance / 2f;

            return AlbertiCameraManager.ActiveCamera.Cam.stereoSeparation * 2f;

        }
    }

    [SerializeField]
    public UpdateMode updateMode;
    public UpdateMode UpdateMode
    {
        get => updateMode;
        set => updateMode = value;
    }

    [SerializeField]
    public StereoMode stereoMode;
    public StereoMode StereoMode
    {
        get => stereoMode;
        set => stereoMode = value;
    }

    [SerializeField]
    public ParallaxMode parallaxMode = ParallaxMode.Off;
    public ParallaxMode ParallaxMode
    {
        get
        {
            if (UpdateMode == UpdateMode.Live && DefaultPictureFrom == TakePictureFromObject.Hmd)
                return parallaxMode;
            switch (parallaxMode)
            {
                case ParallaxMode.Off:
                    return ParallaxMode.Off;
                case ParallaxMode.On:
                    Debug.LogWarning($"{AlbertiLog.Prefix} ParallaxMode requires UpdateMode:Live and TakePictureFromObject:Hmd");
                    parallaxMode = ParallaxMode.Off;
                    return ParallaxMode.Off;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        set
        {
            parallaxMode = value;
            if (parallaxMode == ParallaxMode.Off) return;

            UpdateMode = UpdateMode.Live;
            DefaultPictureFrom = TakePictureFromObject.Hmd;
        }
    }

    [Layer]
    [SerializeField]
    [Tooltip("A layer for the actual frame mesh")]
    public int frameLayer = default;

    [Layer]
    [SerializeField]
    [Tooltip("A layer for VrObjects to not be rendered in the picture")]
    public int vrObjectLayer = default;

    [Layer]
    [SerializeField]
    [Tooltip("A layer to render only Mono Views")]
    public int monoLayer = default;

    [SerializeField]
    [Layer]
    [Tooltip("A layer to render the Stereo Left view")]
    public int stereoLeftLayer = default;

    [SerializeField]
    [Tooltip("A layer to render the Stereo Right view")]
    [Layer]
    public int stereoRightLayer = default;

    [SerializeField]
    [Tooltip("Layers to not be rendered in pictures")]
    [Layer]
    public List<int> ignoreLayers = new List<int>();

    public int MonoEyeLayer => monoLayer;
    public int StereoLeftLayer => stereoLeftLayer;
    public int StereoRightLayer => stereoRightLayer;
    public int FrameLayer => frameLayer;
    public int VrObjectLayer => vrObjectLayer;
    public List<int> IgnoreLayers => ignoreLayers;

    [SerializeField]
    [Range(0, 2)]
    public float parallaxGain = 1;
    public float ParallaxGain
    {
        get => parallaxGain;
        set => parallaxGain = value;
    }

    [SerializeField]
    [Range(0, 2)]
    public float interOcularDistanceGain = 1;
    public float InterOcularDistanceGain
    {
        get => interOcularDistanceGain;
        set => interOcularDistanceGain = value;
    }

    [Tooltip("Base Resolution of canvas. Increase if always pixelated (large performance hit)")]
    [Range(1, 20)]
    public float resolution = 12;
    public float Resolution => resolution;

    [Tooltip("Maximum size of the canvas. Increase if pixelated when very close to the canvas")]
    [Range(246, 10000)]
    public int maxMonoTextureSize = 4000;
    public int MaxMonoTextureSize => MaxMonoTextureSize;

    [Tooltip("Maximum size of the canvas. Increase if pixelated when very close to the canvas")]
    [Range(256, 1000)]
    public int maxStereoTextureSize = 7000;
    public int MaxStereoTextureSize => maxStereoTextureSize;

    public bool debugMode = false;

    public bool DebugMode => debugMode;

    [SerializeField]
    [Tooltip("A settings file")]
    public SettingsMain settingsFile = default;
    public SettingsMain Settings => settingsFile;

    CameraLocation cameraLocation;
    ProjectorLocation projectorLocation;

    public PictureLocation PictureLocation { get; private set; }

    Vector3 pictureEulers;

    bool notYetTurnedOn = true;
    bool firstTimeTurnedOn = false;

    public FrameEvents FrameEvents { get; private set; }
    bool NonStandardGainSelected => Math.Abs(InterOcularDistanceGain - 1f) < 0.001f || Math.Abs(ParallaxGain - 1f) > 0.001f;

    public void OnValidate()
    {
        Assert.IsNotNull(Settings);
    }

    public void OnEnable()
    {
        if (MainCamera == null) throw new NullReferenceException("Remember to set the Main Camera field in the inspector \n");
        if (FrameEvents == null) FrameEvents = new FrameEvents();

        ShowWarnings();

        FrameEvents.OnFoundAlbertiFrameComponent += ListenForRequiredComponents;

        ComponentFinder<AlbertiFrameComponent> componentFinder = new ComponentFinder<AlbertiFrameComponent>(this);
        componentFinder.FindAndBroadcastAllComponents();

        hmdFollow.UpdateParent();
        IsOn = false;
    }

    void ShowWarnings()
    {
        if (NonStandardGainSelected)
            Debug.LogWarning($"{AlbertiLog.Prefix} <color=orange><b>Non-standard gain selected.</b></color> " +
                             $"This could produce unwanted results. Use with caution. Edit in Alberti Frame Inspector." +
                             $"\n InterOcularDistanceGain:{InterOcularDistanceGain}, ParallaxGain:{ParallaxGain}");

        if (debugMode)
            Debug.LogWarning($"{AlbertiLog.Prefix} <color=orange><b>Debug mode on.</b></color> Each eye will have different background and helper objects will be drawn in scene view.");

        if (transform.parent != null)
            Debug.LogWarning($"{AlbertiLog.Prefix} <color=orange><b>Detected parent object (named:{transform.parent.name}).</b></color> " +
                             $"May produce unwanted results. Better: Un-parented, copying pos/rot from an empty object in desired hierarchy.");
    }

    void ListenForRequiredComponents(AlbertiFrameComponent frameComponent)
    {
        switch (frameComponent)
        {
            case HmdFollowComponent foundHmdFollow:
                hmdFollow = foundHmdFollow;
                break;
        }
    }

    [PublicAPI]
    [ContextMenu("ToggleOnOff")]
    public void ToggleOnOff()
    {
        if (!IsOn)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    [PublicAPI]
    public void TurnOff()
    {
        Debug.Log($"{AlbertiLog.Prefix} Turning Frame Off");
        IsOn = false;
        FrameEvents.TurnedOff();
        Update();
    }

    [PublicAPI]
    public void TurnOn()
    {

        Debug.Log($"{AlbertiLog.Prefix} Turning Frame On");
        IsOn = true;
        FrameEvents.TurnedOn();


        Update();

        if (notYetTurnedOn)
        {
            firstTimeTurnedOn = true;
            notYetTurnedOn = false;
        }
    }

    public Transform ActiveCamera => AlbertiCameraManager.ActiveCamera.transform;

    [PublicAPI]
    [ContextMenu("TakePictureFromHMD")]
    public void TakePictureFromHmd()
    {
        if (HmdTransform == null) throw new NullReferenceException("Taking picture from HmdFollowComponent locationm but HmdFollowComponent object is null");

        PictureLocation = new PictureLocation(this, HmdTransform.position);
        FinishTakingPicture();
    }
    [PublicAPI]
    [ContextMenu("TakePictureFromViewPoint")]
    public void TakePictureFromViewPoint()
    {
        PictureLocation = new PictureLocation(this, ViewPoint.transform.position);
        FinishTakingPicture();
    }

    /// <summary>
    /// Takes a Picture from specified location
    /// </summary>
    /// <param name="pictureLocationTransform"></param>
    [PublicAPI]
    public void TakePictureFrom(Transform pictureLocationTransform)
    {
        PictureLocation = new PictureLocation(this, pictureLocationTransform.position);
        FinishTakingPicture();
    }

    /// <summary>
    /// Takes Picture depending on state of DefaultPictureFrom variable.
    ///
    /// </summary>
    [PublicAPI]
    public void TakePicture()
    {
        switch (DefaultPictureFrom)
        {
            case TakePictureFromObject.Hmd:
                TakePictureFromHmd();
                break;
            case TakePictureFromObject.ViewPoint:
                TakePictureFromViewPoint();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void FinishTakingPicture()
    {
        Debug.Log($"{AlbertiLog.Prefix} Taking Picture");
        hmdFollow.UpdateParent();
        FrameEvents.PictureTaken();
        UpdateComponentsInOrder();
    }


    public void Update()
    {

        if (firstTimeTurnedOn)
        {
            Debug.Log($"{AlbertiLog.Prefix} Turned on for first time, taking picture automatically.");
            TakePicture();
            firstTimeTurnedOn = false;
        }

        if (PictureLocation == null) return;
        UpdateComponentsInOrder();


    }

    void UpdateComponentsInOrder()
    {
        //Update components in specific order to prevent order problems
        FrameEvents.UpdateMainAnchorLocation();
        FrameEvents.UpdateMainCameraLocation();
        FrameEvents.UpdateMainProjectorLocation();
        FrameEvents.UpdateMounts();
    }


    public void RaiseFoundComponentEvent(FindableComponent findableComponent)
    {
        FrameEvents.FoundAlbertiFrameComponent(findableComponent as AlbertiFrameComponent);
    }
}
