using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProjectionPlanePosition : MonoBehaviour
{
    [SerializeField] UpdatedAlbertiFrame Frame;
    [SerializeField] Transform parent;
    public Vector3 offset;
    bool pictureTaken;
    private void Update()
    {

    }

    public void PictureTaken()
    {
        transform.parent = null;
        transform.eulerAngles = new Vector3(-parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z);

    }

    public void SetPictureTaken(bool b)
    {
        pictureTaken = b;
    }

    public void SetOffset(Vector3 offset_)
    {
        offset = -offset_;
    }

    public void Reparent()
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        Invoke("PictureTaken", 0.5f);
    }
}
