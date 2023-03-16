using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public bool IsAlive { private get; set; }
    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;

    private void Start()
    {
        IsAlive = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsAlive) return;

        transform.Translate(0, 0, speed * Time.deltaTime);

        var ray = new Ray(transform.position, transform.forward);

        if (!Physics.SphereCast(ray, 0.75f, out var hit)) return;
        var hitObject = hit.transform.gameObject;

        if (hitObject.GetComponent<PlayerCharacter>())
        {
            Debug.Log("Hit a player");
            if (_fireball == null)
            {
                _fireball = Instantiate(fireballPrefab);
                // Move the fireball "forward", from the enemy's perspective, and move that to world coordinates.
                _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                _fireball.transform.rotation = transform.rotation;
            }
        }
        else if (hit.distance < obstacleRange)
        {
            var angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
    }
}