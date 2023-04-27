using Simulation;
using UnityEngine;


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

    public void OnSecondaryButtonPressed()
    {
        Debug.Log("Pressed secondary button");
    }
}
