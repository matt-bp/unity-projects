using System;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class BouncingBall : MonoBehaviour
{
    [SerializeField] private float mass = 1;
    [SerializeField] private Vector2 velocity = Vector2.zero;
    [SerializeField] private Vector2 position = Vector2.zero;
    
    private Vector2 Gravity = new Vector2(0, -10);

    private void Update()
    {
        if (Application.IsPlaying(gameObject))
        {
            if (position.y <= 0)
            {
                velocity.y *= -1;
            }

            // Gravity acts the same on objects of different mass.
            var forces = Gravity * mass;

            var acceleration = forces / mass;
            velocity += acceleration * Time.deltaTime;
            position += velocity * Time.deltaTime;
        
            gameObject.transform.position = new Vector3(position.x, position.y, gameObject.transform.position.z);
        }
        else
        {
            position = gameObject.transform.position;
        }
    }
}