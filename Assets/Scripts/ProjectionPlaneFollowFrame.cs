using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlaneFollowFrame : MonoBehaviour
{
    [SerializeField] Transform AlbertiFrameTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = AlbertiFrameTransform.position;
        transform.eulerAngles = new Vector3(0, 180 - (-360) - AlbertiFrameTransform.eulerAngles.y, 0);
        
    }
}
