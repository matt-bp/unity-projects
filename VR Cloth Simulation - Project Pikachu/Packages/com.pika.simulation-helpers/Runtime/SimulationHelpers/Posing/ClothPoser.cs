using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimulationHelpers.Posing
{
    public class ClothPoser : MonoBehaviour
    {
        public List<Vector3> lastPose = new();

        public void StartNewPose(int particleCount)
        {
            lastPose = Enumerable.Range(0, particleCount).Select(_ => Vector3.zero).ToList();
        }
        
        /// <summary>
        /// Updates the posed particle to the specified world position.
        /// </summary>
        /// <param name="index">Which particle to move.</param>
        /// <param name="worldPosition">Position, in world space, to move the particle to.</param>
        public void UpdatePose(int index, Vector3 worldPosition)
        {
            Debug.Assert(index >= 0 && index < lastPose.Count && lastPose.Count > 0);

            lastPose[index] = worldPosition;
            
            // ResetToLastPose();
        }
        
        // public void ResetToLastPose()
        // {
        //     _positions = lastPose.ToArray();
        //     UpdateMesh();
        // }
    }
}