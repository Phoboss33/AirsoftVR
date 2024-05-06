using UnityEngine;
using Unity.Netcode;

namespace Core.Base
{
    public class PlayerObjectAssignButtons: NetworkBehaviour
    {
        public int prefabIndex;
        
        public void OnButtonPressed()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                // Получаем клиента, который нажал кнопку. В этом случае это будет локальный клиент.
                ulong localClientId = NetworkManager.Singleton.LocalClientId;

                // Вызываем метод привязки объекта из нашего Singleton-а
                ObjectManager.Instance.AssignPlayerObject(localClientId, prefabIndex);
            }
        }
    }
}