using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Grabber : MonoBehaviour
{
    bool grabbing;
    Grabbable grabbedObject;
    Grabbable hoveringObject;
    GameObject placeHolder;
    Hand Hand;
    InputAction grabAction;
    float size;

    public void Init(Hand connectedHand, InputAction connectedGrabAction)
    {
        Hand = connectedHand;
        if (connectedGrabAction == null) gameObject.SetActive(false);

        grabAction = connectedGrabAction;
        // TODO: subscribe to events
    }

    public void GrabEnter(Grabbable grabbable)
    {
        if(hoveringObject == null)
        {
            hoveringObject = grabbable;
        }
    }

    public void GrabExit(Grabbable grabbable)
    {
        if(ReferenceEquals(hoveringObject, grabbable))
        {
            hoveringObject = null;
        }
    }

    // called when the grab button is pressed
    void StartGrabbed()
    {
        if(hoveringObject == null)
        {
            return;
        }
        grabbedObject = hoveringObject;
        placeHolder = new GameObject();
        placeHolder.name = "grab placeholder";
        placeHolder.transform.CopyWorldFrom(grabbedObject.transform);
        placeHolder.transform.SetParent(this.transform);

        grabbing = true;
        Debug.Log($"{AlbertiLog.Prefix} Grab detected: {grabbedObject.name}");

    }

    // call when the grab button is released
    void StopGrabbed()
    {
        grabbedObject = null;
        Destroy(placeHolder);
        grabbing = false;
    }

    private void Update()
    {
        if (Hand == null) return;
        // TODO: the grabber should follow the transform of the hand
    }

    private void LateUpdate()
    {
        if (grabbing)
        {
            grabbedObject.transform.CopyWorldFrom(placeHolder.transform);
        }
    }
}
