using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimulationHelpers.Geometry
{
    public interface IWorldSpaceMesh
    {
        public List<Vector3> GetPositions();
        public List<int> GetIndices();
    }
    
    /// <summary>
    /// Used to use a mesh filter's vertices in world coordinate space.
    ///
    /// This computes the world position once, which can be used by other components at will.
    /// </summary>
    public class WorldSpaceMesh : MonoBehaviour, IWorldSpaceMesh
    {
        private List<Vector3> positions;
        private List<int> indices;

        private void Awake()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            positions = mesh.vertices.Select(v => transform.TransformPoint(v)).ToList();
            Debug.Assert(positions.Count > 0);

            indices = mesh.triangles.ToList();
        }

        public List<Vector3> GetPositions() => positions;
        /// <summary>
        /// I think this returns 3 numbers representing which positions create a triangle
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<int> GetIndices()
        {
            throw new System.NotImplementedException();
        }
    }
}