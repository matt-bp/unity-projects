using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimulationHelpers.Input
{
    [AddComponentMenu("Simulation Helpers/Input/Button Press Logger")]
    public class ButtonPressLogger : MonoBehaviour
    {
        public InputActionReference button;

        private void Update()
        {
            var buttonAction = button.action;
            
            Debug.Log($"Triggered: {buttonAction.triggered}");
        }
    }
}