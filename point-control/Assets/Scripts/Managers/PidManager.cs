using System;
using UnityEngine;

namespace Managers
{
    public class PidManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }

        
        public float kp;
        public float ki;
        public float kd;

        private float commandVariable;
        private float integral;
        private float state;
        
        public void Startup()
        {
            Status = ManagerStatus.Started;
        }

        public void SetCommandVariable(float newValue)
        {
            commandVariable = newValue;
        }

        public float DoUpdate(float newMeasurement)
        {
            var error = commandVariable - newMeasurement;

            integral += error;

            var p = kp * error;
            var i = ki * integral;
            var d = kd * (error - state);
            state = error;

            return p + i + d;
        }
    }
}