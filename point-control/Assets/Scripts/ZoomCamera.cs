using System;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private void Update()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        
        transform.Translate(0, 0 , scroll * speed, Space.Self);
    }
}