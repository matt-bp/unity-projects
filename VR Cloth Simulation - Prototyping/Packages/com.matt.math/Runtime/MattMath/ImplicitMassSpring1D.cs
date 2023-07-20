using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MattMath
{
    public class ImplicitMassSpring1D
    {
        #region Simulation Constants

        private const double K = 1000;
        private const double Kd = 0.0;
        private const double L = 0.1;
        private const double Gravity = -10.0;
        private const double Identity = 1.0;

        #endregion

        #region Simulation State

        private readonly List<double> externalVelocities = new() { 0, 0 };
        public readonly List<double> positions = new() { 3, 0 };
        private readonly List<double> velocities = new() { 0, 0 };
        private readonly List<double> masses = new() { 0.1, 0.1 };
        private readonly List<(int, int)> springs = new() { (0, 1) };

        #endregion

        /// <summary>
        /// Time, in seconds, since the simulation started.
        /// </summary>
        private double Elapsed;

        public void Update(double dt)
        {
            // Clearing out forces
            foreach (var index in Enumerable.Range(0, externalVelocities.Count))
            {
                externalVelocities[index] = Gravity;
            }

            // Setting spring force
            foreach (var (firstIndex, secondIndex) in springs)
            {
                var springF = GetSpringForce(positions[firstIndex], positions[secondIndex]);
                externalVelocities[firstIndex] += springF;
                externalVelocities[secondIndex] -= springF;
            }

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
                a[index][index] += Identity * masses[index];

            // Populate external forces vector
            var f = MakeEmptyGridVector();
            foreach (var index in Enumerable.Range(0, f.Count))
                f[index] = dt * f[index];

            var b = ConjugateGradient1D.CgSub(f, ConjugateGradient1D.CgMult(dfdx, velocities));

            var dvs = ConjugateGradient1D.Solve(a, b, 20, 0.001);

            foreach (var (dv, index) in dvs.Select((v, i) => (v, i)))
            {
                if (index == 1) continue; // We're treating the point at 0 as an anchor.

                velocities[index] += dv * 1 / masses[index];
                positions[index] += velocities[index] * dt;
            }

            Elapsed += Time.deltaTime;
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

        private List<List<double>> MakeEmptyGridMatrix() => Enumerable
            .Range(0, positions.Count)
            .Select(_ => Enumerable.Range(0, positions.Count)
                .Select(_ => 0.0 /* Set value of cells here */)
                .ToList())
            .ToList();

        private List<double> MakeEmptyGridVector() => Enumerable
            .Range(0, positions.Count)
            .Select(_ => 0.0 /* Set value of cells here */)
            .ToList();
    }
}