using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

namespace Core.Base
{
    public class UIManager: NetworkBehaviour
    {
        [SerializeField] private List<Button> buttons = new List<Button>(100);
        [SerializeField] private List<int> objectIds = new List<int>(100);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            if (buttons.Count==objectIds.Count)
            {
                for(int i=0; i<buttons.Count; i++)
                {
                    var btn = buttons[i];
                    btn.onClick.AddListener(() =>
                    {
                        ObjectManager.Instance.AssignPlayerObject(localClientId, objectIds[i]);
                        Debug.LogError($"Btn spawn object: {objectIds[i]} userId: {localClientId}");
                    });
                } 
            }
            else
            {
                Debug.LogError($"sizes objectIds and buttons do not match");
            }
        }

        private void Awake()
        {
            
        }
    }
}