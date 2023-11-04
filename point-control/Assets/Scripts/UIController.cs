using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void OnSliderChange(float value)
    {
        Debug.Log($"Got value {value}");
    }
}
