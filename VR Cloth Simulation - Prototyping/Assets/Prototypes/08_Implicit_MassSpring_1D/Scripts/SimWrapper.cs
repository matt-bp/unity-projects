using System;
using System.Collections.Generic;
using Helpers;
using MattMath;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._08_Implicit_MassSpring_1D.Scripts
{
    public class SimWrapper : MonoBehaviour
    {
        private readonly ImplicitMassSpring1D system = new();
        private List<double> initialPositions;
        [SerializeField] private GameObject massPrefab;
        private readonly List<GameObject> createdPrefabs = new();
        
        private List<RunStatistics1D> runStatistics = new();
        /// <summary>
        /// Time, in seconds, since the simulation run started.
        /// </summary>
        private double Elapsed;
        [SerializeField] private InputAction createReport;
        [SerializeField] private InputAction resetSimulation;
        [SerializeField] private InputAction toggleSimulation;
        public bool isEnabled;
    
        private void Start()
        {
            initialPositions = system.positions;
            
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
                for (var i = 0; i < initialPositions.Count; i++)
                {
                    system.positions[i] = initialPositions[i];
                }
            }

            if (!isEnabled) return;

            RunSimulation();
        }

        private void CreateReport()
        {
            var filename = "./Stats/stats.csv";
            Debug.Log("Creating report here: " + filename);
            StatsWriter.WriteRunStatistics(runStatistics, filename);
        }

        private void RunSimulation()
        {
            system.Update(Time.deltaTime);

            for (var i = 0; i < system.positions.Count; i++)
            {
                createdPrefabs[i].transform.position = new Vector3(0, (float)system.positions[i], 0);
            }

            Elapsed += Time.deltaTime;

            runStatistics.Add(new RunStatistics1D()
            {
                Elapsed = Elapsed,
                Position = system.positions[0],
            });
        }
    }
}
