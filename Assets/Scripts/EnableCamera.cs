using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().enabled = true;
    }

   
}