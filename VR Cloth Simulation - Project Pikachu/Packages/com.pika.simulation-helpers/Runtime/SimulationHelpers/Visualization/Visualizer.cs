using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace SimulationHelpers.Visualization
{
    public class Visualizer : MonoBehaviour
    {
        public virtual void Visualize(List<double3> positions, float elapsed, float dt)
        {
            Debug.Log("Noting");
        }
    }
}