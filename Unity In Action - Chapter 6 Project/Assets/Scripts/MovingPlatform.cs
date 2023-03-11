using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 finishPosition = Vector3.zero;
    public float speed = 0.5f;

    private Vector3 startingPosition;
    private float trackPercent = 0;
    private int direction = 1;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        trackPercent += direction * speed * Time.deltaTime;
        var x = (finishPosition.x - startingPosition.x) * trackPercent + startingPosition.x;
        var y = (finishPosition.y - startingPosition.y) * trackPercent + startingPosition.y;
        transform.position = new Vector3(x, y, startingPosition.z);

        if ((direction == 1 && trackPercent > 0.9f) || (direction == -1 && trackPercent < 0.1f))
        {
            direction *= -1;  
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPosition);
    }
}

