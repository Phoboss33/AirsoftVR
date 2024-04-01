using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class BulletSpawn : NetworkBehaviour
{
    public GameObject bulletPrefab;
    float bulletSpeed = 20f;
    public Transform SpawnPoint;

    [ClientRpc]
    public void SpawnBulletClientRPC()
    {
        GameObject bullet = Instantiate(bulletPrefab, SpawnPoint.transform.position, SpawnPoint.rotation);
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
}