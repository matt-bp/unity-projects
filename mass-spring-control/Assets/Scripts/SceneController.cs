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


        List<Vector3> externalForces = new();

        var updatedCommandVariable = LoadedManagers.ReferencePositionManager.GetCurrentReferencePosition(elapsed);
        
        if (updatedCommandVariable is not null)
        {
            LoadedManagers.Pid.SetCommandVariable(updatedCommandVariable);
            
            var error = LoadedManagers.Pid.DoUpdate(cloth.Positions);
            // Error, don't include mass, want to have it "heavier"
            externalForces = error.Select(e => e / (Time.deltaTime * Time.deltaTime)).ToList();
        }
        
        cloth.Step(externalForces.ToArray());
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
