using System.Collections.Generic;
using System.Linq;
using Helpers;
using MattMath._1D;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._08_Implicit_MassSpring_1D.Scripts
{
    public class SimulationController : MonoBehaviour
    {
        #region Keyboard Shortcuts
    
        [SerializeField] private InputAction resetSimulation;
        [SerializeField] private InputAction toggleSimulation;
    
        #endregion
    
        /// <summary>
        /// Time, in seconds, since the simulation run started.
        /// </summary>
        private double elapsed;
        public bool isEnabled;
        private SimulationStatisticsProcessor processor;
        private ImplicitMassSpring cloth;
        
        private readonly List<double> initialPositions = new();
        
        private void Start()
        {
            processor = GetComponent<SimulationStatisticsProcessor>();
            cloth = GetComponentInChildren<ImplicitMassSpring>();
        
            resetSimulation.Enable();
            toggleSimulation.Enable();
            
            initialPositions.Add(3.0);
            initialPositions.Add(0.0);
            
            cloth.SetPositionsAndSprings(initialPositions);
        }

        void Update()
        {
            if (resetSimulation.WasPerformedThisFrame())
            {
                cloth.SetPositionsAndSprings(initialPositions);
            }
            
            if (toggleSimulation.WasPerformedThisFrame())
            {
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;

            RunSimulation();
            
            // OneShotSimulation();
        }
    
        private void RunSimulation()
        {
            if (Time.deltaTime > 0.01)
            {
                Debug.LogWarning("Skipped a time step because it was too big. Need to handle this by sub stepping simulation.");
                return;
            }
            
            cloth.StepSimulation(Time.deltaTime);

            elapsed += Time.deltaTime;

            processor.AddStat(new RunStatistic1D
            {
                Elapsed = elapsed,
                Positions = cloth.Positions.Select(v => v).ToList(),
                Velocities = cloth.Velocities.Select(v => v).ToList(),
                DeltaTime = Time.deltaTime
            });
        }
        
        /// <summary>
        /// Runs the simulation with a fixed time step. Primarily used to test simulation parameters. Immediately creates a CSV report with run statistics.
        /// </summary>
        private void OneShotSimulation()
        {
            Debug.Log("Running one shot simulation. NOT in real time!");
            
            foreach(var _ in Enumerable.Range(0, 1000))
            {
                cloth.StepSimulation(Time.deltaTime);

                elapsed += Time.deltaTime;

                processor.AddStat(new RunStatistic1D()
                {
                    Elapsed = elapsed,
                    Positions = cloth.Positions.Select(v => v).ToList(),
                    Velocities = cloth.Velocities.Select(v => v).ToList(),
                    DeltaTime = Time.deltaTime
                });
            }

            isEnabled = false;
            
            Debug.Log("Done with one shot simulation! Please now manually create the report.");
        }
    }
}
