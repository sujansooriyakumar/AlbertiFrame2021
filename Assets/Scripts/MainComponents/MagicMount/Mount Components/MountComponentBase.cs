using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountComponentBase : MonoBehaviour, MountFindableComponent
{
    protected FrameEvents FrameEvents => Frame.FrameEvents;

    protected MountEvents MountEvents => Mount.MountEvents;
    protected MagicAlbertiFrame Frame { get; private set; }

    protected MagicMount Mount { get; private set; }

    public void RegisterBaseObject(ComponentFinderBase finderBase)
    {
        Mount = finderBase as MagicMount;
        Frame = Mount?.Frame;
        MountRegistered();
    }

    Transform FindableComponent.transform => transform;

    protected virtual void MountRegistered() { }
}
