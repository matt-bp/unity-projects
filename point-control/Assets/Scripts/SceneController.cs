using System;
using Events;
using Managers;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject thingToControl;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 position;

    [SerializeField] private Vector2 initialPosition;

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

        var updatedCommandVariable = LoadedManagers.ReferencePositionManager.GetCurrentReferencePosition(elapsed);
        
        
        if (updatedCommandVariable.HasValue)
        {
            LoadedManagers.Pid.SetCommandVariable(updatedCommandVariable.Value);
        }

        var newMeasurement = new Vector2(thingToControl.transform.position.x, thingToControl.transform.position.y);
        var error = LoadedManagers.Pid.DoUpdate(newMeasurement);
        
        var acceleration = error / (Time.deltaTime * Time.deltaTime);
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;

        thingToControl.transform.position = new Vector3(position.x, position.y, 0);
    }
    
    private void OnSimulationResetAndStop()
    {
        Reset();
        
        doSimulation = false;
    }
    
    private void Reset()
    {
        Debug.Log("Resetting scene controller.");
        position = initialPosition;
        velocity = Vector2.zero;
        elapsed = 0;
        thingToControl.transform.position = new Vector3(position.x, position.y, 0);
        
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
