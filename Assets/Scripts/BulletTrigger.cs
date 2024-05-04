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
            print("Умер");
            Die();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(int damage, ServerRpcParams rpcParams = default) {
        health.Value -= damage;
    }

    private void TakeDamage(int damage) {
        if (IsServer) {
            health.Value -= damage;
        }
        else {
            TakeDamageServerRpc(damage);
        }
    }

    private void Die() {
        if (IsServer) {
            print($"{OwnerClientId} - Умер!!");
            Destroy(gameObject);
            NetworkObject.Despawn();
            
        }
    }
}