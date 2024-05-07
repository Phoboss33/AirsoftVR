using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Core.Base
{
    public class ObjectManager: NetworkSingleton<ObjectManager>
    {
        public List<GameObject> playerObjectPrefabs;
        private Dictionary<ulong, GameObject> _playerObjectsToSpawn = new Dictionary<ulong, GameObject>(100);
        [SerializeField] private GameObject placeForSpawnItem;

        public void AssignPlayerObject(ulong clientId, int prefabIndex)
        {
            if (IsServer) // Только сервер может привязывать объекты
            {
                if (prefabIndex >= 0 && prefabIndex < playerObjectPrefabs.Count())
                {
                    _playerObjectsToSpawn[clientId] = playerObjectPrefabs[prefabIndex];
                    Debug.LogError($"prefabId: {prefabIndex}; client: {clientId}");
                }
                else
                {
                    Debug.LogError("Prefab index out of range. Cannot assign player object.");
                }
            }
        }
        
        // Добавим функцию RPC для запроса спавна объекта клиентом
        [ServerRpc(RequireOwnership = false)]
        public void RequestAssignPlayerObjectServerRpc(ulong clientId, int prefabIndex)
        {
            AssignPlayerObject(clientId, prefabIndex);
        }
        
        public void SpawnPlayerObject(ulong clientId, Vector3 position)
        {
            if (_playerObjectsToSpawn.TryGetValue(clientId, out GameObject playerObjectPrefab))
            {
                GameObject playerObject = Instantiate(playerObjectPrefab, position, Quaternion.identity);
                playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

                _playerObjectsToSpawn.Remove(clientId);
            }
            else
            {
                Debug.LogError("No player object assigned for clientId: " + clientId);
            }
        }

        // Добавим функцию RPC для запроса спавна объекта с указанными координатами
        [ServerRpc(RequireOwnership = false)]
        public void RequestSpawnPlayerObjectServerRpc(ulong clientId, Vector3 position)
        {
            // Проверка наличия префаба для данного клиента необходима,
            // так как мы не хотим повторно назначать префаб, который уже есть в словаре.
            if (!_playerObjectsToSpawn.ContainsKey(clientId))
            {
                Debug.LogError("Attempted to spawn player object without assignment.");
                return;
            }

            SpawnPlayerObject(clientId, position);
        }
        
        
        // Метод вызываемый на клиенте для выбора префаба объекта (на основе индекса)
        public void SelectAndRequestAssignPlayerObject(ulong clientId, int prefabIndex)
        {
            //ulong clientId = NetworkManager.Singleton.LocalClientId;
            // Вызов RPC для запроса назначения объекта на сервере
            ObjectManager.Instance.RequestAssignPlayerObjectServerRpc(clientId, prefabIndex);
        }

        // Метод вызываемый на клиенте для спавна выбранного объекта по координатам
        public void RequestSpawnSelectedObject(ulong clientId, Vector3 spawnPosition)
        {
            // ulong clientId = NetworkManager.Singleton.LocalClientId;
            // Вызов RPC для запроса спавна объекта на сервере
            ObjectManager.Instance.RequestSpawnPlayerObjectServerRpc(clientId, spawnPosition);
        }
    }
    
    
}