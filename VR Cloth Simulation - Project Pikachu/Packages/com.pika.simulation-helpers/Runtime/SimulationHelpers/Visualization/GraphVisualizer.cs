using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimulationHelpers.Visualization
{
    /// <summary>
    /// This component stores information from the simulation, and then creates a CSV file containing all the stored data.
    ///
    /// This is especially helpful, in conjunction with one shot simulation, to test base numerical stability of the underlying cloth simulation methods.
    /// </summary>
    [AddComponentMenu("Simulation Helpers/Visualization/Graph Visualizer")]
    public class GraphVisualizer : Visualizer
    {
        #region Keyboard Shortcuts
        
        [SerializeField] private InputActionReference createReport;
        [SerializeField] private InputActionReference resetStatistics;
        
        #endregion
        
        private List<IRunStatistic> runStatistics = new();
        
        public override void Visualize(List<Vector3> positions, float elapsed, float dt)
        {
            runStatistics.Add(new RunStatistic3D
            {
                Elapsed = elapsed,
                DeltaTime = dt
            });
        }

        private void Update()
        {
            if (createReport.action.WasPerformedThisFrame())
            {
                Debug.Log($"Creating a report for {runStatistics.Count} stats.");
            }
            
            if (resetStatistics.action.WasPerformedThisFrame())
            {
                Debug.Log("Resetting statistics!");
                runStatistics = new List<IRunStatistic>();
            }
        }
    }
}