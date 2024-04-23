using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    void Start()
    {
        Transform XRO = transform;
        Transform CamO = GameObject.Find("Camera Offset").transform;
        Transform MainC = GameObject.Find("Main Camera").transform;

        Vector3 initPos = Vector3.zero;
        Quaternion initRot = Quaternion.Euler(0, 0, 0);

        XRO.position = initPos;
        XRO.rotation = initRot;
        CamO.position = initPos;
        CamO.rotation = initRot;
        MainC.position = initPos;
        MainC.rotation = initRot;

        Debug.Log("Positon Xr origin:" + XRO.position + ", Cam offset: " + CamO.position + ", Main Camera: " + MainC.position);
        Debug.Log("Positon Xr rotation:" + XRO.rotation + ", Cam offset: " + CamO.rotation + ", Main Camera: " + MainC.rotation);
    }


}
