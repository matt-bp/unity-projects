using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    
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

            // Calculated a quaternion facing in the given direction
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }
}
