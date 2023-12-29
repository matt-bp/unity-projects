using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Models
{
    public class TaskMeasurementModel : MonoBehaviour
    {
        private double _timeSinceLastUpdate = 0;
        
        private void OnEnable()
        {
            Debug.Log("TMM Enable");
        }

        private void OnDisable()
        {
            Debug.Log("TMM Disable");
        }

        public double ElapsedTimeSeconds { get; private set; }

        public void IncrementTime()
        {
            Cloth c;
            ElapsedTimeSeconds += Time.deltaTime;
            _timeSinceLastUpdate += Time.deltaTime;

            if (_timeSinceLastUpdate >= 1)
            {
                Messenger<double>.Broadcast(ModelToPresenter.TIME_UPDATE, ElapsedTimeSeconds);
                _timeSinceLastUpdate -= 1;
            }
        }

        public void ResetTime()
        {
            Debug.Log("Resetting time");
            _timeSinceLastUpdate = 0;
            ElapsedTimeSeconds = 0;
        }
    }
}