using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Managers;
using Simulation;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private MassSpringCloth cloth;

    private bool doSimulation;
    private float elapsed;

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.SimulationResetAndStop, OnSimulationResetAndStop);
        Messenger.AddListener(GameEvent.ToggleSimulation, OnToggleSimulation);
        Messenger.AddListener(StartupEvent.ManagersStarted, OnManagersStarted);
        Messenger<int, int>.AddListener(StartupEvent.ManagersProgress, OnMangerProgress);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.SimulationResetAndStop, OnSimulationResetAndStop);
        Messenger.RemoveListener(GameEvent.ToggleSimulation, OnToggleSimulation);
        Messenger.RemoveListener(StartupEvent.ManagersStarted, OnManagersStarted);
        Messenger<int, int>.RemoveListener(StartupEvent.ManagersProgress, OnMangerProgress);
    }

    private void Update()
    {
        if (!doSimulation) return;

        elapsed += Time.deltaTime;
        
        // var updatedCommandVariable = LoadedManagers.ReferencePositionManager.GetCurrentReferencePosition(elapsed);
        //
        // if (updatedCommandVariable.HasValue)
        // {
        //     LoadedManagers.Pid.SetCommandVariable(updatedCommandVariable.Value);
        // }

        // var forces = Vector2.zero;
        //
        // var newMeasurement = new Vector2(thingToControl.transform.position.x, thingToControl.transform.position.y);
        // var error = LoadedManagers.Pid.DoUpdate(newMeasurement);
        // // Error, don't include mass, want to have it "heavier"
        // forces += (error / (Time.deltaTime * Time.deltaTime));
        //
        // // Gravity
        // forces += new Vector2(0, gravity * mass);
        //
        // var acceleration = forces / mass;
        // velocity += acceleration * Time.deltaTime;
        // position += velocity * Time.deltaTime;
        //
        // thingToControl.transform.position = new Vector3(position.x, position.y, 0);

        var temporaryExternalForces = Enumerable.Range(0, cloth.Positions.Length)
            .Select(_ => Vector3.zero).ToArray();
        cloth.Step(temporaryExternalForces);
    }

    private void OnSimulationResetAndStop()
    {
        Reset();

        doSimulation = false;
    }

    private void Reset()
    {
        Debug.Log("Resetting scene controller.");

        elapsed = 0;
        cloth.Reset();

        LoadedManagers.Pid.Reset();
    }

    private void OnToggleSimulation()
    {
        doSimulation = !doSimulation;
    }

    private void OnManagersStarted()
    {
        doSimulation = false;

        Reset();
    }

    private void OnMangerProgress(int current, int total)
    {
        Debug.Log($"Progress: {current}/{total}");
    }
}
