using System.Collections.Generic;
using System.Linq;
using MassSpring.Integration;
using SimulationHelpers.Posing;
using SimulationHelpers.Visualization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimulationHelpers.Cloth
{
    [AddComponentMenu("Simulation Helpers/Cloth/Cloth Simulation Controller")]
    public class ClothSimulationController : MonoBehaviour
    {
        #region Keyboard Shortcuts
    
        [SerializeField] private InputActionReference resetSimulation;
        [SerializeField] private InputActionReference toggleSimulation;
    
        #endregion
        
        public bool isEnabled;
        private ClothPoser clothPoser;
        private Visualizer visualizer;
        private ImplicitMassSpring3D cloth;
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
            
            // Use the cloth poser to initialize the cloth simulation
        }

        private void Update()
        {
            if (resetSimulation.action.WasPerformedThisFrame())
            {
                // Use the clothPoser as "initial positions" to set the cloth sim to
                // cloth.SetPositionsAndSprings(initialPositions);
            }
            
            if (toggleSimulation.action.WasPerformedThisFrame())
            {
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;
            
            // RunSimulation();
            
            OneShotSimulation();
        }
        
        /// <summary>
        /// Runs the simulation with a fixed time step. Primarily used to test simulation parameters. Immediately creates a CSV report with run statistics.
        /// </summary>
        private void OneShotSimulation()
        {
            Debug.Log("Running one shot simulation. NOT in real time!");
            
            foreach(var _ in Enumerable.Range(0, 1000))
            {
                // cloth.StepSimulation(Time.deltaTime);

                elapsed += Time.deltaTime;
                
                // Create a visualization for those new positions
                visualizer.Visualize(new List<Vector3>(new []
                {
                    new Vector3(),
                    new Vector3(),
                    new Vector3()
                }), elapsed, Time.deltaTime);
            }

            isEnabled = false;
            
            Debug.Log("Done with one shot simulation!");
        }
    }
}