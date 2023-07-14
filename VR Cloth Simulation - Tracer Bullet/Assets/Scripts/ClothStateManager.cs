using System.Collections;
using System.Collections.Generic;
using Simulation;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClothStateManager : MonoBehaviour
{

    #region Editor Fields
    
    [SerializeField] public MassSpringCloth cloth;
    public InputActionReference toggleSimulationEnabled;
    public InputActionReference resetSimulation;
    
    #endregion
    
    private void Update()
    {
        var toggle = toggleSimulationEnabled.action;
        if (toggle.WasPressedThisFrame() && toggle.IsPressed())
        {
            cloth.IsSimulationEnabled = !cloth.IsSimulationEnabled;
        }

        var reset = resetSimulation.action;
        if (reset.WasPressedThisFrame() && reset.IsPressed() && !cloth.IsSimulationEnabled)
        {
            cloth.ResetToLastPose();
        }
    }
}
