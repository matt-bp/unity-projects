using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LinearAlgebra;
using MassSpring.DataStructures;
using Solvers;
using Unity.Mathematics;
using UnityEngine;

namespace MassSpring.Integration
{
    [AddComponentMenu("Mass Spring/Integration/2D Implicit Mass Spring")]
    public class ImplicitMassSpring2D : MonoBehaviour
    {
        #region Simulation Constants

        [SerializeField] private double k = 10;
        [SerializeField] private double kd = 0.0;
        [SerializeField] private double l = 0.1;
        [SerializeField] private double2 gravity = math.double2(0.0, -10.0);
        /// <summary>
        /// This is the mass in kg for each particle. This will eventually be replaced by specifying the weight for the
        /// whole cloth, and then evenly distributing that across all particles.
        /// </summary>
        [SerializeField] private double m = 0.1;

        [SerializeField] private List<int> constrainedIndices;
        [SerializeField] private List<ParticlePair> springs = new();
        
        #endregion
        
        #region Simulation State

        private List<double2> forces = new();
        private List<double2> positions = new();
        private List<double2> velocities = new();
        private List<double> masses = new();

        public List<double2> Positions => positions;

        public void SetPositionsAndSprings(List<double2> newPositions)
        {
            forces = Grid<double2>.MakeVector(newPositions.Count, double2.zero);
            positions = newPositions.Select(x => x).ToList();
            velocities = Grid<double2>.MakeVector(newPositions.Count, double2.zero);
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
            
            var a = Grid<double2x2>.MakeMatrix(positions.Count, double2x2.zero);
            var dfdx = Grid<double2x2>.MakeMatrix(positions.Count, double2x2.zero);

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
                a[index][index] += double2x2.identity * masses[index];
            
            // Populate forces vector
            var f = Grid<double2>.MakeVector(positions.Count, double2.zero);
            foreach (var index in Enumerable.Range(0, f.Count))
                f[index] = dt * forces[index];
            
            var newVelocities = ConjugateGradient2D.Mult(velocities, dt * dt);

            var b = ConjugateGradient2D.Add(f, ConjugateGradient2D.Mult(dfdx, newVelocities));
            
            var dvs = ConjugateGradient2D.ConstrainedSolve(a, b, 20, 0.001, constrainedIndices);

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
                var firstIndex = spring.firstIndex;
                var secondIndex = spring.secondIndex;
                
                var springForce = GetSpringForce(positions[firstIndex], positions[secondIndex]);
                forces[firstIndex] += springForce;
                forces[secondIndex] -= springForce;
            }
        }

        private double2 GetSpringForce(double2 position1, double2 position2)
        {
            var vectorBetween = position1 - position2;
            var distance = math.distance(position1, position2);
            var force = -k * (distance - l) * (vectorBetween / distance);
            return force;
        }

        private double2x2 SpringJdx(int firstIndex, int secondIndex)
        {
            var xij = positions[firstIndex] - positions[secondIndex];
            var dotResult = math.dot(xij, xij);
            Debug.Assert(dotResult > 0);
            var outer = Double2.OuterProduct(xij, xij);
            var xijs = outer / dotResult;
            var magnitude = Math.Sqrt(dotResult);

            return (xijs + (double2x2.identity - xijs) * ((1 - l) / magnitude)) * k;
        }
        
        private double2x2 SpringJdv() => -kd * double2x2.identity;
    }
}