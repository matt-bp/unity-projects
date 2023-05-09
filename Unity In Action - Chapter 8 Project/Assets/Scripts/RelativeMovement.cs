using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    private CharacterController _characterController;

    public float moveSpeed = 6.0f; 
    public float rotSpeed = 15.0f;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        var movement = Vector3.zero;

        var horInput = Input.GetAxis("Horizontal");
        var vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            var right = target.right;
            // The camera is facing the ground, we don't want that forward vector
            var forward = Vector3.Cross(Vector3.right, Vector3.up);
            movement = (right * horInput) + (forward * vertInput);
            movement *= moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            // Calculated a quaternion facing in the given direction
            var direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }
}
