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
            SpawnBullet(spawnPoint.position, spawnPoint.rotation); // ��������������� ������� ���� �� �������
        }
        else {
            RequestFireServerRpc(); // ����������� �������� ���� �� �������
        }
    }

    // ������� �������� ����, ������� ����� ���������� �� �������
    private void SpawnBullet(Vector3 position, Quaternion rotation) {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
        Destroy(bullet, 5);
    }

    [ServerRpc(RequireOwnership = false)] // ���� ����� ����� ������ �� ������� �� ������� �������
    void RequestFireServerRpc(ServerRpcParams rpcParams = default) {
        SpawnBullet(spawnPoint.position, spawnPoint.rotation); // �������� ������� �������� ���� �� �������
    }
}