#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Value = UnityEngine.Vector2;

namespace Managers
{
    public class ReferencePositionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public void Startup()
        {
            Status = ManagerStatus.Started;
        }
        
        private readonly List<(float Time, Value Position)> referencePositions = new();

        public void AddReferencePosition(float time, Value position)
        {
            referencePositions.Add((time, position));
            referencePositions.Sort();

            foreach (var (item, index) in referencePositions.WithIndex())
            {
                Debug.Log($"{index} contains {item}");
            }
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
