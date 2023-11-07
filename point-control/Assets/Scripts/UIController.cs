using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject xValueVisualizer;
    [SerializeField] private GameObject yValueVisualizer;
    [SerializeField] private Button toggleButton;

    public void OnSliderChange(float value)
    {
        xValueVisualizer.transform.position = new Vector3(value, 0, 0);
        
        LoadedManagers.Pid.SetCommandVariable(value);
    }
    
    public void OnResetClick()
    {
        Messenger.Broadcast(GameEvent.SimulationResetAndStop);
    }

    public void OnToggleClick()
    {
        ChangeToggleColor();
    }
    
    private void ChangeToggleColor()
    {
        var newColors = toggleButton.colors;

        if (newColors.normalColor.r > 0.5)
        {
            newColors.normalColor = Color.HSVToRGB(0.375f, 0.88f, 0.77f);
            newColors.selectedColor = Color.HSVToRGB(0.375f, 0.88f, 1);
            newColors.pressedColor = Color.HSVToRGB(0.375f, 0.88f, 0.5f);
        }
        else
        {
            newColors.normalColor = Color.HSVToRGB(0, 0.88f, 0.77f);
            newColors.selectedColor = Color.HSVToRGB(0, 0.88f, 1);
            newColors.pressedColor = Color.HSVToRGB(0, 0.88f, 0.5f);
        }

        toggleButton.colors = newColors;
    }
}
