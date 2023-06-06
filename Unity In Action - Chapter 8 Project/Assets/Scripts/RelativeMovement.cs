using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{
    // This object should move relative to this target, player -> camera
    [SerializeField] Transform target;
    
    // Update is called once per frame
    void Update()
    {
        var movement = Vector3.zero;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            var right = target.right;
            var forward = Vector3.Cross(right, Vector3.up);
            movement = (right * horizontalInput) + (forward * verticalInput);

            transform.rotation = Quaternion.LookRotation(movement);
        }
    }
}
