using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class IndicatorPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshPro Text = default;

    MagicAlbertiFrame frame;

    private void Start()
    {
        frame = GetComponentInParent<MagicAlbertiFrame>();

        if (Text == null || frame == null)
        {
            Debug.LogWarning($"{AlbertiLog.Prefix} Indicator panel has setup issue, so turning itself off.");
            gameObject.SetActive(false);
            return;
        }


        LayerUtil.SetLayerRecursively(gameObject, frame.FrameLayer);
    }

    private void Update()
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (!frame.IsOn)
        {
            Text.text = "Off (Window)";
            return;
        }

        if(frame.StereoMode == StereoMode.Stereo &&
            frame.ParallaxMode == ParallaxMode.On &&
            frame.UpdateMode == UpdateMode.Live)
        {
            Text.text = "Both Stereo and Parallax (Portal)";
            return;
        }

        if (frame.StereoMode == StereoMode.Stereo) stringBuilder.Append("Stereoscopic Only");
        if (frame.ParallaxMode == ParallaxMode.On) stringBuilder.Append("Parallax Only");

        if (frame.StereoMode == StereoMode.Mono &&
                frame.ParallaxMode == ParallaxMode.Off)
        {

            stringBuilder.Append("Flat Picture");
        }

        Text.text = stringBuilder.ToString();

    }
}
