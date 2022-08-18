using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRightEyeOnly : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;
    [SerializeField] bool right = true;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material.SetInt("_rightEye", right ? 1 : 0);
    }
}
