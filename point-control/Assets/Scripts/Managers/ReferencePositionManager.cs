using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class ReferencePositionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public void Startup()
        {
            Status = ManagerStatus.Started;
            
            referencePositions.Add(1, 2);
            referencePositions.Add(5, 4);
            referencePositions.Add(10, 7);
            referencePositions.Add(15, 10);
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
    }
}
