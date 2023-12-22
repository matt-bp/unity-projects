using System;
using UnityEngine;

[RequireComponent(typeof(SimulationManager))]
public class LoadedManagers : MonoBehaviour
{
    public static SimulationManager SimulationManager { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SimulationManager = GetComponent<SimulationManager>();
    }
}