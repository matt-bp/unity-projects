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
        
        private readonly SortedDictionary<float, Value> referencePositions = new();

        public void AddReferencePosition(float time, Value position)
        {
            referencePositions.Add(time, position);
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

        public SortedDictionary<float, Value> GetReferencePositions() => referencePositions;

        public void UpdateReferencePosition(int index, Value value)
        {
            Assert.IsTrue(referencePositions.ContainsKey(index));

            referencePositions[index] = value;
        }
    }
}
