using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveOffAxisCamera : MonoBehaviour
{
    [SerializeField] Transform hmd;
    [SerializeField] Transform frame;
    [SerializeField] float offset;
    Transform parent;
    bool isTracking = true;
    private void Start()
    {
        parent = transform.parent;
    }

    private void Update()
    {
         Vector3 pos = frame.InverseTransformPoint(hmd.position);
         if(isTracking) transform.localPosition = new Vector3(pos.x, pos.y, pos.z+offset);
    }

    public void SetIsTracking(bool val)
    {
        isTracking = val;
    }

    public void SetHmdfollow(Transform t)
    {
        hmd = t;
    }
}
