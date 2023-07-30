using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using MattMath._2D;
using Prototypes._09_Implicit_MassSpring_2D.Scripts;
using Unity.Mathematics;
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
    private SimulationStatisticsProcessor processor;
    private ImplicitMassSpring cloth;
    
    // Start is called before the first frame update
    void Start()
    {
        processor = GetComponent<SimulationStatisticsProcessor>();
        cloth = GetComponentInChildren<ImplicitMassSpring>();
        
        resetSimulation.Enable();
        toggleSimulation.Enable();
    }

    // Update is called once per frame
    void Update()
    {
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

        Elapsed += Time.deltaTime;

        processor.AddStat(new RunStatistic2D()
        {
            Elapsed = Elapsed,
            Position = cloth.Positions?[0] ?? double2.zero - 101,
            DeltaTime = Time.deltaTime
        });
    }
}
