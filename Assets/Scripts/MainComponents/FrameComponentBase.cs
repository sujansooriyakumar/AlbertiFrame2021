using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FrameComponentBase : MonoBehaviour, AlbertiFrameComponent
{
    protected SettingsMain Settings => Frame.Settings;

    protected FrameEvents FrameEvents => Frame.FrameEvents;

    public MagicAlbertiFrame Frame { get; private set; }

    public Transform Transform => transform;
    public void RegisterBaseObject(ComponentFinderBase finderBase)
    {
        Frame = finderBase as MagicAlbertiFrame;
        FrameRegistered();
    }

    protected virtual void FrameRegistered() { }
}