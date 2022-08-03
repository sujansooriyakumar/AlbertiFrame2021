using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountEvents
{
    public delegate void UpdateCameraEvent(CameraLocation cameraLocation);

    public event UpdateCameraEvent OnUpdateCamera;

    public void UpdateCamera(CameraLocation cameraLocation)
    {
        OnUpdateCamera?.Invoke(cameraLocation);
    }

    public delegate void UpdateProjectorEvent(ProjectorLocation projectorLocation);

    public event UpdateProjectorEvent OnUpdateProjector;

    public void UpdateProjector(ProjectorLocation projectorLocation)
    {
        OnUpdateProjector?.Invoke(projectorLocation);

    }

    public delegate void MountActivatedEvent();
    public event MountActivatedEvent OnMountActivated;

    public void MountActivated()
    {
        OnMountActivated?.Invoke();
    }

    public delegate void MountDeactivatedEvent();
    public event MountDeactivatedEvent OnMountDeactivated;

    public void MountDeactivated()
    {
        OnMountDeactivated?.Invoke();
    }

    public delegate void FoundMountComponentEvent(MountFindableComponent albertiMountFindableComponent);

    public event FoundMountComponentEvent OnFoundMountComponent;

    public void FoundMountComponent(MountFindableComponent magicMountFindableComponent)
    {
        OnFoundMountComponent?.Invoke(magicMountFindableComponent);
    }
}
