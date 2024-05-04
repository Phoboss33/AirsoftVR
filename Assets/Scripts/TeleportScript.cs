using System;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public int TeleportId;
    public ServerLogic serverLogic;
    private GameObject player;

    void Start()
    {
        ServerLogic.action += TeleportInPosition;
    }

    void OnDestroy()
    {
        ServerLogic.action -= TeleportInPosition;
    }

    private void TeleportInPosition()
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().TeleportOnPoint(TeleportId);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            serverLogic.IncreasePlayerCountServerRpc();
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            serverLogic.decreasePlayerCountServerRpc();
            player = null;
        }
    }
}