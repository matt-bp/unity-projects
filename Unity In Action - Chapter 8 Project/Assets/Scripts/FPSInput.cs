using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    private CharacterController _characterController;
    
    public float speed = 3;
    public float gravity = -9.8f;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        var deltaX = Input.GetAxis("Horizontal") * speed;
        var deltaZ = Input.GetAxis("Vertical") * speed;
        var movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        movement *= Time.deltaTime;
        // Moving from local to global coordinate space
        movement = transform.TransformDirection(movement);
        _characterController.Move(movement);
    }
}
