using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MagicAlbertiFrameComponent))]
public class ControlsMagicAlbertiFrame : MonoBehaviour
{
    MagicAlbertiFrameComponent frame;
    public Transform ResetToThisPosition;
    Vector3 initialLocation;
    Vector3 initialRotation;
    SettingsMain settings;

    ViewMode currentMode = 0;
    Vector3 startingPosition;
    Quaternion startingRotation;

    [SerializeField] GameObject LeftCanvas;
    [SerializeField] GameObject RightCanvas;
    [SerializeField] GameObject MonoCanvas;

    [SerializeField] GameObject LeftCamera;
    [SerializeField] GameObject RightCamera;
    [SerializeField] GameObject MonoCamera;
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
    public void OnToggleOnAction(InputAction.CallbackContext c)
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

    public void OnTakePictureAction(InputAction.CallbackContext c)
    {
        TakePicture();
        initialLocation = transform.position;
        initialRotation = transform.eulerAngles;
    }
    public void OnCycleModeForwardAction(InputAction.CallbackContext c)
    {
        CycleModesForward();
    }

    public void OnCycleModeBackwardAction(InputAction.CallbackContext c)
    {
        CycleModesBackward();
    }
    void TakePicture()
    {
        frame.TakePicture();
    }

    private void Update()
    {
        CheckPictorialKeyboardToggle();
        /*if (frame.IsOn)
        {
            switch (currentMode)
            {
                case ViewMode.Flat:
                    LeftCanvas.SetActive(false);
                    RightCanvas.SetActive(false);
                    MonoCanvas.SetActive(true);
                    LeftCamera.GetComponent<Camera>().enabled = false;
                    RightCamera.GetComponent<Camera>().enabled = false;
                    MonoCamera.GetComponent<Camera>().enabled = false;

                    frame.UpdateMode = UpdateMode.Still;
                    frame.StereoMode = StereoMode.Mono;
                    frame.ParallaxMode = ParallaxMode.Off;
                    break;
                case ViewMode.Stereoscopic:
                    LeftCanvas.SetActive(true);
                    RightCanvas.SetActive(true);
                    MonoCanvas.SetActive(false);
                    LeftCamera.GetComponent<Camera>().enabled = false;
                    RightCamera.GetComponent<Camera>().enabled = false;
                    MonoCamera.GetComponent<Camera>().enabled = false;
                    frame.UpdateMode = UpdateMode.Live;
                    frame.StereoMode = StereoMode.Stereo;
                    frame.ParallaxMode = ParallaxMode.Off;
                    break;
                case ViewMode.Parallax:
                    LeftCanvas.SetActive(false);
                    RightCanvas.SetActive(false);
                    MonoCanvas.SetActive(true);
                    LeftCamera.GetComponent<Camera>().enabled = false;
                    RightCamera.GetComponent<Camera>().enabled = false;
                    MonoCamera.GetComponent<Camera>().enabled = true;
                    frame.UpdateMode = UpdateMode.Live;
                    frame.StereoMode = StereoMode.Mono;
                    frame.ParallaxMode = ParallaxMode.On;
                    break;
                case ViewMode.Portal:
                    LeftCanvas.SetActive(true);
                    RightCanvas.SetActive(true);
                    MonoCanvas.SetActive(false);
                    LeftCamera.GetComponent<Camera>().enabled = true;
                    RightCamera.GetComponent<Camera>().enabled = true;
                    MonoCamera.GetComponent<Camera>().enabled = false;
                    frame.UpdateMode = UpdateMode.Live;
                    frame.StereoMode = StereoMode.Stereo;
                    frame.ParallaxMode = ParallaxMode.On;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }*/
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
                /*LeftCanvas.SetActive(false);
                RightCanvas.SetActive(false);
                MonoCanvas.SetActive(true);
                LeftCamera.GetComponent<Camera>().enabled = false;
                RightCamera.GetComponent<Camera>().enabled = false;
                MonoCamera.GetComponent<Camera>().enabled = false;*/

                frame.UpdateMode = UpdateMode.Still;
                frame.StereoMode = StereoMode.Mono;
                frame.ParallaxMode = ParallaxMode.Off;
                break;
            case ViewMode.Stereoscopic:
                /*LeftCanvas.SetActive(true);
                RightCanvas.SetActive(true);
                MonoCanvas.SetActive(false);
                LeftCamera.GetComponent<Camera>().enabled = false;
                RightCamera.GetComponent<Camera>().enabled = false;
                MonoCamera.GetComponent<Camera>().enabled = false;*/
                frame.UpdateMode = UpdateMode.Live;
                frame.StereoMode = StereoMode.Stereo;
                frame.ParallaxMode = ParallaxMode.Off;
                break;
            case ViewMode.Parallax:
               /* LeftCanvas.SetActive(false);
                RightCanvas.SetActive(false);
                MonoCanvas.SetActive(true);
                LeftCamera.GetComponent<Camera>().enabled = false;
                RightCamera.GetComponent<Camera>().enabled = false;
                MonoCamera.GetComponent<Camera>().enabled = true;*/
                frame.UpdateMode = UpdateMode.Live;
                frame.StereoMode = StereoMode.Mono;
                frame.ParallaxMode = ParallaxMode.On;
                break;
            case ViewMode.Portal:
                /*LeftCanvas.SetActive(true);
                RightCanvas.SetActive(true);
                MonoCanvas.SetActive(false);
                LeftCamera.GetComponent<Camera>().enabled = true;
                RightCamera.GetComponent<Camera>().enabled = true;
                MonoCamera.GetComponent<Camera>().enabled = false;*/
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
