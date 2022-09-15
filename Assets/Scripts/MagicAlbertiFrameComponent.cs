using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem.XR;
using UnityEngine.Serialization;
using UnityEngine.XR;

[SelectionBase]
public class MagicAlbertiFrameComponent : MonoBehaviour, MagicAlbertiFrame
{

    [SerializeField] GameObject LeftOffAxisCamera;
    [SerializeField] GameObject RightOffAxisCamera;
    [SerializeField] GameObject MonoOffAxisCamera;
    [SerializeField] GameObject LeftScreen;
    [SerializeField] GameObject RightScreen;
    [SerializeField] GameObject MonoScreen;
    public bool IsOn { get; private set; }

    [Tooltip("The Main Camera of the scene, usually the camera inside the Player Prefab from SteamVR")]
    [SerializeField]
    // ReSharper disable once InconsistentNaming
    public Camera mainVrCamera = default;
    public Camera MainCamera => mainVrCamera;

    [Tooltip("A transform from which pictures can optionally be captured. Think of this as a camera in the scene (no parallax mode)")]
    [SerializeField]
    public Transform ViewPoint;

    [FormerlySerializedAs("DefaultDefaultPictureFromObject")]
    [FormerlySerializedAs("defaultPictureFromObject")]
    [FormerlySerializedAs("pictureFromObject")]
    [SerializeField]
    [Tooltip("Manually controls whether pictures are taken from HMD or from ViewPoint. Parallax mode overrides this")]
    // ReSharper disable once InconsistentNaming
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
            //ignore obsolete warning
#pragma warning disable 618
            Vector3 positionRight = InputTracking.GetLocalPosition(XRNode.RightEye);
            Vector3 positionLeft = InputTracking.GetLocalPosition(XRNode.LeftEye);
#pragma warning restore 618
            float distance = Vector3.Distance(positionRight, positionLeft);
            Debug.Log($"distance= {distance}");
            AlbertiCameraManager.StereoLeftCameraInstance.Cam.stereoSeparation = distance / 2f;
            AlbertiCameraManager.StereoRightCameraInstance.Cam.stereoSeparation = distance / 2f;
            Debug.Log($"stereoSep: {AlbertiCameraManager.ActiveCamera.Cam.stereoSeparation}");
            //return distance * InterOcularDistanceGain;
            return AlbertiCameraManager.ActiveCamera.Cam.stereoSeparation * 2f;
        }
    }

    [SerializeField]
    // ReSharper disable once InconsistentNaming
    public UpdateMode updateMode;
    public UpdateMode UpdateMode
    {
        get => updateMode;
        set => updateMode = value;
    }

    [FormerlySerializedAs("eyeViewMode")]
    [SerializeField]
    // ReSharper disable once InconsistentNaming
    public StereoMode stereoMode;
    public StereoMode StereoMode
    {
        get => stereoMode;
        set => stereoMode = value;
    }

    [SerializeField]
    // ReSharper disable once InconsistentNaming
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
    // ReSharper disable once InconsistentNaming
    public int frameLayer = default;

    [Layer]
    [SerializeField]
    [Tooltip("A layer for VrObjects to not be rendered in the picture")]
    // ReSharper disable once InconsistentNaming
    public int vrObjectLayer = default;

    [FormerlySerializedAs("monoEyeLayer")]
    [Layer]
    [SerializeField]
    [Tooltip("A layer To render only Mono Views")]
    // ReSharper disable once InconsistentNaming
    public int monoLayer = default;

    [SerializeField]
    [Layer]
    [Tooltip("A layer To render the Stereo Left view")]
    // ReSharper disable once InconsistentNaming
    public int stereoLeftLayer = default;

    [SerializeField]
    [Tooltip("A layer To render the Stereo Right view")]
    [Layer]
    // ReSharper disable once InconsistentNaming
    public int stereoRightLayer = default;

    [SerializeField]
    [Tooltip("Layers to not be rendered in pictures")]
    [Layer]
    // ReSharper disable once InconsistentNaming
    public List<int> ignoreLayers = new List<int>();

    public int MonoEyeLayer => monoLayer;
    public int StereoLeftLayer => stereoLeftLayer;
    public int StereoRightLayer => stereoRightLayer;
    public int FrameLayer => frameLayer;
    public int VrObjectLayer => vrObjectLayer;
    public List<int> IgnoreLayers => ignoreLayers;

    [SerializeField]
    [Range(0, 2)]
    // ReSharper disable once InconsistentNaming
    public float parallaxGain = 1;
    public float ParallaxGain
    {
        get => parallaxGain;
        set => parallaxGain = value;
    }

    [SerializeField]
    [Range(0, 2)]
    // ReSharper disable once InconsistentNaming
    public float interOcularDistanceGain = 1;
    public float InterOcularDistanceGain
    {
        get => interOcularDistanceGain;
        set => interOcularDistanceGain = value;
    }

    [Tooltip("Base Resolution of canvas. Increase if always pixelated (large performance hit)")]
    [Range(1, 20)]
    // ReSharper disable once InconsistentNaming
    public float resolution = 12;
    public float Resolution => resolution;

    [Tooltip("Maximum Size of canvas. Increase if pixelated when very close to canvas")]
    [Range(256, 10000)]
    // ReSharper disable once InconsistentNaming
    public int maxMonoTextureSize = 4000;
    public int MaxMonoTextureSize => maxMonoTextureSize;

    [Tooltip("Maximum Size of canvas. Increase if pixelated when very close to canvas")]
    [Range(256, 10000)]
    // ReSharper disable once InconsistentNaming
    public int maxStereoTextureSize = 7000;
    public int MaxStereoTextureSize => maxStereoTextureSize;

    // ReSharper disable once InconsistentNaming
    public bool debugMode = false;

    public bool DebugMode => debugMode;

    [FormerlySerializedAs("settings")]
    [SerializeField]
    [Tooltip("A settings file. Can be duplicated or edited")]
    // ReSharper disable once InconsistentNaming
    public SettingsMain settingsFile = default;
    public SettingsMain Settings => settingsFile;

    CameraLocation cameraLocation;
    ProjectorLocation projectorLocation;

    public PictureLocation PictureLocation { get; private set; }

    Vector3 pictureEulers;

    bool notYetTurnedOn = true;
    bool firstTimeTurnedOn = false;

    public FrameEvents FrameEvents { get; private set; }
    bool NonStandardGainSelected => Math.Abs(InterOcularDistanceGain - 1f) > 0.001f || Math.Abs(ParallaxGain - 1f) > 0.001f;

    public void OnValidate()
    {
        Assert.IsNotNull(Settings);
    }

    public void OnEnable()
    {
        if (MainCamera == null) throw new NullReferenceException("Remember to set the Main Camera field in the inspector of the Alberti Frame\n");
        if (FrameEvents == null) FrameEvents = new FrameEvents();

        ShowWarnings();

        FrameEvents.OnFoundAlbertiFrameComponent += ListenForRequiredComponents;

        ComponentFinder<AlbertiFrameComponent> componentFinder = new ComponentFinder<AlbertiFrameComponent>(this);
        componentFinder.FindAndBroadcastAllComponents();

        hmdFollow.UpdateParent(); // important for first picture to work.
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


    /// <summary>
    /// Toggles Frame On/Off
    /// (between window and picture)
    /// </summary>
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


    /// <summary>
    /// Turns frame off to hide contents
    /// (Back to being window)
    /// </summary>
    [PublicAPI]
    public void TurnOff()
    {
        Debug.Log($"{AlbertiLog.Prefix} Turning Frame Off");
        IsOn = false;
        FrameEvents.TurnedOff();
        Update();
    }

    /// <summary>
    /// Turns frame on to show contents
    /// (To being a picture)
    /// </summary>
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

    /// <summary>
    /// Takes a picture from the current location of the HMD
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    [PublicAPI]
    [ContextMenu("TakePictureFromHMD")]
    public void TakePictureFromHmd()
    {
        if (HmdTransform == null) throw new NullReferenceException("Taking picture from HmdFollowComponent location but HmdFollowComponent object null");


        if (stereoMode == StereoMode.Mono)
        {
            if (parallaxMode == ParallaxMode.On)
            {

            }
            else
            {
                MonoOffAxisCamera.GetComponent<Camera>().enabled = true;
                MonoOffAxisCamera.GetComponent<TrackedPoseDriver>().enabled = false;
                Invoke("ResetCameraMono", 0.5f);
               
            }
        }

        if (stereoMode == StereoMode.Stereo)
        {

        }

       /* RightScreen.GetComponent<ProjectionPlaneFollowFrame>().UpdateInitialPosition(transform.position); 
        LeftScreen.GetComponent<ProjectionPlaneFollowFrame>().UpdateInitialPosition(transform.position);
        MonoScreen.GetComponent<ProjectionPlaneFollowFrame>().UpdateInitialPosition(transform.position);*/

       /* RightScreen.GetComponent<ProjectionPlaneFollowFrame>().UpdateInitialRotation(new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
        LeftScreen.GetComponent<ProjectionPlaneFollowFrame>().UpdateInitialRotation(new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
        MonoScreen.GetComponent<ProjectionPlaneFollowFrame>().UpdateInitialRotation(new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));*/

       /* LeftScreen.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 + transform.eulerAngles.y, -transform.eulerAngles.z);
        RightScreen.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 + transform.eulerAngles.y, -transform.eulerAngles.z);
        MonoScreen.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 + transform.eulerAngles.y, -transform.eulerAngles.z);*/

        PictureLocation = new PictureLocation(this, HmdTransform.position);

       
        //LeftOffAxisCamera.transform.position = HmdTransform.position;
        //RightOffAxisCamera.transform.position = HmdTransform.position;
        //MonoOffAxisCamera.transform.position = HmdTransform.position;

        FinishTakingPicture();
    }

    void ResetCameraMono()
    {
        MonoOffAxisCamera.GetComponent<Camera>().enabled = false;
        MonoOffAxisCamera.GetComponent<TrackedPoseDriver>().enabled = true;
    }

    /// <summary>
    /// Takes a picture from the referenced Viewpoint transform.
    /// </summary>
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
        HmdTransform.gameObject.GetComponent<HmdFollowComponent>().UpdateParent();
        FrameEvents.PictureTaken();
        UpdateComponentsInOrder();
      
        //LeftOffAxisCamera.SetActive(true);
        //RightOffAxisCamera.SetActive(true);

        // switch parallax condition

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