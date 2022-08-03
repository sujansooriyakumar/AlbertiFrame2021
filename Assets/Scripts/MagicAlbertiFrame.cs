using System.Collections.Generic;

using JetBrains.Annotations;
using UnityEngine;

public interface MagicAlbertiFrame : ComponentFinderBase
{
    SettingsMain Settings { get; }

    int FrameLayer { get; }
    int VrObjectLayer { get; }
    int MonoEyeLayer { get; }
    int StereoLeftLayer { get; }
    int StereoRightLayer { get; }

    FrameEvents FrameEvents { get; }
    UpdateMode UpdateMode { get; set; }
    ParallaxMode ParallaxMode { get; set; }

    float InterOcularDistance { get; }

    Camera MainCamera { get; }
    StereoMode StereoMode { get; set; }
    bool IsOn { get; }

    List<int> IgnoreLayers { get; }
    PictureLocation PictureLocation { get; }

    float Resolution { get; }
    int MaxMonoTextureSize { get; }
    int MaxStereoTextureSize { get; }
    bool DebugMode { get; }

    [PublicAPI]
    float InterOcularDistanceGain { get; set; }

    [PublicAPI]
    float ParallaxGain { get; set; }


    [PublicAPI]
    Transform ActiveCamera { get; }

    /// <summary>
    /// Takes a picture from the current location of the HMD
    /// </summary>
    [PublicAPI]
    void TakePictureFromHmd();

    /// <summary>
    /// Takes a picture from the referenced Viewpoint transform.
    /// </summary>
    [PublicAPI]
    void TakePictureFromViewPoint();

    /// <summary>
    /// Takes a Picture from specified Transform
    /// </summary>
    /// <param name="pictureLocationTransform"></param>
    [PublicAPI]
    void TakePictureFrom(Transform pictureLocationTransform);

    /// <summary>
    /// Takes Picture depending on state of DefaultPictureFrom variable.
    /// </summary>
    [PublicAPI]
    void TakePicture();

    /// <summary>
    /// Turns frame off to hide contents
    /// (Back to being window)
    /// </summary>
    [PublicAPI]
    void TurnOff();

    /// <summary>
    /// Turns frame on to show contents
    /// (To being a picture)
    /// </summary>
    [PublicAPI]
    void TurnOn();
}