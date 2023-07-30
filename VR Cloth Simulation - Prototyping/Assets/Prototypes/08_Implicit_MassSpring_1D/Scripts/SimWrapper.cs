using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using MattMath;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._08_Implicit_MassSpring_1D.Scripts
{
    public class SimWrapper : MonoBehaviour
    {
        private readonly ImplicitMassSpring1D system = new();
        private IList<double> initialPositions;
        [SerializeField] private GameObject massPrefab;
        private readonly List<GameObject> createdPrefabs = new();
        
        private List<IRunStatistic> runStatistics = new();
        /// <summary>
        /// Time, in seconds, since the simulation run started.
        /// </summary>
        private double Elapsed;
        [SerializeField] private InputAction createReport;
        [SerializeField] private InputAction resetSimulation;
        [SerializeField] private InputAction toggleSimulation;
        public bool isEnabled;
        private readonly Guid runIdentifier = Guid.NewGuid();
    
        private void Start()
        {
            initialPositions = system.positions.Select(x => x).ToList().AsReadOnlyList();
            
            foreach (var pos in system.positions)
            {
                var newPrefab = Instantiate(massPrefab);
                createdPrefabs.Add(newPrefab);

                newPrefab.transform.position = new Vector3(0, (float)pos, 0);
            }
            
            createReport.Enable();
            resetSimulation.Enable();
            toggleSimulation.Enable();
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (createReport.WasPerformedThisFrame())
            {
                CreateReport();
            }

            if (resetSimulation.WasPerformedThisFrame())
            {
                Debug.Log("Reset!");
                for (var i = 0; i < initialPositions.Count; i++)
                {
                    system.positions[i] = initialPositions[i];
                }

                UpdateMassVisualization();
                runStatistics = new List<IRunStatistic>();
            }

            if (toggleSimulation.WasPerformedThisFrame())
            {
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;

            RunSimulation();
            
            // OneShotSimulation();
        }

        private void CreateReport()
        {
            var filename = $"./Stats/stats-{runIdentifier}";
            Debug.Log("Creating report here: " + filename);
            StatsWriter.WriteRunStatistics(runStatistics, filename, "csv");
        }

        private void RunSimulation()
        {
            system.Update(Time.deltaTime);

            UpdateMassVisualization();

            Elapsed += Time.deltaTime;

            runStatistics.Add(new RunStatistics1D()
            {
                Elapsed = Elapsed,
                Position = system.positions[0],
                DeltaTime = Time.deltaTime
            });
        }

        private void UpdateMassVisualization()
        {
            for (var i = 0; i < system.positions.Count; i++)
            {
                createdPrefabs[i].transform.position = new Vector3(0, (float)system.positions[i], 0);
            }
        }

        /// <summary>
        /// Runs the simulation with a fixed time step. Primarily used to test simulation parameters. Immediately creates a CSV report with run statistics.
        /// </summary>
        private void OneShotSimulation()
        {
            Debug.Log("Running one shot simulation. NOT in real time!");
            
            foreach(var _ in Enumerable.Range(0, 1000))
            {
                system.Update(Time.deltaTime);

                Elapsed += Time.deltaTime;

                runStatistics.Add(new RunStatistics1D()
                {
                    Elapsed = Elapsed,
                    Position = system.positions[0],
                    DeltaTime = Time.deltaTime
                });
            }

            isEnabled = false;
            
            CreateReport();
        }
    }
}
