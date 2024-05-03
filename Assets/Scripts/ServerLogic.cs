using Unity.Netcode;
using TMPro;
using UnityEngine;

public class ServerLogic : NetworkBehaviour
{
    public NetworkVariable<int> playerValue = new NetworkVariable<int>();

    public TextMeshProUGUI text;

    private void Start()
    {
        playerValue.OnValueChanged += UpdatePlayerCountUI; // ƒобавить callback на изменение значени€
    }

    private void UpdatePlayerCountUI(int oldValue, int newValue)
    {
        text.text = newValue.ToString();
    }

    [ServerRpc] // јтрибут, указывающий, что метод может быть вызван из клиента на сервере
    public void IncreasePlayerCountServerRpc()
    {
        playerValue.Value++; // ќбновить значение сетевой переменной
    }
}