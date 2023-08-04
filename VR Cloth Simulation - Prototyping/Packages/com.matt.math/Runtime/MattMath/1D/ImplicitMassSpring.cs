using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MattMath._1D
{
    public class ImplicitMassSpring
    {
        #region Simulation Constants

        private const double K = 10;
        private const double Kd = 0.0;
        private const double L = 0.1;
        private const double Gravity = -10.0;
        private const double Identity = 1.0;

        #endregion

        #region Simulation State

        private readonly List<double> externalForces = new() { 0, 0 };
        public readonly List<double> positions = new() { 3, 0 };
        private readonly List<double> velocities = new() { 0, 0 };
        private readonly List<double> masses = new() { 0.1, 0.1 };
        private readonly List<(int, int)> springs = new() { (0, 1) };

        #endregion
        
        public void Update(double dt)
        {
            //Debug.Log(dt);
            
            // Clearing out forces
            foreach (var index in Enumerable.Range(0, externalForces.Count))
            {
                externalForces[index] = Gravity * masses[index];
            }

            // Setting spring force
            foreach (var (firstIndex, secondIndex) in springs)
            {
                var springF = GetSpringForce(positions[firstIndex], positions[secondIndex]);
                externalForces[firstIndex] += springF;
                externalForces[secondIndex] -= springF;
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
                f[index] = dt * externalForces[index];

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