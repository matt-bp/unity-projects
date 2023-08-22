using System;
using System.Collections.Generic;
using System.Linq;
using MassSpring.Integration;
using SimulationHelpers.Posing;
using SimulationHelpers.Visualization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimulationHelpers.Cloth
{
    [AddComponentMenu("Simulation Helpers/Cloth/Cloth Simulation Controller")]
    public class ClothSimulationController : MonoBehaviour
    {
        #region Keyboard Shortcuts
    
        [SerializeField] private InputActionReference resetAndStopSimulation;
        [SerializeField] private InputActionReference toggleSimulation;
    
        #endregion

        [SerializeField] private int oneShotIterationCount = 1000;
        public bool isEnabled;
        private ClothPoser clothPoser;
        private Visualizer visualizer;
        private ImplicitMassSpring1D cloth;
        /// <summary>
        /// Time, in seconds, that this simulation has been enabled.
        /// </summary>
        private float elapsed;

        private void Start()
        {
            clothPoser = GetComponentInChildren<ClothPoser>();
            Debug.Assert(clothPoser is not null);
            
            visualizer = GetComponentInChildren<Visualizer>();
            Debug.Assert(visualizer is not null);

            cloth = GetComponentInChildren<ImplicitMassSpring1D>();
            Debug.Assert(cloth is not null);

            // Use the cloth poser to initialize the cloth simulation
            Debug.Assert(clothPoser.lastPose.Count > 0);
            var positions = clothPoser.lastPose.Select(v => v.y).ToList();
            var testPositions = new List<double> { positions[0], positions[2] };
            cloth.SetPositionsAndSprings(testPositions);
        }

        private void Update()
        {
            if (resetAndStopSimulation.action.WasPerformedThisFrame())
            {
                var positions = clothPoser.lastPose.Select(v => v.y).ToList();
                var testPositions = new List<double> { positions[0], positions[2] };
                cloth.SetPositionsAndSprings(testPositions);
                visualizer.Clear();
                isEnabled = false;
            }
            
            if (toggleSimulation.action.WasPerformedThisFrame())
            {
                if (!isEnabled)
                {
                    var positions = clothPoser.lastPose.Select(v => v.y).ToList();
                    var testPositions = new List<double> { positions[0], positions[2] };
                    cloth.SetPositionsAndSprings(testPositions);
                }
                
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;
            
            RunSimulation();
            
            // OneShotSimulation();
        }

        private void RunSimulation()
        {
            cloth.StepSimulation(Time.deltaTime);

            elapsed += Time.deltaTime;
            
            visualizer.Visualize(cloth.Positions.Select(v => new Vector3(0, (float)v, 0)).ToList(), elapsed, Time.deltaTime);
        }
        
        
        /// <summary>
        /// Runs the simulation with a fixed time step. Primarily used to test simulation parameters. Immediately creates a CSV report with run statistics.
        /// </summary>
        private void OneShotSimulation()
        {
            Debug.Log("Running one shot simulation. NOT in real time!");
            
            foreach(var _ in Enumerable.Range(0, oneShotIterationCount))
            {
                cloth.StepSimulation(Time.deltaTime);

                elapsed += Time.deltaTime;
                
                // Create a visualization for those new positions
                visualizer.Visualize(cloth.Positions.Select(v => new Vector3(0, (float)v, 0)).ToList(), elapsed, Time.deltaTime);
            }

            isEnabled = false;
            
            Debug.Log("Done with one shot simulation!");
        }
    }
}