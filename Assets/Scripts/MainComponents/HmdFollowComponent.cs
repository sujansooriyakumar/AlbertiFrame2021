using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HmdFollowComponent : FrameComponentBase, HmdFollow
{
   public void UpdateParent()
    {
        Transform parent = AlbertiCameraManager.ActiveCamera.transform;
        transform.SetParent(parent.transform);
        transform.ResetLocal();
    }
    
}
