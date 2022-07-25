using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FindableComponent
{
    void RegisterBaseObject(ComponentFinderBase finderBase);

    Transform transform { get; }
}