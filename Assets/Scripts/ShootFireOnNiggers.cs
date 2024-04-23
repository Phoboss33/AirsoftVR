using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootFireOnNiggers : NetworkBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform point;
    private void Start()
    {
        if (IsOwner && (IsServer || IsClient))
        {
            XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
            grabbable.activated.AddListener((ActivateEventArgs arg) => ShootFireServerRPC(point.position, point.rotation));
        }
    }

    [ServerRpc]
    private void ShootFireServerRPC(Vector3 position, Quaternion rotation)
    {
        GameObject InstansiateBullet = Instantiate(bullet, position, rotation);
        InstansiateBullet.GetComponent<NetworkObject>().Spawn();
        Destroy(InstansiateBullet, 5);
    }
}
