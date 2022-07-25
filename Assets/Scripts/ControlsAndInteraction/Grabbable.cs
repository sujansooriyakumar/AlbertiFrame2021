using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public GameObject Indicator;

    public int layer;

    private void OnTriggerEnter(Collider other)
    {
        Grabber grabber = other.GetComponent<Grabber>();
        if(grabber != null)
        {
            grabber.GrabEnter(this);
            ActivateIndicator();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Grabber grabber = other.GetComponent<Grabber>();
        if(grabber != null)
        {
            grabber.GrabExit(this);
            DeactivateIndicator();
        }
    }

    void ActivateIndicator()
    {
        if (Indicator == null) return;
        Indicator.SetActive(true);
    }

    void DeactivateIndicator()
    {
        if (Indicator == null) return;
        Indicator.SetActive(false);
    }
}
