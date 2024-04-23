using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LaunchUI : MonoBehaviour {
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    private void Awake() {
        HostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        ClientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
