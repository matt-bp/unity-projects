using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Unity.Mathematics;
using UnityEngine;

namespace MattMath._2D
{
    public class ImplicitMassSpring : MonoBehaviour
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

        #endregion
        
        #region Simulation State

        private List<double2> forces = new();
        private List<double2> positions = new();
        private List<double2> velocities = new();
        private List<double> masses = new();
        private List<(int, int)> springs = new();

        public List<double2> Positions => positions;

        public void SetPositionsAndSprings(List<double2> newPositions, List<(int, int)> newSprings)
        {
            forces = Grid<double2>.MakeVector(newPositions.Count, double2.zero);
            positions = newPositions.Select(x => x).ToList();
            velocities = Grid<double2>.MakeVector(newPositions.Count, double2.zero);
            masses = Grid<double>.MakeVector(newPositions.Count, m);
            Debug.Assert(newSprings.All(x => x.Item1 != x.Item2));
            springs = newSprings.Select(x => (x.Item1, x.Item2)).ToList();
            
            Debug.Assert(positions.Count == forces.Count);
            Debug.Assert(positions.Count == velocities.Count);
            Debug.Assert(positions.Count == masses.Count);
            Debug.Assert(springs.All(pair =>
                pair.Item1 >= 0 && pair.Item1 < positions.Count && pair.Item2 >= 0 &&
                pair.Item2 < positions.Count));
            Debug.Assert(springs.All(pair => pair.Item1 != pair.Item2));
        }

        #endregion

        public void StepSimulation(double dt)
        {
            SetForces();
            
            var a = MakeEmptyGridMatrix();
            var dfdx = MakeEmptyGridMatrix();

            foreach (var (firstIndex, secondIndex) in springs)
            {
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
            var f = MakeEmptyGridVector();
            foreach (var index in Enumerable.Range(0, f.Count))
                f[index] = dt * forces[index];
            
            var newVelocities = ConjugateGradient.Mult(velocities, dt * dt);

            var b = ConjugateGradient.Add(f, ConjugateGradient.Mult(dfdx, newVelocities));
            
            var dvs = ConjugateGradient.Solve(a, b, 20, 0.001);

            foreach (var (dv, index) in dvs.Select((v, i) => (v, i)))
            {
                if (index == 1) continue;

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
            foreach (var (firstIndex, secondIndex) in springs)
            {
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
        
        private List<List<double2x2>> MakeEmptyGridMatrix() => Enumerable
            .Range(0, positions.Count)
            .Select(_ => Enumerable.Range(0, positions.Count)
                .Select(_ => double2x2.zero /* Set value of cells here */)
                .ToList())
            .ToList();

        private List<double2> MakeEmptyGridVector() => Enumerable
            .Range(0, positions.Count)
            .Select(_ => double2.zero /* Set value of cells here */)
            .ToList();

        private double2x2 SpringJdx(int firstIndex, int secondIndex)
        {
            var xij = positions[firstIndex] - positions[secondIndex];
            var dotResult = math.dot(xij, xij);
            Debug.Assert(dotResult > 0);
            var outer = mm.outerProduct(xij, xij);
            var xijs = outer / dotResult;
            var magnitude = Math.Sqrt(dotResult);

            return (xijs + (double2x2.identity - xijs) * ((1 - l) / magnitude)) * k;
        }
        
        private double2x2 SpringJdv() => -kd * double2x2.identity;
    }
}