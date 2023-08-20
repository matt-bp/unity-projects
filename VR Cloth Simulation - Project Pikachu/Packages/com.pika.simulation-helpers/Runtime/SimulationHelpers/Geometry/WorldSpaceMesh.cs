using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimulationHelpers.Geometry
{
    /// <summary>
    /// Used to use a mesh filter's vertices in world coordinate space.
    ///
    /// This computes the world position once, which can be used by other components at will.
    /// </summary>
    public class WorldSpaceMesh : MonoBehaviour
    {
        public List<Vector3> positions;

        private void Awake()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            positions = mesh.vertices.Select(v => transform.TransformPoint(v)).ToList();
            Debug.Assert(positions.Count > 0);
        }
    }
}