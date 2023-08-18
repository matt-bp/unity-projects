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
        private ClothVisualizer visualizer;

        private void Start()
        {
            clothPoser = GetComponentInChildren<ClothPoser>();
            Debug.Assert(clothPoser is not null);
            
            visualizer = GetComponentInChildren<ClothVisualizer>();
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
            
            // OneShotSimulation();

            // Create a visualization for those new positions
            visualizer.Visualize(new());
        }
    }
}