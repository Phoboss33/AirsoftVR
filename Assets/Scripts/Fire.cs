using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;

public class Fire : NetworkBehaviour {
    public GameObject bulletPrefab;
    public Transform spawnPoint;

    void Start() {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(FireBullet);
    }

    public void FireBullet(ActivateEventArgs arg) {
        if (IsServer) {
            SpawnBullet(spawnPoint.position, spawnPoint.rotation); // Непосредственно создаем пулю на сервере
        }
        else {
            RequestFireServerRpc(); // Запрашиваем создание пули на сервере
        }
    }

    // Функция создания пули, которая будет вызываться на сервере
    private void SpawnBullet(Vector3 position, Quaternion rotation) {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
        Destroy(bullet, 5);
    }

    [ServerRpc(RequireOwnership = false)] // Этот метод будет вызван на сервере по запросу клиента
    void RequestFireServerRpc(ServerRpcParams rpcParams = default) {
        SpawnBullet(spawnPoint.position, spawnPoint.rotation); // Вызываем функцию создания пули на сервере
    }
}