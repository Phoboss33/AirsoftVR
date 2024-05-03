using UnityEngine;
using Unity.Netcode;
using System.Diagnostics.Contracts;

public class NetworkPlayer : NetworkBehaviour
{
    
    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform body;

    private GameObject XRRig;
    public Renderer[] meshToDisable;

    // Grab
    NetworkObject leftGrabbedObject, rightGrabbedObject;

    public override void OnNetworkSpawn()
    {
        var myID = transform.GetComponent<NetworkObject>().NetworkObjectId;
        base.OnNetworkSpawn();
        if(IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.enabled = false;
            }
        }

        XRRig = VrRigPreference.Singleton.XRRig;

        if (XRRig) {
            //print("TRUEEEEE");
            XRRig.GetComponent<XRGrabEventHandler>().avatarNetworkObjectId = myID;
            XRRig.GetComponent<XRGrabEventHandler>().avatarObject = transform.gameObject;
        }
        else {
            //print("FAAAALSSS");
        }

    }

    public void Test()
    {
        print("true");
    }

    void Update()
    {
        if (IsOwner)
        {
            root.position = VrRigPreference.Singleton.root.position;
            root.rotation = VrRigPreference.Singleton.root.rotation;
            head.position = VrRigPreference.Singleton.head.position;
            head.rotation = VrRigPreference.Singleton.head.rotation;
            leftHand.position = VrRigPreference.Singleton.leftHand.position;
            leftHand.rotation = VrRigPreference.Singleton.leftHand.rotation;
            rightHand.position = VrRigPreference.Singleton.rightHand.position;
            rightHand.rotation = VrRigPreference.Singleton.rightHand.rotation;
            body.position = head.position;

            body.rotation = Quaternion.Euler(body.rotation.eulerAngles.x, head.rotation.eulerAngles.y, body.rotation.eulerAngles.z);
        

        if (leftGrabbedObject) MoveGrabbedObjectServerRpc(leftGrabbedObject, leftHand.position, leftHand.rotation);
        if (rightGrabbedObject) MoveGrabbedObjectServerRpc(rightGrabbedObject, rightHand.position, rightHand.rotation);
        }

    }

    public void AvatarSelectGrabEnterEventHub(NetworkObject netObj, bool wichHand) {
        if (wichHand)
        {
            leftGrabbedObject = netObj;
        }
        else
        {
            rightGrabbedObject = netObj;
        }

        setIsKinematicServerRpc(netObj, true);

        
    }
    public void AvatarSelectGrabExitEventHub(NetworkObject netObj, bool wichHand) {
        if (wichHand) 
            leftGrabbedObject = null;
        else
            rightGrabbedObject = null;

        setIsKinematicServerRpc(netObj, false);
    }

    [ServerRpc] public void MoveGrabbedObjectServerRpc(NetworkObjectReference grabbedObj, Vector3 position , Quaternion rotation) {
        if (!IsServer) return;

        if (grabbedObj.TryGet(out NetworkObject netObj)) {
            netObj.transform.position = position;
            netObj.transform.rotation = rotation;
            //Debug.Log("Client moved object");
        }
    }

    [ServerRpc] public void setIsKinematicServerRpc(NetworkObjectReference grabbedObj, bool value) {
        if (!IsServer) return;

        if (grabbedObj.TryGet(out NetworkObject netObj)) {
            netObj.GetComponent<Rigidbody>().isKinematic = value;
        }
    }

}
