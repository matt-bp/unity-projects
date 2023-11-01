using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour
{
    public float radius = 1.5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var hitColliders = Physics.OverlapSphere(transform.position, radius);

            foreach (var collider in hitColliders)
            {
                collider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
