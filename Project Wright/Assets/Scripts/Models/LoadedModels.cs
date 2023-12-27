using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Models
{
    [RequireComponent(typeof(LevelProgressionModel))]
    public class LoadedModels : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            Messenger.Broadcast(GameEvents.START);
        }
    }
}