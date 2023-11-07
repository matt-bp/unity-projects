using Events;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject xValueVisualizer;
    [SerializeField] private GameObject yValueVisualizer;
    [SerializeField] private Button toggleButton;

    private bool showGreen;
    
    public void OnSliderChange(float value)
    {
        xValueVisualizer.transform.position = new Vector3(value, 0, 0);
        
        LoadedManagers.Pid.SetCommandVariable(value);
    }
    
    public void OnResetClick()
    {
        showGreen = false;
        ChangeToggleText();
        ChangeToggleColor();
        
        Messenger.Broadcast(GameEvent.SimulationResetAndStop);
    }

    public void OnToggleClick()
    {
        showGreen = !showGreen;
        
        ChangeToggleText();
        ChangeToggleColor();

        Messenger.Broadcast(GameEvent.ToggleSimulation);
    }

    private void ChangeToggleText()
    {
        var text = toggleButton.GetComponentInChildren<TMP_Text>();
        if (text is not null)
        {
            text.text = "Toggle " + (showGreen ? "Off" : "On");
        }
    }
    
    private void ChangeToggleColor()
    {
        var newColors = toggleButton.colors;

        if (showGreen)
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
