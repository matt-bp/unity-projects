using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Models
{
    [RequireComponent(typeof(LevelProgressionModel))]
    [RequireComponent(typeof(TaskMeasurementModel))]
    [RequireComponent(typeof(DataExportModel))]
    public class LoadedModels : MonoBehaviour
    {
        public static TaskMeasurementModel Measurement { get; private set; }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            Measurement = GetComponent<TaskMeasurementModel>();

            Messenger.Broadcast(GameEvents.START);
        }
    }
}