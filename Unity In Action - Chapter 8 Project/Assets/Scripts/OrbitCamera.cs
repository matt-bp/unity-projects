using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotationSpeed = 0.2f;

    private float rotationY;
    private Vector3 offset;
    
    private void Start()
    {
        rotationY = transform.eulerAngles.y;
        offset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (!Mathf.Approximately(horizontalInput, 0))
        {
            rotationY += horizontalInput * rotationSpeed;
        }
        else
        {
            rotationY += Input.GetAxis("Mouse X") * rotationSpeed * 10;
        }

        var rotation = Quaternion.Euler(0, rotationY, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);
    }
}
