using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlaneAspectRatioAndResizer : MonoBehaviour
{
    public float width;
    public float height;
    public bool LockAspect;

    [Range(0, 1)]
    public float Aspect;

    private void Update()
    {
        transform.localScale = new Vector3(width, height, 1);

        if (LockAspect) height = width * Aspect;
    }
}
