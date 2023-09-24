using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace SimulationHelpers.Posing
{
    public class FramePoser : MonoBehaviour
    {
        public List<double3> lastPose = new();
        [SerializeField] private MeshFilter meshFilter; 

        public void StartNewPose(int particleCount)
        {
            lastPose = Enumerable.Range(0, particleCount).Select(_ => double3.zero).ToList();
        }
        
        /// <summary>
        /// Updates the posed particle to the specified world position.
        /// </summary>
        /// <param name="index">Which particle to move.</param>
        /// <param name="worldPosition">Position, in world space, to move the particle to.</param>
        public void UpdatePose(int index, Vector3 worldPosition)
        {
            Debug.Assert(index >= 0 && index < lastPose.Count && lastPose.Count > 0);

            lastPose[index] = math.double3(worldPosition);

            UpdateFramePose();
        }

        public List<int> GetTriangleIndices() => meshFilter.mesh.triangles.ToList();

        private void UpdateFramePose()
        {
            meshFilter.mesh.vertices = lastPose.Select(v => meshFilter.gameObject.transform.InverseTransformPoint(new Vector3((float)v.x, (float)v.y, (float)v.z))).ToArray();
            meshFilter.mesh.RecalculateBounds();
        }
    }
}