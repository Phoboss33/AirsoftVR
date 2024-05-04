using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletTrigger : NetworkBehaviour {
    public NetworkVariable<int> health = new NetworkVariable<int>();

    private void Awake() {
        health.Value = 3;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Bullet") {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
    }

    private void Update() {
        if (health.Value <= 0) {

            Die();
        }
    }

    [ServerRpc]
    private void TakeDamageServerRpc(int damage, ServerRpcParams rpcParams = default) {
        health.Value -= damage;
    }

    private void TakeDamage(int damage) {
        TakeDamageServerRpc(damage);
    }

    [ServerRpc]
    private void DieServerRpc(ServerRpcParams rpcParams = default) {
        print($"{OwnerClientId} - Умер!!");
        Destroy(gameObject);
        NetworkObject.Despawn();
    }

    private void Die() {
        DieServerRpc();
    }
}