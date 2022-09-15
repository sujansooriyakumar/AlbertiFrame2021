using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRig : MonoBehaviour
{
    [SerializeField] Transform parent;
    bool isTracking = true;
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(-parent.eulerAngles.x, 180 + parent.eulerAngles.y, parent.eulerAngles.z);
    }

   
}
