using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UIButtonHandler : MonoBehaviour
{
    //public NetworkObjectSpawner spawner;

    void Start()
    {

    }

    private void OnButtonClicked()
    {
        // ���������, �������� �� �� ������ ��� ��������, � �������� RPC
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            //spawner.SpawnObjectServerRpc();
        }
        else
        {
            Debug.LogError("Only the server or host can spawn objects.");
        }
    }
}
