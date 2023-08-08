using System.Collections.Generic;
using System.Linq;
using Helpers;
using MattMath._2D;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._09_Implicit_MassSpring_2D.Scripts
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
        
        private readonly List<double2> initialPositions = new();
        
        private void Start()
        {
            processor = GetComponent<SimulationStatisticsProcessor>();
            cloth = GetComponentInChildren<ImplicitMassSpring>();
        
            resetSimulation.Enable();
            toggleSimulation.Enable();
            
            initialPositions.Add(math.double2(1, 3));
            initialPositions.Add(math.double2(0, 0));

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
            cloth.StepSimulation(Time.deltaTime);

            elapsed += Time.deltaTime;

            processor.AddStat(new RunStatistic2D
            {
                Elapsed = elapsed,
                Position = cloth.Positions?[0] ?? double2.zero - 101,
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

                processor.AddStat(new RunStatistic2D()
                {
                    Elapsed = elapsed,
                    Position = cloth.Positions[0],
                    DeltaTime = Time.deltaTime
                });
            }

            isEnabled = false;
            
            Debug.Log("Done with one shot simulation!");
        }
    }
}
