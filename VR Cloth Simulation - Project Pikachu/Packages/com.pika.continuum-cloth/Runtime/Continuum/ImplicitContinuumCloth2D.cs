using System.Collections.Generic;
using SimulationHelpers.Geometry;
using Unity.Mathematics;
using UnityEngine;

namespace Continuum
{
    [AddComponentMenu("Continuum Cloth/Integration/2D Implicit Continuum Cloth")]
    public class ImplicitContinuumCloth2D : MonoBehaviour
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
        
        public void SetRestMesh(IWorldSpaceMesh meshFilter)
        {
            // Make rest post out of filter
            // Need
            // [ ] Indices
            // [ ] World space triangles
            // [ ] Rest space triangles
            // Does updating an array index value that was passed into a c# object, update it's contents? Like, is it a pointer?
            
            
        }
        
        public List<double3> Positions => positions;
        
        #endregion

        public void StepSimulation(double dt)
        {

            // Set entries in matrix A and matrix dfdx for stretch condition, this will look different because 
            //  I'm doing it all at once, and not multiplying matrices explicitly
            
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