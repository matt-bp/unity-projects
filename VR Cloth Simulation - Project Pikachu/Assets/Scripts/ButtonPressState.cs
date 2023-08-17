using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPressState : MonoBehaviour
{
    public InputActionReference buttonToPress;
    
    private float prevValue { get; set; }

    // Update is called once per frame
    void Update()
    {
        var buttonAction = buttonToPress.action;

        // var currentValue = buttonAction.ReadValue<float>();
        Debug.Log($"triggered: {buttonAction.triggered}");
        
        // if (currentValue != prevValue)
        // {
        //     prevValue = currentValue;
        //     Debug.Log($"Val: {currentValue}, triggered: {buttonAction.triggered}");
        // }
    }
}
