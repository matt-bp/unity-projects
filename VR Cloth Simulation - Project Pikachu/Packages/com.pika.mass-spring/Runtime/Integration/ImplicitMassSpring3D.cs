using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LinearAlgebra;
using Solvers;
using Unity.Mathematics;
using UnityEngine;

namespace Integration
{
    public class ImplicitMassSpring3D : MonoBehaviour
    {
        #region Simulation Constants

        [SerializeField] private double k = 10;
        [SerializeField] private double kd = 0.0;
        [SerializeField] private double l = 0.1;
        [SerializeField] private double3 gravity = math.double3(0.0, -10.0, 0.0);
        /// <summary>
        /// This is the mass in kg for each particle. This will eventually be replaced by specifying the weight for the
        /// whole cloth, and then evenly distributing that across all particles.
        /// </summary>
        [SerializeField] private double m = 0.1;

        #endregion
        
        #region Simulation State

        private List<double3> forces = new();
        private List<double3> positions = new();
        private List<double3> velocities = new();
        private List<double> masses = new();
        [SerializeField] private List<int> constrainedIndices = new();
        [SerializeField] private List<ParticlePair> springs = new();

        public List<double3> Positions => positions;

        public void SetPositionsAndSprings(List<double3> newPositions)
        {
            forces = Grid<double3>.MakeVector(newPositions.Count, double3.zero);
            positions = newPositions.Select(x => x).ToList();
            velocities = Grid<double3>.MakeVector(newPositions.Count, double3.zero);
            masses = Grid<double>.MakeVector(newPositions.Count, m);
            
            Debug.Assert(positions.Count == forces.Count);
            Debug.Assert(positions.Count == velocities.Count);
            Debug.Assert(positions.Count == masses.Count);
            Debug.Assert(springs.All(pair =>
                pair.firstIndex >= 0 && pair.firstIndex < positions.Count && pair.secondIndex >= 0 &&
                pair.secondIndex < positions.Count));
            Debug.Assert(springs.All(pair => pair.firstIndex != pair.secondIndex));
        }

        #endregion

        public void StepSimulation(double dt)
        {
            SetForces();
            
            var a = MakeEmptyGridMatrix();
            var dfdx = MakeEmptyGridMatrix();
            
            foreach (var spring in springs)
            {
                var firstIndex = spring.firstIndex;
                var secondIndex = spring.secondIndex;
                
                var jp = dt * dt * SpringJdx(firstIndex, secondIndex);
                var jv = dt * SpringJdv();
                
                a[firstIndex][firstIndex] -= jp - jv;
                a[firstIndex][secondIndex] += jp + jv;
                a[secondIndex][secondIndex] -= jp - jv;
                a[secondIndex][firstIndex] += jp + jv;

                dfdx[firstIndex][firstIndex] -= jp;
                dfdx[firstIndex][secondIndex] += jp;
                dfdx[secondIndex][secondIndex] -= jp;
                dfdx[secondIndex][firstIndex] += jp;
            }
            
            // M
            foreach (var index in Enumerable.Range(0, a.Count))
                a[index][index] += double3x3.identity * masses[index];
            
            // Populate forces vector
            var f = MakeEmptyGridVector();
            foreach (var index in Enumerable.Range(0, f.Count))
                f[index] = dt * forces[index];
            
            var newVelocities = ConjugateGradient3D.Mult(velocities, dt * dt);

            var b = ConjugateGradient3D.Add(f, ConjugateGradient3D.Mult(dfdx, newVelocities));
            
            var dvs = ConjugateGradient3D.ConstrainedSolve(a, b, 1000, 0.001, constrainedIndices);

            foreach (var (dv, index) in dvs.Select((v, i) => (v, i)))
            {
                positions[index] += dt * (velocities[index] + dv);
                velocities[index] += dt * dv;
            }
        }
        
        private void SetForces()
        {
            // Gravitational force, also resets the force vector
            foreach (var index in Enumerable.Range(0, forces.Count))
            {
                forces[index] = gravity * masses[index];
            }
            
            // Spring force, should be added onto force vector
            foreach (var spring in springs)
            {
                var springForce = GetSpringForce(positions[spring.firstIndex], positions[spring.secondIndex]);
                forces[spring.firstIndex] += springForce;
                forces[spring.secondIndex] -= springForce;
            }
        }
        
        private double3 GetSpringForce(double3 position1, double3 position2)
        {
            var vectorBetween = position1 - position2;
            var distance = math.distance(position1, position2);
            var force = -k * (distance - l) * (vectorBetween / distance);
            return force;
        }
        
        private double3x3 SpringJdx(int firstIndex, int secondIndex)
        {
            var xij = positions[firstIndex] - positions[secondIndex];
            var dotResult = math.dot(xij, xij);
            Debug.Assert(dotResult > 0);
            var outer = Double3.OuterProduct(xij, xij);
            var xijs = outer / dotResult;
            var magnitude = Math.Sqrt(dotResult);

            return (xijs + (double3x3.identity - xijs) * ((1 - l) / magnitude)) * k;
        }
        
        private double3x3 SpringJdv() => -kd * double3x3.identity;
        
        private List<List<double3x3>> MakeEmptyGridMatrix() => Enumerable
            .Range(0, positions.Count)
            .Select(_ => Enumerable.Range(0, positions.Count)
                .Select(_ => double3x3.zero /* Set value of cells here */)
                .ToList())
            .ToList();

        private List<double3> MakeEmptyGridVector() => Enumerable
            .Range(0, positions.Count)
            .Select(_ => double3.zero /* Set value of cells here */)
            .ToList();
    }
}
