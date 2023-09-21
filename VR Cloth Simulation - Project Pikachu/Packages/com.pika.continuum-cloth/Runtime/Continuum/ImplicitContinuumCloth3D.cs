using System.Collections.Generic;
using SimulationHelpers.Geometry;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using Conditions;
using DataStructures;
using LinearAlgebra;
using Triangles;

namespace Continuum
{
    [AddComponentMenu("Continuum Cloth/Integration/3D Implicit Continuum Cloth")]
    public class ImplicitContinuumCloth3D : MonoBehaviour
    {
        #region Simulation Constants

        [SerializeField] private double2 b = math.double2(1, 1);
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
        [SerializeField] private List<int> constrainedIndices = new();
        private List<int> indices = new();
        private List<List<int>> triangleIndices = new();

        private readonly List<WorldSpaceTriangle> worldSpaceTriangles = new();
        private readonly List<RestSpaceTriangle> restSpaceTriangles = new();

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

            foreach (var triangle in triangleIndices)
            {
                var pos0 = positions[triangle[0]];
                var pos1 = positions[triangle[1]];
                var pos2 = positions[triangle[2]];

                worldSpaceTriangles.Add(new WorldSpaceTriangle(
                    pos0,
                    pos1,
                    pos2));

                restSpaceTriangles.Add(new RestSpaceTriangle(
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
            var a = Grid<double3x3>.MakeMatrix(positions.Count, double3x3.zero);
            
            // Clear out force vector
            // Gravitational force, also resets the force vector
            foreach (var index in Enumerable.Range(0, forces.Count))
            {
                forces[index] = gravity * m;
            }

            // Set entries in matrix A and matrix dfdx for stretch condition, this will look different because 
            //  I'm doing it all at once, and not multiplying matrices explicitly
            for (var i = 0; i < triangleIndices.Count; i++)
            {
                var sq = new StretchConditionQuantities(restSpaceTriangles[i], worldSpaceTriangles[i], b);

                // // Compute force for triangle
                var force0 = -k * (sq.Dcu.dx0 * sq.Cu + sq.Dcv.dx0 * sq.Cv);
                var force1 = -k * (sq.Dcu.dx1 * sq.Cu + sq.Dcv.dx1 * sq.Cv);
                var force2 = -k * (sq.Dcu.dx2 * sq.Cu + sq.Dcv.dx2 * sq.Cv);

                // // Add it to each index in the triangle's force vector
                var index0 = triangleIndices[i][0];
                var index1 = triangleIndices[i][1];
                var index2 = triangleIndices[i][2];

                forces[index0] += force0;
                forces[index1] += force1;
                forces[index2] += force2;

                // Compute force first derivative (Jacobian), and add it to large matrix
                var df0dx0 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx0, sq.Dcu.dx0) + sq.D2CuDx0.dx0 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx0, sq.Dcv.dx0) + sq.D2CvDx0.dx0 * sq.Cv
                );
                var df0dx1 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx0, sq.Dcu.dx1) + sq.D2CuDx0.dx1 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx0, sq.Dcv.dx1) + sq.D2CvDx0.dx1 * sq.Cv
                );
                var df0dx2 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx0, sq.Dcu.dx2) + sq.D2CuDx0.dx2 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx0, sq.Dcv.dx2) + sq.D2CvDx0.dx2 * sq.Cv
                );

                var df1dx0 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx1, sq.Dcu.dx0) + sq.D2CuDx1.dx0 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx1, sq.Dcv.dx0) + sq.D2CvDx1.dx0 * sq.Cv
                );
                var df1dx1 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx1, sq.Dcu.dx1) + sq.D2CuDx1.dx1 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx1, sq.Dcv.dx1) + sq.D2CvDx1.dx1 * sq.Cv
                );
                var df1dx2 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx1, sq.Dcu.dx2) + sq.D2CuDx1.dx2 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx1, sq.Dcv.dx2) + sq.D2CvDx1.dx2 * sq.Cv
                );

                var df2dx0 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx2, sq.Dcu.dx0) + sq.D2CuDx2.dx0 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx2, sq.Dcv.dx0) + sq.D2CvDx2.dx0 * sq.Cv
                );
                var df2dx1 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx2, sq.Dcu.dx1) + sq.D2CuDx2.dx1 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx2, sq.Dcv.dx1) + sq.D2CvDx2.dx1 * sq.Cv
                );
                var df2dx2 = -k * (
                    Double3.OuterProduct(sq.Dcu.dx2, sq.Dcu.dx2) + sq.D2CuDx2.dx2 * sq.Cu +
                    Double3.OuterProduct(sq.Dcv.dx2, sq.Dcv.dx2) + sq.D2CvDx2.dx2 * sq.Cv
                );

                a[index0][index0] -= dt * dt * df0dx0;
                a[index0][index1] += dt * dt * df0dx1;
                a[index0][index2] += dt * dt * df0dx2;
                
                a[index1][index0] += dt * dt * df1dx0;
                a[index1][index1] -= dt * dt * df1dx1;
                a[index1][index2] += dt * dt * df1dx2;
                
                a[index2][index0] += dt * dt * df2dx0;
                a[index2][index1] += dt * dt * df2dx1;
                a[index2][index2] -= dt * dt * df2dx2;

                // Compute derivative of damping force (Jacobian), and add it to large matrix
            }

            // M

            // Set Current Force Vector (f_0)

            // Set v_0, with time steps and everything

            // Construct b vector from parts

            // Make sure that the global Jacobian matrix is symmetric

            // Solve for delta V

            // Update positions and velocities from the new dvv that we just calculated.

            throw new System.NotImplementedException();
        }
    }
}