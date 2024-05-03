using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public int TeleportId;
    public ServerLogic serverLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // ����� ServerRpc ��� ���������� ��������
            serverLogic.IncreasePlayerCountServerRpc();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // ����� ServerRpc ��� ���������� ��������
            serverLogic.decreasePlayerCountServerRpc();
        }
    }
}