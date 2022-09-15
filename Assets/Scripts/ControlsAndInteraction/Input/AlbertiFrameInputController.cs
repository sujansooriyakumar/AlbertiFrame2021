using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class AlbertiFrameInputController : MonoBehaviour
{
    [SerializeField] InputAction TurnOnFrame;
    [SerializeField] InputAction NextMode;
    [SerializeField] InputAction PreviousMode;
    [SerializeField] InputAction TakePicture;
    [SerializeField] UpdatedAlbertiFrame AlbertiFrame;

    private void Awake()
    {
        TurnOnFrame.performed += ToggleFrame;
        NextMode.performed += CycleNextMode;
        PreviousMode.performed += CyclePreviousMode;
        TakePicture.performed += StartTakePicture;
    }

    private void OnEnable()
    {
        TurnOnFrame.Enable();
        NextMode.Enable();
        PreviousMode.Enable();
        TakePicture.Enable();
    }

    public void ToggleFrame(InputAction.CallbackContext c)
    {
        AlbertiFrame.ToggleFrame();
    }

    public void CycleNextMode(InputAction.CallbackContext c)
    {
        AlbertiFrame.CycleModeForward();
    }

    public void CyclePreviousMode(InputAction.CallbackContext c)
    {
        AlbertiFrame.CycleModeBackward();
    }

    public void StartTakePicture(InputAction.CallbackContext c)
    {
        AlbertiFrame.TakePicture();
    }
}
