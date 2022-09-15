using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveHMD : MonoBehaviour
{
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        Vector3 leftEyePos = leftEye.position + new Vector3(.65f / 2, 0, 0);
        Vector3 rightEyePos = rightEye.position - new Vector3(.65f / 2, 0, 0);

        Vector3 pos = (leftEyePos + rightEyePos) / 2;
        transform.position = pos;
    }
}
