using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameHighlight : MonoBehaviour
{
    MagicAlbertiFrame frame;

    private void Start()
    {
        frame = GetComponentInParent<MagicAlbertiFrame>();
        if (frame == null)
        {
            Debug.LogError($"{AlbertiLog.Prefix} FrameHighlight could not find its associated frame.");
            return;
        }
        gameObject.layer = frame.FrameLayer;
    }
}
