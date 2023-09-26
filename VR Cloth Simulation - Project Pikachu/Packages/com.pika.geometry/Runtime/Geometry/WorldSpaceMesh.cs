using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Geometry
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
    public abstract class WorldSpaceMesh : MonoBehaviour
    {
        public abstract List<Vector3> GetPositions();
        // public abstract List<int> GetIndices();
    }
}