using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace SimulationHelpers.Visualization
{
    public class Visualizer : MonoBehaviour
    {
        public virtual void Visualize(List<Vector3> positions, float elapsed, float dt)
        {
            Debug.Assert(false);
        }

        public virtual void Clear()
        {
            Debug.Assert(false);
        }
    }
}