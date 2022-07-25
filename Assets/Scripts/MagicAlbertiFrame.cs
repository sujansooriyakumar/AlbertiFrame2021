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
    void TakePictureFromHmd();

    [PublicAPI]
    void TakePictureFromViewPoint();

    [PublicAPI]
    void TakePictureFrom(Transform pictureLocationTransform);

    [PublicAPI]
    void TakePicture();

    [PublicAPI]
    void TurnOff();

    [PublicAPI]
    void TurnOn();
}