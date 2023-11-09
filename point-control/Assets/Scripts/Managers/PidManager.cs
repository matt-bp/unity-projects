using UnityEngine;
using Value = UnityEngine.Vector2;

namespace Managers
{
    public class PidManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }

        /// <summary>
        /// Global control that adjusts each value together.
        /// </summary>
        public float kg = 1;
        public float kp;
        public float ki;
        public float kd;

        private Value commandVariable;
        private Value integral;
        private Value state;
        
        public void Startup()
        {
            Status = ManagerStatus.Started;
        }

        public void SetCommandVariable(Value newValue)
        {
            commandVariable = newValue;
        }

        public Value DoUpdate(Value newMeasurement)
        {
            var error = commandVariable - newMeasurement;

            integral += error;

            var p = kg * kp * error;
            var i = kg * ki * integral;
            var d = kg * kd * (error - state);
            state = error;

            return p + i + d;
        }

        public void Reset()
        {
            integral = default;
        }
    }
}
