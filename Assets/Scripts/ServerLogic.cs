using Unity.Netcode;
using TMPro;
using UnityEngine;
using System.Collections;

public class ServerLogic : NetworkBehaviour
{
    public NetworkVariable<int> playerValue = new NetworkVariable<int>();
    [SerializeField] private int maxPlayerCount = 10; 

    public TextMeshProUGUI text;

    private Coroutine timerCoroutine;
    private int countdownTime = 10;
    private bool timerIsActive = false;


    private void Start()
    {
        playerValue.OnValueChanged += UpdatePlayerCountUI;
    }

    private void StopTimerAndShowPlayersCount()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        timerIsActive = false;

        text.text = playerValue.Value.ToString() + "/" + maxPlayerCount;
    }

    private void UpdatePlayerCountUI(int oldValue, int newValue)
    {
        if (timerIsActive && newValue != maxPlayerCount)
        {
            StopTimerAndShowPlayersCount();
        }
        else if (!timerIsActive)
        {
            text.text = newValue.ToString() + "/" + maxPlayerCount;
        }

        if (newValue == maxPlayerCount && timerCoroutine == null)
        {
            timerIsActive = true;
            timerCoroutine = StartCoroutine(TimerCountdown());
        }
    }

    private IEnumerator TimerCountdown()
    {
        int timeLeft = countdownTime;
        while (timeLeft > 0)
        {
            text.text = timeLeft.ToString();
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        timerIsActive = false;
        text.text = "0"; 
        UpdatePlayerCountUI(0, playerValue.Value); 

        timerCoroutine = null;
    }


    [ServerRpc(RequireOwnership = false)]
    public void IncreasePlayerCountServerRpc()
    {
        playerValue.Value++;
    }

    [ServerRpc(RequireOwnership = false)]
    public void decreasePlayerCountServerRpc()
    {
        playerValue.Value--;
    }
}