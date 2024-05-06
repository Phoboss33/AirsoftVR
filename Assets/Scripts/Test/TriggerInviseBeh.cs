using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInviseBeh : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("SwatTest"))
        {
            
            other.gameObject.GetComponent<EliminateBeh>().InviseMePLZYouOkeyBroServerRpc();
        }
        if (other.gameObject.CompareTag("Player"))
        {

            other.gameObject.GetComponent<PlayerInGameBehavior>().InviseMe();
        }
    }
}
