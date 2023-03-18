using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;

    private void Update()
    {
        scoreLabel.text = Time.realtimeSinceStartup.ToString();
    }

    public void OnOpenSettings()
    {
        Debug.Log("open settings");
    }

    public void OnDown()
    {
        Debug.Log("Down...");
    }
}
