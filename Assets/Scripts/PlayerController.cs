using Core.Base;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform resetTransform;
    [SerializeField] GameObject player;
    [SerializeField] Camera playerHead;

    public Transform[] SpawnPoints;

    public Transform[] SpawnPointObjects;

    [ContextMenu("Reset Position")]

    public void ResetPosition()
    {
        var rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;

        player.transform.Rotate(0, rotationAngleY, 0);

        var distanceDiff = resetTransform.position - playerHead.transform.position;

        player.transform.position += distanceDiff;
    }
    private void Start()
    {
        Invoke("ResetPosition", 0.1f);
    }

    public void TeleportOnPoint(int point)
    {
        var rotationAngleY = SpawnPoints[point].rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;

        player.transform.Rotate(0, rotationAngleY, 0);

        var distanceDiff = SpawnPoints[point].position - playerHead.transform.position;

        player.transform.position += distanceDiff;
        
        ObjectManager.Instance.RequestSpawnSelectedObject(NetworkManager.Singleton.LocalClientId, SpawnPointObjects[point].position);
    }
}
