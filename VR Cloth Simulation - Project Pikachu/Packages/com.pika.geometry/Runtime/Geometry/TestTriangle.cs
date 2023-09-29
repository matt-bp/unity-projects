using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Geometry
{
    public class TestTriangle : WorldSpaceMesh
    {
        [SerializeField] private Vector3[] newVertices;
        [SerializeField] private int[] newTriangles;
        private Mesh mesh;

        private void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();
            
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = newVertices;
            mesh.triangles = newTriangles;
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }

        public override List<Vector3> GetPositions() => mesh.vertices.Select(v => transform.TransformPoint(v)).ToList();
        public override int[] GetTriangles() => mesh.triangles;
    }
}