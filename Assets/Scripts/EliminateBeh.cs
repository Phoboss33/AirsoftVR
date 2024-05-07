using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class EliminateBeh : NetworkBehaviour
{
    public GameObject myGeo;
    public Transform SpawnPosition;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SpawnPosition = GameObject.FindGameObjectWithTag("DieSpawnPoint");

    }


    [ServerRpc(RequireOwnership = false)]
    public void InviseMePLZYouOkeyBroServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong triggeringPlayerId = rpcParams.Receive.SenderClientId;
        InvisePlayerClientRpc(triggeringPlayerId);
    }

    [ClientRpc]
    private void InvisePlayerClientRpc(ulong playerId, ClientRpcParams rpcParams = default)
    {
        if (NetworkManager.Singleton.LocalClientId == playerId)
        {
            myGeo.SetActive(false);
        }
    }
}
