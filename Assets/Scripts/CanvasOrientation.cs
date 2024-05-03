using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOrientation : MonoBehaviour
{
    public Transform cameraTransform;

    void Update()
    {
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
            cameraTransform.rotation * Vector3.up);
    }
}
