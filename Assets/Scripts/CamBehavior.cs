using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehavior : MonoBehaviour
{
    public GameObject[] cameras;

    private void Start()
    {
        TurnOffAllCameras();
    }

    public void TurnOffAllCameras()
    {
        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
    }
}
