using System;
using System.Collections.Generic;
using System.IO;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._10_Implicit_MassSpring_3D.Scripts
{
    public class SimulationStatisticsProcessor : MonoBehaviour
    {
        #region Keyboard Shortcuts

        [SerializeField] private InputAction createReport;
        [SerializeField] private InputAction resetStatistics;

        #endregion

        [SerializeField] private string outputFolder = "./Stats";
        private List<IRunStatistic> runStatistics = new();
        private readonly Guid runIdentifier = Guid.NewGuid();

        private void Start()
        {
            if (!Directory.Exists(outputFolder))
            {
                Debug.LogError($"The simulation stats output folder {outputFolder} doesn't exist. Please create it or change the output folder.");
            }
            
            createReport.Enable();
            resetStatistics.Enable();
        }
        
        private void Update()
        {
            if (createReport.WasPerformedThisFrame())
            {
                CreateReport();
            }

            if (resetStatistics.WasPerformedThisFrame())
            {
                runStatistics = new List<IRunStatistic>();
            }
        }
        
        private void CreateReport()
        {
            var filename = $"./Stats/stats-{runIdentifier}";
            Debug.Log("Creating report here: " + filename);
            StatsWriter.WriteRunStatistics(runStatistics, filename, "csv");
        }

        public void AddStat(RunStatistic2D stat)
        {
            runStatistics.Add(stat);
        }
    }
}