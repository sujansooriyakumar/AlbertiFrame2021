using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProjectionCameraRotation : MonoBehaviour
{
    [SerializeField] Transform parent;

    private void Awake()
    {
    }
    private void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 180f - parent.localRotation.eulerAngles.y, 0));
    }
}
