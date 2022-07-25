using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ComponentFinderBase
{
    Transform transform { get; }

    void RaiseFoundComponentEvent(FindableComponent findableComponent);
}