using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Value = System.Collections.Generic.List<UnityEngine.Vector3>;

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

        private List<Vector3?> commandVariable = new();
        private Value integral = new();
        private Value state = new();
        
        public void Startup()
        {
            Status = ManagerStatus.Started;
        }

        public void SetCommandVariable(List<Vector3?> newValue)
        {
            commandVariable = newValue;

            if (integral == null || !integral.Any())
            {
                integral = Enumerable.Repeat(Vector3.zero, newValue.Count).ToList();
            }

            if (state == null || !state.Any())
            {
                state = Enumerable.Repeat(Vector3.zero, newValue.Count).ToList();
            }
        }

        public Value DoUpdate(IEnumerable<Vector3> newMeasurement)
        {
            Debug.Assert(commandVariable.Any());
            Debug.Assert(integral.Any());
            Debug.Assert(state.Any());
            
            var update = new Value();
            
            foreach (var item in newMeasurement.Select((v, i) => new {v, i}))
            {
                var measurement = item.v;
                var index = item.i;

                // If we're not controlling this index, return a zero for error, we're where we want to be!
                if (commandVariable[index] is null)
                {
                    update.Add(Vector3.zero);
                    continue;
                }
                
                var error = (commandVariable[index] - measurement).Value;
                
                integral[index] += error;

                var p = kg * kp * error;
                var i = kg * ki * integral[index];
                var d = kg * kd * (error - state[index]);
                state[index] = error;
                
                update.Add(p + i + d);
            }

            return update;
        }

        public void Reset()
        {
            integral = default;
        }
    }
}
