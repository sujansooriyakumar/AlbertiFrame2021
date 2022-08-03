using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbertiInteractionSystem : MonoBehaviour
{
    [SerializeField] GameObject LeftHand;
    [SerializeField] GameObject RightHand;

    public Grabber RightGrabber;
    public Grabber LeftGrabber;

    public GameObject ControllerBindingsAttachmentPoint;
    public bool showControls;

    [Layer]
    public int SetToLayer;

    bool rightHandLayerSet = false;
    bool leftHandLayerSet = false;

    void OnEnable()
    {
        

    }

    void Update()
    {
        if (!leftHandLayerSet) SetupLeftHandLayer();
        if (!rightHandLayerSet) SetupRightHandLayer();
    }

    void SetupRightHandLayer()
    {
       
            LayerUtil.SetLayerRecursively(RightHand, SetToLayer);
            rightHandLayerSet = true;
        
    }

    void SetupLeftHandLayer()
    {
      
            LayerUtil.SetLayerRecursively(LeftHand, SetToLayer);
            leftHandLayerSet = true;
        
    }
}