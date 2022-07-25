using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this isnt static so we can have multiple frames per scene
public class FrameEvents
{
    public delegate void TurnOnEvent();

    public event TurnOnEvent OnTurnOn;

    public void TurnedOn()
    {
        OnTurnOn?.Invoke();
    }

    public delegate void TurnOffEvent();

    public event TurnOffEvent OnTurnOff;

    public void TurnedOff()
    {
        OnTurnOff?.Invoke();
    }

    public delegate void PictureTakenEvent();

    public event PictureTakenEvent OnPictureTaken;

    public void PictureTaken()
    {
        OnPictureTaken?.Invoke();
    }

    public delegate void PictureClearedEvent();

    public event PictureClearedEvent OnPictureCleared;

    public void PictureCleared()
    {
        OnPictureCleared?.Invoke();
    }

    public delegate void UpdateMainAnchorLocationEvent();

    public event UpdateMainAnchorLocationEvent OnUpdateMainAnchorLocation;

    public void UpdateMainAnchorLocation()
    {
        OnUpdateMainAnchorLocation?.Invoke();
    }

    public delegate void UpdateAnchorsEvent();

    public event UpdateAnchorsEvent OnUpdateAnchors;

    public void UpdateAnchors()
    {
        OnUpdateAnchors?.Invoke();
    }

    public delegate void UpdateMainCameraLocationEvent();

    public event UpdateMainAnchorLocationEvent OnUpdateMainCameraLocation;

    public void UpdateMainCameraLocation()
    {
        OnUpdateMainCameraLocation?.Invoke();
    }

    public delegate void UpdateMountsEvent();

    public event UpdateMountsEvent OnUpdateMounts;

    public void UpdateMounts()
    {
        OnUpdateMounts?.Invoke();
    }

    public delegate void UpdateMainProjectorLocationEvent();

    public event UpdateMainProjectorLocationEvent OnUpdateMainProjectorLocation;

    public void UpdateMainProjectorLocation()
    {
        OnUpdateMainProjectorLocation?.Invoke();
    }

    public delegate void UpdateProjectorsEvent();

    public event UpdateProjectorsEvent OnUpdateProjectors;

    public void UpdateProjectors()
    {
        OnUpdateProjectors?.Invoke();

    }

    public delegate void FoundAlbertiFrameComponentEvent(AlbertiFrameComponent frameComponent);

    public event FoundAlbertiFrameComponentEvent OnFoundAlbertiFrameComponent;

    public void FoundAlbertiFrameComponent(AlbertiFrameComponent findableComponent)
    {
        OnFoundAlbertiFrameComponent?.Invoke(findableComponent);
    }
}
