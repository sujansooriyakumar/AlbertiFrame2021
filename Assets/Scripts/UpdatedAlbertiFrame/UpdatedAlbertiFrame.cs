using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedAlbertiFrame : MonoBehaviour
{
    public enum AlbertiViewMode
    {
        FLAT,
        STEREOPSIS,
        PARALLAX,
        PORTAL
    }

    public enum ParallaxCondition
    {
        ON,
        OFF
    }

    public enum StereopsisCondition
    {
        MONO,
        STEREO
    }
    public static UpdatedAlbertiFrame instance;

    [SerializeField] public AlbertiViewMode viewMode;
    [SerializeField] public ParallaxCondition parallaxMode;
    [SerializeField] StereopsisCondition stereoMode;
    public bool isOn;

    [SerializeField] GameObject[] cameras;
    [SerializeField] GameObject[] canvases;
    [SerializeField] GameObject[] rigs;
    [SerializeField] Transform hmd;
    [SerializeField] Transform leftEyeHmd;
    [SerializeField] Transform rightEyeHmd;
    Vector3 positionAtPictureTaken;
    Vector3 leftEyePositionAtPictureTaken;
    Vector3 rightEyePositionAtPictureTaken;
    public float parallaxGain;
    private void Awake()
    {
        viewMode = AlbertiViewMode.FLAT;
        parallaxMode = ParallaxCondition.OFF;
        stereoMode = StereopsisCondition.MONO;
        isOn = false;
        instance = this;
    }

    private void Update()
    {
        if (isOn)
        {
            switch (viewMode)
            {
                case AlbertiViewMode.FLAT:
                    canvases[0].SetActive(true);
                    canvases[1].SetActive(false);
                    canvases[2].SetActive(false);
                    break;
                case AlbertiViewMode.STEREOPSIS:
                    canvases[0].SetActive(false);
                    canvases[1].SetActive(true);
                    canvases[2].SetActive(true);
                    break;
                case AlbertiViewMode.PARALLAX:
                    canvases[0].SetActive(true);
                    canvases[1].SetActive(false);
                    canvases[2].SetActive(false);
                    break;
                case AlbertiViewMode.PORTAL:
                    canvases[0].SetActive(false);
                    canvases[1].SetActive(true);
                    canvases[2].SetActive(true);
                    break;
            }
        }
  
        
      
    }

    public void TakePicture()
    {
        positionAtPictureTaken = transform.position;
        leftEyePositionAtPictureTaken = leftEyeHmd.position;
        rightEyePositionAtPictureTaken = rightEyeHmd.position;
        foreach(GameObject r in rigs)
        {
            r.transform.position = positionAtPictureTaken;
            r.transform.rotation = transform.parent.transform.rotation;
        }

        switch (viewMode)
        {
            case AlbertiViewMode.FLAT:
                cameras[0].GetComponent<DriveOffAxisCamera>().SetIsTracking(false);
                Vector3 pos = rigs[0].transform.InverseTransformPoint(hmd.position);
                cameras[0].transform.localPosition = pos;
                parallaxMode = ParallaxCondition.OFF;
                stereoMode = StereopsisCondition.MONO;
                break;
            case AlbertiViewMode.STEREOPSIS:
                cameras[1].GetComponent<DriveOffAxisCamera>().SetIsTracking(false);
                cameras[2].GetComponent<DriveOffAxisCamera>().SetIsTracking(false);
                parallaxMode = ParallaxCondition.OFF;
                stereoMode = StereopsisCondition.STEREO;
                Vector3 posLeft = rigs[1].transform.InverseTransformPoint(leftEyeHmd.position);
                Vector3 posRight = rigs[2].transform.InverseTransformPoint(rightEyeHmd.position);
                cameras[1].transform.localPosition = posLeft;
                cameras[2].transform.localPosition = posRight;            
                break;
            case AlbertiViewMode.PARALLAX:
                cameras[0].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                parallaxMode = ParallaxCondition.ON;
                stereoMode = StereopsisCondition.MONO;
                cameras[0].GetComponent<Camera>().enabled = true;
                break;
            case AlbertiViewMode.PORTAL:
                cameras[1].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                cameras[2].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                parallaxMode = ParallaxCondition.ON;
                stereoMode = StereopsisCondition.STEREO;
                break;
        }      
    }

    public void CycleModeForward()
    {
        if((int)viewMode == 3)
        {
            viewMode = 0; 
        }
        else
        {
            viewMode++;
        }
        UpdateAccordingToViewMode();
    }

    public void CycleModeBackward()
    {
        if ((int)viewMode == 0)
        {
            viewMode = (AlbertiViewMode)3;
        }
        else
        {
            viewMode--;
        }
        UpdateAccordingToViewMode();
    }

    void UpdateAccordingToViewMode()
    {
        switch (viewMode)
        {
            case AlbertiViewMode.FLAT:
                cameras[0].GetComponent<DriveOffAxisCamera>().SetIsTracking(false);
                Vector3 pos = transform.parent.InverseTransformPoint(hmd.position);
                cameras[0].transform.localPosition = pos;
                parallaxMode = ParallaxCondition.OFF;
                stereoMode = StereopsisCondition.MONO;
                break;
            case AlbertiViewMode.STEREOPSIS:
                cameras[1].GetComponent<DriveOffAxisCamera>().SetIsTracking(false);
                cameras[2].GetComponent<DriveOffAxisCamera>().SetIsTracking(false);
                parallaxMode = ParallaxCondition.OFF;
                stereoMode = StereopsisCondition.STEREO;
                break;
            case AlbertiViewMode.PARALLAX:
                cameras[0].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                cameras[1].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                cameras[2].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                parallaxMode = ParallaxCondition.ON;
                stereoMode = StereopsisCondition.STEREO;
                break;
            case AlbertiViewMode.PORTAL:
                cameras[0].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                cameras[1].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                cameras[2].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
                parallaxMode = ParallaxCondition.ON;
                stereoMode = StereopsisCondition.STEREO;
                break;
        }
    }

    public Vector3 GetPositionWhenThePictureWasTaken()
    {
        return positionAtPictureTaken;
    }

    public void TurnOnFrame()
    {
        isOn = true;
        switch (viewMode)
        {
            case AlbertiViewMode.FLAT:
                canvases[0].SetActive(true);
                break;
            case AlbertiViewMode.PARALLAX:
                canvases[0].SetActive(true);
                break;
            case AlbertiViewMode.STEREOPSIS:
                canvases[1].SetActive(true);
                canvases[2].SetActive(true);
                break;
            case AlbertiViewMode.PORTAL:
                canvases[1].SetActive(true);
                canvases[2].SetActive(true);
                break;
        }
        cameras[0].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
        cameras[1].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
        cameras[2].GetComponent<DriveOffAxisCamera>().SetIsTracking(true);
        TakePicture();
    }


    public void ToggleFrame()
    {
        if (isOn)
        {
            TurnOffFrame();
        }
        else
        {
            TurnOnFrame();
        }
    }

    void TurnOffFrame()
    {
        isOn = false;
        foreach(GameObject g in canvases)
        {
            g.SetActive(false);
        }
    }

    public Vector3 GetPositionAtPictureTaken()
    {
        return positionAtPictureTaken;
    }
}
