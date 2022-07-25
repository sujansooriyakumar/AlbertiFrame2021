using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MagicAlbertiFrameComponent))]
public class ControlsMagicAlbertiFrame : MonoBehaviour
{
    MagicAlbertiFrameComponent frame;
    public Transform ResetToThisPosition;
    SettingsMain settings;

    ViewMode currentMode = 0;
    Vector3 startingPosition;
    Quaternion startingRotation;
    enum ViewMode
    {
        Flat,
        Stereoscopic,
        Parallax,
        Portal
    }
    private void OnEnable()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        frame = GetComponent<MagicAlbertiFrameComponent>();
        if (frame == null) throw new NullReferenceException("Frame controls attached to object without an AlbertiFrame");

        settings = frame.Settings;
        if(settings == null)
        {
            Debug.LogWarning($"{AlbertiLog.Prefix} Frame Controls lacks reference to settings.");
        }

        // TODO: here is where we need to activate the settings

    }

    // this function should be called by the input system
    void OnToggleOnAction()
    {
        ToggleFrameOn();
    }

    void ToggleFrameOn()
    {
        frame.ToggleOnOff();
    }

    private void OnDisable()
    {
       // TODO: disable settings here
    }

    void OnTakePictureAction()
    {
        TakePicture();
    }

    void TakePicture()
    {
        frame.TakePicture();
    }

    private void Update()
    {
        CheckPictorialKeyboardToggle();
    }

    void CycleModesForward()
    {
        currentMode = GetNextMode();
        UpdateAlbertiViewMode();
    }

    void CycleModesBackward()
    {
        currentMode = GetPreviousMode();
        UpdateAlbertiViewMode();
    }

    void UpdateAlbertiViewMode()
    {
        switch (currentMode)
        {
            case ViewMode.Flat:
                frame.UpdateMode = UpdateMode.Live;
                frame.StereoMode = StereoMode.Mono;
                frame.ParallaxMode = ParallaxMode.Off;
                break;
            case ViewMode.Stereoscopic:
                frame.UpdateMode = UpdateMode.Live;
                frame.StereoMode = StereoMode.Stereo;
                frame.ParallaxMode = ParallaxMode.Off;
                break;
            case ViewMode.Parallax:
                frame.UpdateMode = UpdateMode.Live;
                frame.StereoMode = StereoMode.Mono;
                frame.ParallaxMode = ParallaxMode.On;
                break;
            case ViewMode.Portal:
                frame.UpdateMode = UpdateMode.Live;
                frame.StereoMode = StereoMode.Stereo;
                frame.ParallaxMode = ParallaxMode.On;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    ViewMode GetNextMode()
    {
        int modeIndex = (int)currentMode;
        modeIndex++;
        if (modeIndex == 4) modeIndex = 0;
        return (ViewMode)modeIndex;
    }

    ViewMode GetPreviousMode()
    {
        int modeIndex = (int)currentMode;
        modeIndex--;
        if (modeIndex == -1) modeIndex = 3;
        return (ViewMode)modeIndex;
    }

    void ResetFramePosition()
    {
        if(ResetToThisPosition != null)
        {
            frame.transform.CopyWorldFrom(ResetToThisPosition);
        }
        else
        {
            frame.transform.position = startingPosition;
            frame.transform.rotation = startingRotation;
        }
    }

    private void CheckPictorialKeyboardToggle()
    {
        // TODO: keyboard bind to take picture
    }


}
