using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

namespace MattMath._1D
{
    public class ImplicitMassSpring : MonoBehaviour
    {
        #region Simulation Constants

        [SerializeField] private double K = 10;
        [SerializeField] private double Kd = 0.0;
        [SerializeField] private double L = 0.1;
        [SerializeField] private double Gravity = -10.0;
        /// <summary>
        /// This is the mass in kg for each particle. This will eventually be replaced by specifying the weight for the
        /// whole cloth, and then evenly distributing that across all particles.
        /// </summary>
        [SerializeField] private double m = 0.1;
        private const double Identity = 1.0;

        #endregion

        #region Simulation State

        private List<double> forces = new();
        private List<double> positions = new();
        private List<double> velocities = new();
        private List<double> masses = new();
        [SerializeField] private List<Tuple<int, int>> springs = new();
        [SerializeField] private List<int> constrainedIndices = new();
        // Replace this with a custom spring pair class
        [SerializeField] private List<SerializableDictionary<int, string>> temp = new();
        
        public List<double> Positions => positions;
        
        public void SetPositionsAndSprings(List<double> newPositions, List<Tuple<int, int>> newSprings)
        {
            forces = Grid<double>.MakeVector(newPositions.Count, 0.0);
            positions = newPositions.Select(x => x).ToList();
            velocities = Grid<double>.MakeVector(newPositions.Count, 0.0);
            masses = Grid<double>.MakeVector(newPositions.Count, m);
            Debug.Assert(newSprings.All(x => x.Item1 != x.Item2));
            springs = newSprings;
            
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

            var a = Grid<double>.MakeMatrix(positions.Count, 0.0);
            var dfdx = Grid<double>.MakeMatrix(positions.Count, 0.0);

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
                a[index][index] += Identity * masses[index];

            // Populate external forces vector
            var f = Helpers.Grid<double>.MakeVector(positions.Count, 0.0);
            foreach (var index in Enumerable.Range(0, f.Count))
                f[index] = dt * forces[index];

            var newVelocities = ConjugateGradient.CgMult(velocities, dt * dt);

            var b = ConjugateGradient.CgAdd(f, ConjugateGradient.CgMult(dfdx, newVelocities));
            
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
            // Clearing out forces
            foreach (var index in Enumerable.Range(0, forces.Count))
            {
                forces[index] = Gravity * masses[index];
            }

            // Setting spring force
            foreach (var (firstIndex, secondIndex) in springs)
            {
                var springF = GetSpringForce(positions[firstIndex], positions[secondIndex]);
                forces[firstIndex] += springF;
                forces[secondIndex] -= springF;
            }
        }

        private double SpringJdx(int firstIndex, int secondIndex)
        {
            var xij = positions[firstIndex] - positions[secondIndex];
            var dotResult = xij * xij;
            Debug.Assert(dotResult > 0);
            var outer = xij * xij;
            var xijs = outer / dotResult;
            var magnitude = Math.Sqrt(dotResult);

            return (xijs + (Identity - xijs) * ((1 - L) / magnitude)) * K;
        }

        private double SpringJdv()
        {
            return -Kd * Identity;
        }

        private double GetSpringForce(double pos1, double pos2)
        {
            var vectorBetween = pos1 - pos2;
            var dist = Math.Abs(vectorBetween);
            var force = -K * (dist - L) * (vectorBetween / dist);
            return force;
        }
    }
}