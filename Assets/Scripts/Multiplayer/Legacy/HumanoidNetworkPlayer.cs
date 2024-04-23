using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class HumanoidNetworkPlayer : NetworkBehaviour {
    GameObject xrRig;
    Transform xrLH, xrRH, xrLC, xrRC, xrCam;
    Transform avHead, avLeft, avRight, avBody;

    float avtScale, avtHeight, avtToeYPos, avtEyeYPos;
    bool autoTune = false;

    Transform myCameraOffset;

    [SerializeField] Vector3 avatarLeftPositionOffset, avatarRightPositionOffset;
    [SerializeField] Quaternion avatarLeftRotationOffset, avatarRightRotationOffset;
    [SerializeField] Vector3 avatarHeadPositionOffset;
    [SerializeField] Quaternion avatarHeadRotationOffset;

    [SerializeField] Vector3 thirdOffset, firstOffset;

    public override void OnNetworkSpawn() {
        var myID = transform.GetComponent<NetworkObject>().NetworkObjectId;
        if (IsOwnedByServer) {
            transform.name = "Host" + myID;
        }
        else {
            transform.name = "Client: " + myID;
        }

        if (!IsOwner) return;

        xrRig = GameObject.Find("XR Origin (XR Rig)");
        if (xrRig) Debug.Log("Xr found");
        else Debug.Log("Xr not found");

        //HaCM = xrRig.GetComponent<XRInputModalityManager>();

        //xrLH = HaCM.leftHand.transform.Find("Direct Interactor");
        //xrRH = HaCM.rightHand.transform.Find("Direct Interactor");

        xrLC = GameObject.Find("Left Controller").transform;
        xrRC = GameObject.Find("Right Controller").transform;

        xrCam = GameObject.Find("Main Camera").transform;
        
        myCameraOffset = GameObject.Find("Camera Offset").transform;

        avLeft = transform.Find("XR IK").Find("Left Arm IK").Find("Left Arm IK_target");
        avRight = transform.Find("XR IK").Find("Right Arm IK").Find("Right Arm IK_target");
        avHead = transform.Find("XR IK").Find("Head IK").Find("Head IK_target");
        avBody = transform;


    }

    void Update() {
        if (!IsOwner) return;
        if (!xrRig) return;

        Vector3 cameraOffset = myCameraOffset.rotation * firstOffset;

        if (avLeft) {
            //avLeft.position = xrLC.position + avatarLeftPositionOffset;
            avLeft.localPosition = Quaternion.Inverse(myCameraOffset.rotation) * (xrLC.position - myCameraOffset.position);
            avLeft.rotation = xrLC.rotation * avatarLeftRotationOffset;
        }
        else {
            print("нет левой");
        }
        if (avRight) {
            //avRight.position = xrRC.position + avatarRightPositionOffset;
            avRight.localPosition = Quaternion.Inverse(myCameraOffset.rotation) * (xrRC.position - myCameraOffset.position);
            avRight.rotation = xrRC.rotation * avatarRightRotationOffset;

        }
        else
            print("нет правой");

        if (avHead) {
            //avHead.position = xrCam.position + avatarHeadPositionOffset;

            avHead.rotation = xrCam.rotation * avatarHeadRotationOffset;

        }
        else
            print("нет головы");

        if (avBody) {
            avBody.position = avHead.position + cameraOffset;

            // —охран€ем текущее вращение по ос€м X и Z
            float originalX = avBody.rotation.eulerAngles.x;
            float originalZ = avBody.rotation.eulerAngles.z;

            // ѕримен€ем вращение объекта avHead только по оси Y
            float yRotation = avHead.rotation.eulerAngles.y;

            // ”станавливаем вращение с сохранением оригинальных значений по ос€м X и Z
            avBody.rotation = Quaternion.Euler(originalX, yRotation, originalZ);
        }
        else
            print("нет тела");


    }
}
