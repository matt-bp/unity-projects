using System;
using Managers;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject thingToControl;
    [SerializeField] private float xVelocity;
    [SerializeField] private float xPosition;

    [SerializeField] private float initialXPosition;
    
    private void Update()
    {
        var error = LoadedManagers.Pid.DoUpdate(thingToControl.transform.position.x);
        
        var acceleration = error / (Time.deltaTime * Time.deltaTime);
        xVelocity += acceleration * Time.deltaTime;
        xPosition += xVelocity * Time.deltaTime;

        thingToControl.transform.position = new Vector3(xPosition, 0, 0);
    }

    public void Reset()
    {
        xPosition = initialXPosition;
        xVelocity = 0;
    }
}