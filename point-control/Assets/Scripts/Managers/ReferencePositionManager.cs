using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers
{
    public class ReferencePositionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public void Startup()
        {
            Status = ManagerStatus.Started;
            
            referencePositions.Add(0, -5);
            // referencePositions.Add(1, 10);
        }
        
        private readonly SortedDictionary<float, float> referencePositions = new();

        public void AddReferencePosition(float time, float position)
        {
            referencePositions.Add(time, position);
        }

        public float? GetCurrentReferencePosition(float elapsed)
        {
            float? position = null;
            
            foreach (var (time, pos) in referencePositions)
            {
                if (elapsed >= time)
                {
                    position = pos;
                }
            }

            return position;
        }

        public SortedDictionary<float, float> GetReferencePositions() => referencePositions;

        public void UpdateReferencePosition(int index, float value)
        {
            Assert.IsTrue(referencePositions.ContainsKey(index));

            referencePositions[index] = value;
        }
    }
}
