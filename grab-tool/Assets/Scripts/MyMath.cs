using System.Linq;
using UnityEngine;

public static class MyMath
{
    public static bool Raycast(Ray ray, MeshFilter meshFilter, out RaycastHit hit)
    {
        hit = new RaycastHit();

        var sourceTriangles = meshFilter.sharedMesh.triangles;

        var triangles = sourceTriangles
            .Select((v, i) => new { v, i })
            .GroupBy(x => x.i / 3)
            .Select(g => g.Select(x => x.v).ToList())
            .ToList();

        foreach (var triangle in triangles)
        {
            Debug.Assert(triangle.Count == 3);
            
            // Intersect ray with triangle
        }
        
        return false;
    }
}