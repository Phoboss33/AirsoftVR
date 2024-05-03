using Unity.Netcode;
using TMPro;
using UnityEngine;

public class ServerLogic : NetworkBehaviour
{
    public NetworkVariable<int> playerValue = new NetworkVariable<int>();

    public TextMeshProUGUI text;

    private void Start()
    {
        playerValue.OnValueChanged += UpdatePlayerCountUI; 
    }

    private void UpdatePlayerCountUI(int oldValue, int newValue)
    {
        text.text = newValue.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreasePlayerCountServerRpc()
    {
        playerValue.Value++;
    }
}