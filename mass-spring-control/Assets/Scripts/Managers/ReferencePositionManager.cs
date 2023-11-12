#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Value = System.Collections.Generic.List<UnityEngine.Vector3>;

namespace Managers
{
    public class ReferencePositionManager : MonoBehaviour, IGameManager
    {
        [Serializable]
        public class ReferencePosition
        {
            public float Time;
            public Value Position = new();
        }
        
        public ManagerStatus Status { get; private set; }
        public void Startup()
        {
            Status = ManagerStatus.Started;
            
            referencePositions.Add(new ReferencePosition{ Time = 5, Position = new Value()
            {
                new(1.035f, 0.7348798f, 0.9054227f),
                new(3.165f, 0.7348798f, 0.9054227f),
                new(1.035f, 2.36512f, -0.4654227f),
                new(3.165f, 2.36512f, -0.4654227f)
            }});
        }
        
        [SerializeField] private List<ReferencePosition> referencePositions = new();

        public void AddReferencePosition(float time, Value position)
        {
            referencePositions.Add(new ReferencePosition{Time = time, Position =  position});
            referencePositions.Sort();
        }

        public Value? GetCurrentReferencePosition(float elapsed)
        {
            Value? position = null;
            
            foreach (var value in referencePositions)
            {
                if (elapsed >= value.Time)
                {
                    position = value.Position;
                }
            }

            return position;
        }

        public List<ReferencePosition> GetReferencePositions() => referencePositions;

        public Value GetReferencePosition(int index) => referencePositions[index].Position;
        
        public void UpdateReferencePosition(int index, Value value)
        {
            Assert.IsTrue(referencePositions.Count > index && index >= 0);

            referencePositions[index].Position = value;
        }
    }
}
