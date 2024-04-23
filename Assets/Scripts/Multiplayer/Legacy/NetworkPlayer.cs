using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Vector2 placementArea = new Vector2(-2f, 2f);

    public override void OnNetworkSpawn() {
        DisableClientInput();
    }

    public void DisableClientInput() {
        if (IsClient && !IsOwner) {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();

            clientCamera.enabled = false;
            clientMoveProvider.enableInputActions = false;
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
            clientHead.enabled = false;

            foreach (var controller in clientControllers) {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }


        }
    }

    private void Start() {
        if (IsClient && IsOwner) {
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), transform.position.y, Random.Range(placementArea.x, placementArea.y));
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs) {
        if (IsClient && IsOwner) {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform
                .GetComponent<NetworkObject>();

            if (networkObjectSelected != null)
            {
                // request owership from the server
                RequestGrabbableOwershipServerRpc(OwnerClientId, networkObjectSelected);
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference) {
        if (networkObjectReference.TryGet(out NetworkObject networkObject)) {
            networkObject.ChangeOwnership(newOwnerClientId);

        }
        else {
            print("Unable to change owership for clientIF " + newOwnerClientId.ToString());
            
        }
    }
}
