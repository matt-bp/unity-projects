using Simulation;
using UnityEngine;


/// <summary>
/// Facilitates miscellaneous controls to the cloth simulation, like toggling if it is enabled.
/// </summary>
public class VRButtonController : MonoBehaviour
{
    #region Editor Fields
    
    [SerializeField] public MassSpringCloth simulatedObject;
    
    #endregion
    
    public void OnPrimaryButtonPressed()
    {
        Debug.Log("Toggling simulated object");

        simulatedObject.IsSimulationEnabled = !simulatedObject.IsSimulationEnabled;
    }
}
