using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFinder<T> where T: FindableComponent
{
    readonly ComponentFinderBase finderBase;

    public ComponentFinder(ComponentFinderBase finderBase)
    {
        this.finderBase = finderBase;
    }

    public void FindAndBroadcastAllComponents()
    {
        T[] foundComponents = FindComponents();
        RegisterComponents(foundComponents);
        BroadCastComponents(foundComponents);
    }

    T[] FindComponents()
    {
        T[] foundComponents = finderBase.transform.GetComponentsInChildren<T>();
        return foundComponents;
    }

    void RegisterComponents(T[] foundComponents)
    {
        foreach(T component in foundComponents)
        {
            component.RegisterBaseObject(finderBase);
        }
    }

    void BroadCastComponents(T[] foundComponents)
    {
        foreach(T foundComponent in foundComponents)
        {
            finderBase.RaiseFoundComponentEvent(foundComponent);
        }
    }

}