using Unity.Netcode;
using TMPro;
using UnityEngine;

public class ServerLogic : NetworkBehaviour
{
    public NetworkVariable<int> playerValue = new NetworkVariable<int>();

    public TextMeshProUGUI text;

    private void Start()
    {
        playerValue.OnValueChanged += UpdatePlayerCountUI; // �������� callback �� ��������� ��������
    }

    private void UpdatePlayerCountUI(int oldValue, int newValue)
    {
        text.text = newValue.ToString();
    }

    [ServerRpc] // �������, �����������, ��� ����� ����� ���� ������ �� ������� �� �������
    public void IncreasePlayerCountServerRpc()
    {
        playerValue.Value++; // �������� �������� ������� ����������
    }
}