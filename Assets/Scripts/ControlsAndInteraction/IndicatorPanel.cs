using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class IndicatorPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshPro Text = default;
    [SerializeField] UpdatedAlbertiFrame frame;





    private void Update()
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (!frame.isOn)
        {
            Text.text = "Off (Window)";
            return;
        }

        if(frame.viewMode == UpdatedAlbertiFrame.AlbertiViewMode.FLAT)
        {
            Text.text = "Flat Picture";
            return;
        }

        if (frame.viewMode == UpdatedAlbertiFrame.AlbertiViewMode.PARALLAX)
        {
            Text.text = "Parallax Mode";
            return;
        }
        if (frame.viewMode == UpdatedAlbertiFrame.AlbertiViewMode.STEREOPSIS)
        {
            Text.text = "Stereopsis Mode";
            return;
        }

        if (frame.viewMode == UpdatedAlbertiFrame.AlbertiViewMode.PORTAL)
        {

            Text.text = "Portal Mode";
            return;
        }

        Text.text = stringBuilder.ToString();

    }
}
