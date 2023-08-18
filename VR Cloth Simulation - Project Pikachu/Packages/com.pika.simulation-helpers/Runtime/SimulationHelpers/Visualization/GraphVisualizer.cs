using System.Collections.Generic;
using UnityEngine;

namespace SimulationHelpers.Visualization
{
    /// <summary>
    /// This component stores information from the simulation, and then creates a CSV file containing all the stored data.
    ///
    /// This is especially helpful, in conjunction with one shot simulation, to test base numerical stability of the underlying cloth simulation methods.
    /// </summary>
    [AddComponentMenu("Simulation Helpers/Visualization/Graph Visualizer")]
    public class GraphVisualizer : ClothVisualizer
    {
        public override void Visualize(List<Vector3> positions)
        {
            Debug.Log("Graphing!");
        }
    }
}