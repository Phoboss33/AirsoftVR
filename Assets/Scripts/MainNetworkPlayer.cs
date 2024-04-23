using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainNetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    public Transform lefthand;
    public Transform righthand;

    public Renderer[] meshToDisable;
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsOwner) {
            foreach (var item in meshToDisable) {
                item.enabled = false;
            }
        }
    }

    void Update() {
        if (IsOwner) {
            root.position = VRRigReferences.Singleton.root.position;
            root.rotation = VRRigReferences.Singleton.root.rotation;

            head.position = VRRigReferences.Singleton.head.position;
            head.rotation = VRRigReferences.Singleton.head.rotation;

            lefthand.position = VRRigReferences.Singleton.leftHand.position;
            lefthand.rotation = VRRigReferences.Singleton.leftHand.rotation;

            righthand.position = VRRigReferences.Singleton.rightHand.position;
            righthand.rotation = VRRigReferences.Singleton.rightHand.rotation;
        }

    }
}
