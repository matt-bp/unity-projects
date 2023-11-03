using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    public bool requireKey;

    private void OnTriggerEnter(Collider other)
    {
        if (requireKey && Managers.Inventory.EquippedItem != "Key")
        {
            return;
        }
        
        foreach (var target in targets)
        {
            target.SendMessage("Activate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var target in targets)
        {
            target.SendMessage("Deactivate");
        }
    }
}
