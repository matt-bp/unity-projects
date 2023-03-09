using System;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseX;
    public float sensitivityHorizontal = 9.0f;
    public float sensitivityVertical = 9.0f;

    public float minVert = -45.0f;
    public float maxVert = 45.0f;

    private float verticalRot = 0;

    private void Start()
    {
        var body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
    }
    
    private void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            var speed = Input.GetAxis("Mouse X") * sensitivityHorizontal;
            transform.Rotate(0.0f, speed, 0.0f);
        }
        else
        {
            verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVertical;
            verticalRot = Math.Clamp(verticalRot, minVert, maxVert);

            var horizontalRot = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(horizontalRot, verticalRot, 0);
        }
    }
}
