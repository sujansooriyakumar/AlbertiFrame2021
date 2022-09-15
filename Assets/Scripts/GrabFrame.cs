using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabFrame : MonoBehaviour
{
    // Start is called before the first frame update
    bool grabbed;
    bool selected;
    Transform trackedHand;
    Vector3 startingEulers;
    Vector3 startingPosition;
    Vector3 initRotationOfFrame;
    Vector3 initPositionOfFrame;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed && trackedHand != null)
        {
            transform.position = trackedHand.position;
            Vector3 rot = trackedHand.eulerAngles - startingEulers;
            transform.eulerAngles = initRotationOfFrame + rot;
            Vector3 pos = trackedHand.position - startingPosition;
            transform.position = initPositionOfFrame + pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        selected = true;
        trackedHand = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        selected = false;
       // trackedHand = null;
    }

    public void Grab()
    {
        if (trackedHand != null)
        {
            initRotationOfFrame = transform.eulerAngles;
            startingEulers = trackedHand.eulerAngles;
            initPositionOfFrame = transform.position;
            startingPosition = trackedHand.position;
            grabbed = true;
        }
    }

    public void Release()
    {
        grabbed = false;
        trackedHand = null;
    }
}
