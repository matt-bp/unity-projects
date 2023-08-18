using System;
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
        
        public void Update()
        {
            if (toggleSimulation.action.WasPerformedThisFrame())
            {
                isEnabled = !isEnabled;
            }

            if (!isEnabled) return;

            // Get positions from cloth poser for initial start

            // Feed them to cloth to get new positions out

            // Create a visualization for those new positions
        }
    }
}