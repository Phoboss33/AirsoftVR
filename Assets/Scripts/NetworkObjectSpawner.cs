using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; 


    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc()
    {
        GameObject newObj = Instantiate(objectPrefab);
        newObj.GetComponent<NetworkObject>().Spawn();
    }
}
