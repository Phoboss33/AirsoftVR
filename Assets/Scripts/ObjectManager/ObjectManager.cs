using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Base
{
    public class ObjectManager: NetworkSingleton<ObjectManager>
    {
        [FormerlySerializedAs("Items")] public List<GameObject> playerObjectPrefabs;
        private Dictionary<ulong, GameObject> _playerObjectsToSpawn = new Dictionary<ulong, GameObject>(100);
        [SerializeField] private GameObject placeForSpawnItem;
        
        public void AssignClientObject(ulong clientId, int prefabIndex)
        {
            if (IsServer) // Только сервер может привязывать объекты
            {
                if (prefabIndex >= 0 && prefabIndex < playerObjectPrefabs.Count())
                {
                    _playerObjectsToSpawn[clientId] = playerObjectPrefabs[prefabIndex];
                }
                else
                {
                    Debug.LogError("Prefab index out of range. Cannot assign player object.");
                }
            }
        }
       
        public void AssignPlayerObject(ulong clientId, int prefabIndex)
        {
            if (IsServer) // Только сервер может привязывать объекты
            {
                // Проверяем, что переданный индекс находится в пределах массива
                if (prefabIndex >= 0 && prefabIndex < playerObjectPrefabs.Count())
                {
                    // Помещаем префаб в словарь привязки к игрокам
                    _playerObjectsToSpawn[clientId] = playerObjectPrefabs[prefabIndex];
                    Debug.LogError($"prefabId: {prefabIndex}; client: {clientId}");
                }
                else
                {
                    Debug.LogError("Prefab index out of range. Cannot assign player object.");
                }
            }
        }
        
        public void SpawnPlayerObject(ulong clientId, Vector3 position)
        {
            // Если в словаре есть префаб для данного ID клиента
            if (_playerObjectsToSpawn.TryGetValue(clientId, out GameObject playerObjectPrefab))
            {
                // Создаём объект из префаба
                GameObject playerObject = Instantiate(playerObjectPrefab, position, Quaternion.identity);
                // Спавним объект с NetworkObject, привязываем к ID клиента
                playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

                // Удаляем префаб из словаря после спаунинга
                _playerObjectsToSpawn.Remove(clientId);
            }
            else
            {
                Debug.LogError("No player object assigned for clientId: " + clientId);
            }
        }
        
    }
}