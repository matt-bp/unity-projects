using System;
using UnityEngine;

namespace Managers
{
    public class PidManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public double CommandVariable { get; private set; }

        private double integral;
        private double state;
        
        public void Startup()
        {
            Status = ManagerStatus.Started;
        }

        public void SetCommandVariable(double newValue)
        {
            CommandVariable = newValue;
        }

        public double UpdateError(double newMeasurement)
        {
            throw new NotImplementedException();
        }
    }
}