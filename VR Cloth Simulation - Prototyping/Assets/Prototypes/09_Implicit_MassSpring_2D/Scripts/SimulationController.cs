using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimulationController : MonoBehaviour
{
    
    #region Keyboard Shortcuts
    
    [SerializeField] private InputAction resetSimulation;
    [SerializeField] private InputAction toggleSimulation;
    
    #endregion
    
    /// <summary>
    /// Time, in seconds, since the simulation run started.
    /// </summary>
    private double Elapsed;
    public bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
