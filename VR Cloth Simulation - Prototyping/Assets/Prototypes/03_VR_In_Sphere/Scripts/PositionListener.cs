using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PositionListener : MonoBehaviour
{
    public InputActionProperty leftController;
    
    // Start is called before the first frame update
    void Start()
    {
        var action = leftController.action;
        
        Debug.Log(action.ReadValue<Vector3>());
    }

    // Update is called once per frame
    void Update()
    {
        var action = leftController.action;
        
        // We are now getting the position of the VR controller!
        Debug.Log("New:" + action.ReadValue<Vector3>());
    }
}
