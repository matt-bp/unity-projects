using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject xValueVisualizer;
    [SerializeField] private GameObject yValueVisualizer;
    
    public void OnSliderChange(float value)
    {
        xValueVisualizer.transform.position = new Vector3(value, 0, 0);
        
        LoadedManagers.Pid.SetCommandVariable(value);
    }
}
