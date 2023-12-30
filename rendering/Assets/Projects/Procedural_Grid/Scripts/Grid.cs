using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    public int xSize, ySize;
    private Vector3[] vertices;
    private Mesh mesh;
    
    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        var wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (var x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
        
        mesh.vertices = vertices;

        var triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = xSize + 1;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = xSize + 1;
        triangles[5] = xSize + 2;

        mesh.triangles = triangles; // has to come after setting vertices
    }

    private void OnDrawGizmos()
    {
        if (vertices is null)
        {
            return;
        }
        
        Gizmos.color = Color.black;
        for (var i = 0; i < vertices.Length; i++)
        {
            // Drawing in world space
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
