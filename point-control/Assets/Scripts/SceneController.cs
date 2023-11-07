using System;
using Events;
using Managers;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject thingToControl;
    [SerializeField] private float xVelocity;
    [SerializeField] private float xPosition;

    [SerializeField] private float initialXPosition;

    private bool doSimulation;

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.SimulationResetAndStop, OnSimulationResetAndStop);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.SimulationResetAndStop, OnSimulationResetAndStop);
    }

    private void Update()
    {
        if (!doSimulation) return;
        
        var error = LoadedManagers.Pid.DoUpdate(thingToControl.transform.position.x);
        
        var acceleration = error / (Time.deltaTime * Time.deltaTime);
        xVelocity += acceleration * Time.deltaTime;
        xPosition += xVelocity * Time.deltaTime;

        thingToControl.transform.position = new Vector3(xPosition, 0, 0);
    }
    
    private void OnSimulationResetAndStop()
    {
        Reset();
        
        doSimulation = false;
    }
    
    private void Reset()
    {
        Debug.Log("Resetting scene controller.");
        xPosition = initialXPosition;
        xVelocity = 0;
        thingToControl.transform.position = new Vector3(xPosition, 0, 0);
        
        LoadedManagers.Pid.Reset();
    }
}