using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlaneFollowFrame : MonoBehaviour
{
    
    [SerializeField] Transform AlbertiFrameTransform;
    [SerializeField] Transform TrackedHead;
    Vector3 initialPosition;
    Vector3 initialRotation;
    bool updateRotation;
    bool updatePosition;

    // Update is called once per frame
    MagicAlbertiFrameComponent frame;
    private void Awake()
    {
        frame = AlbertiFrameTransform.gameObject.GetComponent<MagicAlbertiFrameComponent>();
        initialPosition = Vector3.zero;
        initialRotation = new Vector3(0, 180f, 0);
    }
    void Update()
    {
        if (frame.parallaxMode == ParallaxMode.On)
        {
            //transform.position = AlbertiFrameTransform.position;
            transform.eulerAngles = new Vector3(-AlbertiFrameTransform.eulerAngles.x, 180+AlbertiFrameTransform.eulerAngles.y, -AlbertiFrameTransform.eulerAngles.z);

        }
        else if (frame.parallaxMode == ParallaxMode.Off && updateRotation && updatePosition)
        {
            transform.position = initialPosition;
            transform.eulerAngles = initialRotation;
        }
    }

    public void UpdateInitialPosition(Vector3 pos)
    {
        initialPosition = pos;
        updatePosition = true;
    }

    public void UpdateInitialRotation(Vector3 eulers)
    {
        initialRotation = new Vector3(eulers.x, 180 + eulers.y, eulers.z);
        updateRotation = true;
    }
}
