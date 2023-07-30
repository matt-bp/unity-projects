using System.Collections.Generic;
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
        private readonly List<(int, int)> springs = new();
        
        private void Start()
        {
            Debug.Log("Banana1");
            processor = GetComponent<SimulationStatisticsProcessor>();
            cloth = GetComponentInChildren<ImplicitMassSpring>();
        
            resetSimulation.Enable();
            toggleSimulation.Enable();
            Debug.Log("Banana2");
            initialPositions.Add(math.double2(0, 3));
            initialPositions.Add(math.double2(0, 0));
        
            springs.Add((0, 1));
            Debug.Log("Banana3");
            cloth.SetPositionsAndSprings(initialPositions, springs);
        }

        void Update()
        {
            if (resetSimulation.WasPerformedThisFrame())
            {
                cloth.SetPositionsAndSprings(initialPositions, springs);
            }
            
            if (toggleSimulation.WasPerformedThisFrame())
            {
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;

            RunSimulation();
        }
    
        private void RunSimulation()
        {
            //cloth.StepSimulation(Time.deltaTime);

            elapsed += Time.deltaTime;

            processor.AddStat(new RunStatistic2D
            {
                Elapsed = elapsed,
                Position = cloth.Positions?[0] ?? double2.zero - 101,
                DeltaTime = Time.deltaTime
            });
        }
    }
}
