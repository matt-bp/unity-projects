using System.Collections.Generic;
using SimulationHelpers.Geometry;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using Triangles;

namespace Continuum
{
    [AddComponentMenu("Continuum Cloth/Integration/3D Implicit Continuum Cloth")]
    public class ImplicitContinuumCloth3D : MonoBehaviour
    {
        #region Simulation Constants
        
        [SerializeField] private double k = 10;
        [SerializeField] private double kd = 0.0;
        [SerializeField] private double3 gravity = math.double3(0.0, -10.0, 0.0);
        /// <summary>
        /// This is the mass in kg for each particle. This will eventually be replaced by specifying the weight for the
        /// whole cloth, and then evenly distributing that across all particles.
        /// </summary>
        [SerializeField] private double m = 0.1;
        
        #endregion

        #region Simulation State
        
        private GameObject restMesh;
        private List<double3> forces = new();
        private List<double3> positions = new();
        private List<double3> velocities = new();
        private List<double> masses = new();
        [SerializeField] private List<int> constrainedIndices = new();
        private List<int> indices = new();
        private List<List<int>> triangleIndices = new();

        private List<WorldSpaceTriangle> WorldSpaceTriangles = new();
        private List<RestSpaceTriangle> RestSpaceTriangles = new();
        
        public void SetRestMesh(IWorldSpaceMesh meshFilter)
        {
            // Make rest post out of filter
            // Need to create
            // [x] Indices
            // [x] World space triangles
            // [x] Rest space triangles
            // Does updating an array index value that was passed into a c# object, update it's contents? Like, is it a pointer?
            indices = meshFilter.GetIndices();

            triangleIndices = GetTriangles(indices).Select(x => x).ToList();
            
            foreach(var triangle in triangleIndices)
            {
                var pos0 = positions[triangle[0]];
                var pos1 = positions[triangle[1]];
                var pos2 = positions[triangle[2]];
                
                WorldSpaceTriangles.Add(new WorldSpaceTriangle(
                    pos0, 
                    pos1, 
                    pos2));
                
                RestSpaceTriangles.Add(new RestSpaceTriangle(
                    pos0.xy,
                    pos1.xy,
                    pos2.xy
                    ));
            }
        }
        
        public List<double3> Positions => positions;

        public static IEnumerable<List<int>> GetTriangles(List<int> indicesList)
        {
            for (var i = 0; i < indicesList.Count; i += 3)
            {
                yield return indicesList.Skip(i).Take(3).ToList();
            }
        }
        
        #endregion

        public void StepSimulation(double dt)
        {

            // Set entries in matrix A and matrix dfdx for stretch condition, this will look different because 
            //  I'm doing it all at once, and not multiplying matrices explicitly
            foreach (var triangle in triangleIndices)
            {
                
            }
            
            
            // Add in damping force for stretch condition as well
            
            // M
            
            // Set Forces
            // Set Current Force Vector (f_0)
            
            // Set v_0, with time steps and everything
            
            // Construct b vector from parts
            
            // Solve for delta V
            
            // Update positions and velocities from the new dvv that we just calculated.

            throw new System.NotImplementedException();
        }
        
        
        
    }
}