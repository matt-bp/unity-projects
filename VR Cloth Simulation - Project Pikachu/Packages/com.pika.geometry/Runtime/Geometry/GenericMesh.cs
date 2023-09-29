using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Geometry
{
    public class GenericMesh : WorldSpaceMesh
    {
        private List<Vector3> positions;
        private int[] indices;

        private void Awake()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            positions = mesh.vertices.Select(v => transform.TransformPoint(v)).ToList();
            Debug.Assert(positions.Count > 0);

            indices = mesh.triangles;
        }

        public override List<Vector3> GetPositions() => positions;
        public override int[] GetTriangles() => indices;
    }
}