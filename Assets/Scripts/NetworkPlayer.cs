using UnityEngine;
using Unity.Netcode;
using System.Diagnostics.Contracts;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UIElements;
using System.Drawing;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour
{

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform body;

    public EventHandler eventHandler;

    public GameObject bulletPref;
    private GameObject XRRig;
    public Renderer[] meshToDisable;
    private GameObject currentPistol;
    [SerializeField] GameObject pistolPrefab;

    private bool hasPistol = false; //Проверка был ли взят пистолет
    // Grab
    NetworkObject leftGrabbedObject, rightGrabbedObject;
    //S[SerializeField] private XRGrabInteractable grabbableLeft, grabbable;
    private XRGrabInteractable grabInteractable;
    public override void OnNetworkSpawn()
    {
        var myID = transform.GetComponent<NetworkObject>().NetworkObjectId;
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.enabled = false;
            }
            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.activated.AddListener(OnActivated);
            }

        }

        XRRig = VrRigPreference.Singleton.XRRig;

        if (XRRig) {
            print("TRUEEEEE");
            XRRig.GetComponent<XRGrabEventHandler>().avatarNetworkObjectId = myID;
            XRRig.GetComponent<XRGrabEventHandler>().avatarObject = transform.gameObject;
        }
        else {
            print("FAAAALSSS");
        }

    }
    private void OnActivated(ActivateEventArgs arg)
    {
        TryShoot();
    }
    private void TryShoot()
    {
        if (canShoot)
        {
            canShoot = false;
            RequestShootServerRpc(spawnPoint.position, spawnPoint.rotation);

        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestShootServerRpc(Vector3 position, Quaternion rotation)
    {
        SpawnBulletServerRPC(position, rotation);
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
           // ShootActive(currentObj);

            body.rotation = Quaternion.Euler(body.rotation.eulerAngles.x, head.rotation.eulerAngles.y, body.rotation.eulerAngles.z);


            if (leftGrabbedObject) MoveGrabbedObjectServerRpc(leftGrabbedObject, leftHand.position, leftHand.rotation);
            if (rightGrabbedObject) MoveGrabbedObjectServerRpc(rightGrabbedObject, rightHand.position, rightHand.rotation);
        }

    }
    private void Start()
    {

    }

    public NetworkObject currentObj;
    public Transform spawnPoint;    
    public void AvatarSelectGrabEnterEventHub(NetworkObject netObj, bool wichHand) {
        if (wichHand) {
            leftGrabbedObject = netObj;
        }
        else
        {
            rightGrabbedObject = netObj;
        }


        setIsKinematicServerRpc(netObj, true);

        currentObj = netObj;
        spawnPoint = currentObj.transform.Find("BulletSpawnPoint");
        if (spawnPoint != null)
        {
            print("Vse Norm");

        }
        else
        {
            print("Niche ne norm");
        }
    }

    public void AvatarSelectGrabExitEventHub(NetworkObject netObj, bool wichHand) {
        if (wichHand)
            leftGrabbedObject = null;
        else
            rightGrabbedObject = null;

        setIsKinematicServerRpc(netObj, false);




    }
    private bool canShoot = true;
   
    
    /*public void ShootActive(NetworkObject pistolObj)
    {
        if (!IsOwner) return;
        pistolObj.GetComponent<XRGrabInteractable>().activated.AddListener((ActivateEventArgs arg) =>
        {
            if (canShoot)
            {
                canShoot = false;
                SpawnBulletServerRPC(spawnPoint.position, spawnPoint.rotation);
                StartCoroutine(ResetShootFlag());
            }
        }); 
    }
    private IEnumerator ResetShootFlag()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }*/

    public GameObject bulletPrefab;
    float bulletSpeed = 20f;

    
    [ServerRpc] public void SpawnBulletServerRPC(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (!IsServer) return;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        NetworkObject networkObject = bullet.GetComponent<NetworkObject>();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.forward * bulletSpeed;
        }
        if (networkObject != null)
        {
            networkObject.Spawn();
        }
    }

    [ServerRpc] public void MoveGrabbedObjectServerRpc(NetworkObjectReference grabbedObj, Vector3 position , Quaternion rotation) {
        if (!IsServer) return;

        if (grabbedObj.TryGet(out NetworkObject netObj)) {
            netObj.transform.position = position;
            netObj.transform.rotation = rotation;
            Debug.Log("Client moved object");
        }
    }

    [ServerRpc] public void setIsKinematicServerRpc(NetworkObjectReference grabbedObj, bool value) {
        if (!IsServer) return;

        if (grabbedObj.TryGet(out NetworkObject netObj)) {
            netObj.GetComponent<Rigidbody>().isKinematic = value;
        }
    }
}
