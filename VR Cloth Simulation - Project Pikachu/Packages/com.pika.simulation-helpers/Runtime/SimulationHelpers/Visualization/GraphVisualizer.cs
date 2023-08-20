using System;
using System.Collections.Generic;
using Unity.Mathematics;
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
        
        [SerializeField] private string outputFolder = "./Stats";
        private List<IRunStatistic> runStatistics = new();
        private readonly Guid runIdentifier = Guid.NewGuid();
        private int? expectedPositionCount;
        
        public override void Visualize(List<double3> positions, float elapsed, float dt)
        {
            if (expectedPositionCount is null) 
                expectedPositionCount = positions.Count;
            
            Debug.Assert(positions.Count == expectedPositionCount);
            
            runStatistics.Add(new RunStatistic3D
            {
                Elapsed = elapsed,
                DeltaTime = dt,
                Positions = positions
            });
        }

        private void Update()
        {
            if (createReport.action.WasPerformedThisFrame())
            {
                CreateReport();
            }
            
            if (resetStatistics.action.WasPerformedThisFrame())
            {
                Reset();
            }
        }
        
        private void CreateReport()
        {
            if (runStatistics.Count == 0)
            {
                Debug.Log("There are no statistics to write, skipping noop.");
                return;
            }
            
            var filename = $"{outputFolder}/stats-{runIdentifier}";
            Debug.Log("Creating report: " + filename);
            RunStatisticWriter.Write(runStatistics, filename, "csv");
        }

        private void Reset()
        {
            runStatistics = new List<IRunStatistic>();
            expectedPositionCount = null;
        }
    }
}