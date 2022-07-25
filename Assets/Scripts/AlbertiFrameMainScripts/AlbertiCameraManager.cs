using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlbertiCameraManager
{

    static StereoMode stereoMode;

    public static AlbertiCameraComponentBase StereoLeftCameraInstance;
    public static AlbertiCameraComponentBase StereoRightCameraInstance;
    public static AlbertiCameraComponentBase ActiveCamera => StereoRightCameraInstance;
    public static bool StereoRightNotYetSet => StereoRightCameraInstance == null;
    public static bool StereoLeftNotYetSet => StereoLeftCameraInstance == null;
}
