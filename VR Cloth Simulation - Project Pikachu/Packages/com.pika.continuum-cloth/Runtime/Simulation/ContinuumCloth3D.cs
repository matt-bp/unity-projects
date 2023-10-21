using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using Conditions;
using DataStructures;
using Forces;
using Geometry;
using LinearAlgebra;
using Solvers;
using Triangles;

namespace Simulation
{
    [AddComponentMenu("Cloth Simulation/3D Continuum Cloth")]
    public class ContinuumCloth3D : MonoBehaviour
    {
        [SerializeField] private bool useExplicitIntegration;
        
        #region Simulation Constants

        [SerializeField] private double2 bControl = math.double2(1, 1);
        [SerializeField] private double k = 10;
        [SerializeField] private double kd = 0.0;
        [SerializeField] private double shearK = 10;
        [SerializeField] private double shearKd = 0.0;
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
        private List<Double3> positions = new();
        private List<double3> velocities = new();
        [SerializeField] private List<int> constrainedIndices = new();

        private List<(int, int, int)> triangleIndices = new();
        [SerializeField] private List<WorldSpaceTriangle> worldSpaceTriangles = new();
        [SerializeField] private List<RestSpaceTriangle> restSpaceTriangles = new();

        public void SetTriangles(List<int> flatTriangleIndices)
        {
            Debug.Assert(positions.Count > 0);
            Debug.Assert(flatTriangleIndices.Count > 0);

            worldSpaceTriangles = new List<WorldSpaceTriangle>();
            restSpaceTriangles = new List<RestSpaceTriangle>();
            
            triangleIndices = GetTriangles(flatTriangleIndices).Select(x => (x[0], x[1], x[2])).ToList();

            foreach (var triangle in triangleIndices)
            {
                var pos0 = positions[triangle.Item1];
                var pos1 = positions[triangle.Item2];
                var pos2 = positions[triangle.Item3];

                worldSpaceTriangles.Add(new WorldSpaceTriangle(
                    positions[triangle.Item1],
                    positions[triangle.Item2],
                    positions[triangle.Item3]));

                restSpaceTriangles.Add(new RestSpaceTriangle(
                    pos0.Value.xy,
                    pos1.Value.xy,
                    pos2.Value.xy
                ));
            }
            
            // TODO: Find triangles that share an edge, and add those indices to a list (4 grouped indices in the list).
            var quads = Quad.MakeFromSharedEdges(triangleIndices);
            
            // Create triangle pairs
        }

        public void SetWorldSpacePositions(List<double3> worldSpacePositions)
        {
            if (!positions.Any())
            {
                Debug.Log("Creating a new positions array, first time");
                positions = worldSpacePositions.Select(x => new Double3(x)).ToList();
            }
            else
            {
                Debug.Log("Updating the old positions array.");
                for (var i = 0; i < positions.Count; i++)
                {
                    positions[i].Value = worldSpacePositions[i];
                }
            }

            forces = Grid<double3>.MakeVector(positions.Count, double3.zero);

            velocities = Grid<double3>.MakeVector(positions.Count, double3.zero);

            Debug.Assert(positions.Count == forces.Count);
            Debug.Assert(positions.Count == velocities.Count);
            
            Debug.Log("Setup!");
        }

        public List<double3> Positions => positions.Select(p => p.Value).ToList();
        public List<double3> Velocities => velocities.ToList();

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
            // Clear out force vector
            // Gravitational force, also resets the force vector
            foreach (var index in Enumerable.Range(0, forces.Count))
            {
                forces[index] = gravity * m;
            }

            if (useExplicitIntegration)
            {
                for (var i = 0; i < triangleIndices.Count; i++)
                {
                    var index0 = triangleIndices[i].Item1;
                    var index1 = triangleIndices[i].Item2;
                    var index2 = triangleIndices[i].Item3;

                    var v = new List<double3>()
                    {
                        velocities[index0],
                        velocities[index1],
                        velocities[index2]
                    };

                    var ct = new CombinedTriangle(restSpaceTriangles[i], worldSpaceTriangles[i]);
                    var sq = new StretchConditionQuantities(ct, bControl, v);
                    var stretchForces = new StretchConditionForceCalculator(k, kd, sq);
                    
                    // Compute forces for triangle
                    forces[index0] += stretchForces.GetForce(0);
                    forces[index1] += stretchForces.GetForce(1);
                    forces[index2] += stretchForces.GetForce(2);
                    
                    // Compute damping force
                    forces[index0] += stretchForces.GetDampingForce(0);
                    forces[index1] += stretchForces.GetDampingForce(1);
                    forces[index2] += stretchForces.GetDampingForce(2);

                    var shearQ = new ShearConditionQuantities(ct, v);
                    var shearForces = new ShearConditionForceCalculator(shearK, shearKd, shearQ);
                    
                    // Compute forces for triangle
                    forces[index0] += shearForces.GetForce(0);
                    forces[index1] += shearForces.GetForce(1);
                    forces[index2] += shearForces.GetForce(2);
                    
                    // Compute damping force
                    forces[index0] += shearForces.GetDampingForce(0);
                    forces[index1] += shearForces.GetDampingForce(1);
                    forces[index2] += shearForces.GetDampingForce(2);
                }

                // Iterate over edge sharing triangles (need to have constructed this list before)
                // Iterate over triangle pairs and compute bend forces
                
                for (var i = 0; i < forces.Count; i++)
                {
                    if (constrainedIndices.Contains(i)) continue;
                    
                    var a = forces[i] / m;
                    velocities[i] += dt * a;
                    positions[i].Value += dt * velocities[i];
                }
            }
            else
            {
                var a = Grid<double3x3>.MakeMatrix(positions.Count, double3x3.zero);
                var dfdx = Grid<double3x3>.MakeMatrix(positions.Count, double3x3.zero);

                void SetForces(IConditionForceCalculator cfc, (int, int, int) indices)
                {
                    var index0 = indices.Item1;
                    var index1 = indices.Item2;
                    var index2 = indices.Item3;
                    
                    // Compute force for triangle
                    forces[index0] += cfc.GetForce(0);
                    forces[index1] += cfc.GetForce(1);
                    forces[index2] += cfc.GetForce(2);

                    var df0 = cfc.GetForceFirstPartialDerivative(0);
                    var df1 = cfc.GetForceFirstPartialDerivative(1);
                    var df2 = cfc.GetForceFirstPartialDerivative(2);

                    a[index0][index0] -= dt * dt * df0.dx0;
                    a[index0][index1] += dt * dt * df0.dx1;
                    a[index0][index2] += dt * dt * df0.dx2;

                    a[index1][index0] += dt * dt * df1.dx0;
                    a[index1][index1] -= dt * dt * df1.dx1;
                    a[index1][index2] += dt * dt * df1.dx2;

                    a[index2][index0] += dt * dt * df2.dx0;
                    a[index2][index1] += dt * dt * df2.dx1;
                    a[index2][index2] -= dt * dt * df2.dx2;

                    // Compute damping force
                    forces[index0] += cfc.GetDampingForce(0);
                    forces[index1] += cfc.GetDampingForce(1);
                    forces[index2] += cfc.GetDampingForce(2);

                    // Compute damping force partial derivative with respect to position
                    var dd0 = cfc.GetDampingForcePartialDerivativeWrtPosition(0);
                    var dd1 = cfc.GetDampingForcePartialDerivativeWrtPosition(1);
                    var dd2 = cfc.GetDampingForcePartialDerivativeWrtPosition(2);

                    dfdx[index0][index0] -= dt * dt * dd0.dx0;
                    dfdx[index0][index1] += dt * dt * dd0.dx1;
                    dfdx[index0][index2] += dt * dt * dd0.dx2;

                    dfdx[index1][index0] += dt * dt * dd1.dx0;
                    dfdx[index1][index1] -= dt * dt * dd1.dx1;
                    dfdx[index1][index2] += dt * dt * dd1.dx2;

                    dfdx[index2][index0] += dt * dt * dd2.dx0;
                    dfdx[index2][index1] += dt * dt * dd2.dx1;
                    dfdx[index2][index2] -= dt * dt * dd2.dx2;

                    // Compute damping force partial derivative with respect to velocity
                    var dd0V = cfc.GetDampingForcePartialDerivativeWrtVelocity(0);
                    var dd1V = cfc.GetDampingForcePartialDerivativeWrtVelocity(1);
                    var dd2V = cfc.GetDampingForcePartialDerivativeWrtVelocity(2);

                    // I'm adding incrementally into the A matrix, that is why this is happening below
                    a[index0][index0] -= dt * dd0V.dv0;
                    a[index0][index1] += dt * dd0V.dv1;
                    a[index0][index2] += dt * dd0V.dv2;

                    a[index1][index0] += dt * dd1V.dv0;
                    a[index1][index1] -= dt * dd1V.dv1;
                    a[index1][index2] += dt * dd1V.dv2;

                    a[index2][index0] += dt * dd2V.dv0;
                    a[index2][index1] += dt * dd2V.dv1;
                    a[index2][index2] -= dt * dd2V.dv2;
                }
                
                // Set entries in matrix A and matrix dfdx for stretch condition, this will look different because 
                //  I'm doing it all at once, and not multiplying matrices explicitly
                for (var i = 0; i < triangleIndices.Count; i++)
                {
                    var index0 = triangleIndices[i].Item1;
                    var index1 = triangleIndices[i].Item2;
                    var index2 = triangleIndices[i].Item3;

                    var v = new List<double3>()
                    {
                        velocities[index0],
                        velocities[index1],
                        velocities[index2]
                    };

                    var combined = new CombinedTriangle(restSpaceTriangles[i], worldSpaceTriangles[i]);
                    
                    var sq = new StretchConditionQuantities(
                        combined, bControl, v);
                    var cf = new StretchConditionForceCalculator(k, kd, sq);
                    
                    SetForces(cf, triangleIndices[i]);

                    var shearQ = new ShearConditionQuantities(combined, v);
                    var scf = new ShearConditionForceCalculator(shearK, shearKd, shearQ);
                    
                    SetForces(scf, triangleIndices[i]);
                }

                // M
                foreach (var index in Enumerable.Range(0, a.Count))
                    a[index][index] += double3x3.identity * m;

                // Set Current Force Vector (f_0)
                var f0 = forces.Select(f => dt * f).ToList();

                // Set v_0, with time steps and everything
                // I don't know if this needs dt, because we're already multiplying dfdx by dt^2 above
                // var v0 = ConjugateGradient3D.Mult(velocities, dt * dt);

                // Construct b vector from parts
                var b = ConjugateGradient3D.Add(f0, ConjugateGradient3D.Mult(dfdx, velocities));

                // Make sure that the global Jacobian matrix is symmetric

                // Solve for delta V
                var solvedDvs = ConjugateGradient3D.ConstrainedSolve(a, b, 1000, 0.001, constrainedIndices);

                // Update positions and velocities from the new dvv that we just calculated.
                foreach (var (dv, index) in solvedDvs.Select((v, i) => (v, i)))
                {
                    velocities[index] += dv;
                    positions[index].Value += dt * velocities[index];
                }
            }
        }
    }
}