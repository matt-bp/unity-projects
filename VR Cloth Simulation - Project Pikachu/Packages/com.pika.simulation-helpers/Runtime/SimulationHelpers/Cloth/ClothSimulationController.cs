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

        public bool isOneShotSimulation;
        [SerializeField] private int oneShotIterationCount = 1000;
        public bool isEnabled;
        [SerializeField] private FramePoser framePoser;
        private Visualizer visualizer;
        private ImplicitMassSpring2D cloth;
        /// <summary>
        /// Time, in seconds, that this simulation has been enabled.
        /// </summary>
        private float elapsed;

        private void Start()
        {
            visualizer = GetComponentInChildren<Visualizer>();
            Debug.Assert(visualizer is not null);

            cloth = GetComponentInChildren<ImplicitMassSpring2D>();
            Debug.Assert(cloth is not null);

            // Use the cloth poser to initialize the cloth simulation
            Debug.Assert(framePoser.lastPose.Count > 0);
            var positions = framePoser.lastPose.Select(v => v.xy).ToList();
            cloth.SetPositionsAndSprings(positions);
        }

        private void Update()
        {
            if (resetAndStopSimulation.action.WasPerformedThisFrame())
            {
                var positions = framePoser.lastPose.Select(v => v.xy).ToList();
                cloth.SetPositionsAndSprings(positions);
                visualizer.Clear();
                isEnabled = false;
            }
            
            if (toggleSimulation.action.WasPerformedThisFrame())
            {
                if (!isEnabled)
                {
                    var positions = framePoser.lastPose.Select(v => v.xy).ToList();
                    cloth.SetPositionsAndSprings(positions);
                }
                
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;

            if (isOneShotSimulation)
            {
                OneShotSimulation();
            }
            else
            {
                RunSimulation();
            }
        }

        private void RunSimulation()
        {
            cloth.StepSimulation(Time.deltaTime);

            elapsed += Time.deltaTime;
            
            visualizer.Visualize(cloth.Positions.Select(v => new Vector3((float)v.x, (float)v.y, 0)).ToList(), elapsed, Time.deltaTime);
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
                visualizer.Visualize(cloth.Positions.Select(v => new Vector3((float)v.x, (float)v.y, 0)).ToList(), elapsed, Time.deltaTime);
            }

            isEnabled = false;
            
            Debug.Log("Done with one shot simulation!");
        }
    }
}