using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private GameObject hitLocationIndicator;
    private GameObject hitLocationInstance;
    
    private void Start()
    {
        hitLocationInstance = Instantiate(hitLocationIndicator, Vector3.zero,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var mouseHit, 1000, LayerMask.GetMask("MovableCloth")))
        {
            var hitObject = mouseHit.transform.gameObject;

            Debug.Log($"Mouse is over: {hitObject.name}");

            var worldSpacePosition = mouseHit.point;
            
            hitLocationInstance.transform.position = worldSpacePosition;
            hitLocationInstance.transform.localScale = new Vector3(size, size, size);

            // var localSpacePosition = hitObject.transform.InverseTransformPoint(worldSpacePosition);
            
            // From local space point, get all vertices withing size away from this point.

            var vertices = hitObject.GetComponent<MeshFilter>().sharedMesh.vertices;

            var count = 0;
            
            foreach (var v in vertices)
            {
                var worldPos = hitObject.transform.TransformPoint(v);
                if (Vector3.Distance(worldPos, worldSpacePosition) <= size)
                {
                    count++;
                }
            }

            Debug.Log($"Found {count} vertices");
        }
    }
}
