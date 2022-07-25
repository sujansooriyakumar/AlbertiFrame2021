using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CameraAspect : MonoBehaviour
{
    public float Aspect = 1;
    private Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        thisCamera.aspect = Aspect;
    }
}
