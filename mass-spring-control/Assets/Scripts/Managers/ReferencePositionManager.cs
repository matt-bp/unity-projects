#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Value = System.Collections.Generic.List<UnityEngine.Vector3>;

namespace Managers
{
    public class ReferencePositionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public void Startup()
        {
            Status = ManagerStatus.Started;
            
            referencePositions.Add((0, new Value()
            {
                new(1.035f, 0.7348798f, 0.9054227f),
                new(3.165f, 0.7348798f, 0.9054227f),
                new(1.035f, 2.36512f, -0.4654227f),
                new(3.165f, 2.36512f, -0.4654227f)
            }));
        }
        
        private readonly List<(float Time, Value Position)> referencePositions = new();

        public void AddReferencePosition(float time, Value position)
        {
            referencePositions.Add((time, position));
            referencePositions.Sort();
        }

        public Value? GetCurrentReferencePosition(float elapsed)
        {
            Value? position = null;
            
            foreach (var (time, pos) in referencePositions)
            {
                if (elapsed >= time)
                {
                    position = pos;
                }
            }

            return position;
        }

        public List<(float Time, Value Position)> GetReferencePositions() => referencePositions;

        public void UpdateReferencePosition(int index, Value value)
        {
            Assert.IsTrue(referencePositions.Count > index && index >= 0);

            referencePositions[index] = (referencePositions[index].Time, value);
        }
    }
}
