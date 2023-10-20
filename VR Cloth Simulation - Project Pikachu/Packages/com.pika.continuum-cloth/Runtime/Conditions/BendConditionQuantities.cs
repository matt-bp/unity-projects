using System;
using System.Collections.Generic;
using LinearAlgebra;
using Unity.Mathematics;

namespace Conditions
{
    public interface IBendConditionQuantities
    {
        /// <summary>
        /// <para>Check out equation 7.34 Stuyck (pg. 72).</para>
        /// <returns>The angle between triangle normals.</returns>
        /// </summary>
        public double C { get; }

        /// <summary>
        /// <para>Check out equation 7.35 Stuyck (pg. 72).</para>
        /// <para>Check out equation 54 of Pritchard (pg. 5) for per vector quantity formulations.</para>
        /// <returns>The first derivative of the bend condition function with respect to the two triangle's 4 vertices.</returns>
        /// </summary>
        public WithRespectTo4<double3> Dc { get; }
    }

    public class BendConditionQuantities : IBendConditionQuantities
    {
        private double3 X0 { get; }
        private double3 X1 { get; }
        private double3 X2 { get; }
        private double3 X3 { get; }
        private double3 Na => math.cross(X2 - X0, X1 - X0);
        private double3 NaHat => math.normalize(Na);
        private double3 Nb => math.cross(X1 - X3, X2 - X3);
        private double3 NbHat => math.normalize(Nb);
        private double3 E => X1 - X2;
        private double3 EHat => math.normalize(E);
        private double Cos => math.dot(NaHat, NbHat);
        private double Sin => math.dot(math.cross(NaHat, NbHat), E);

        private Tuple<double3, double3, double3, double3> Qa =>
            Tuple.Create(X2 - X1, X0 - X2, X1 - X0, math.double3(0));
        private WithRespectTo4<double3x3> Dna => MakeDn(Qa);
        private WithRespectTo4<double3x3> DnaHat => MakeDHat(Na, Dna);
        private Tuple<double3, double3, double3, double3> Qb =>
            Tuple.Create(math.double3(0), X2 - X3, X3 - X1, X1 - X2);
        private WithRespectTo4<double3x3> Dnb => MakeDn(Qb);
        private WithRespectTo4<double3x3> DnbHat => MakeDHat(Nb, Dnb);
        private static Tuple<double3, double3, double3, double3> Qe =>
            Tuple.Create(math.double3(0), math.double3(1), math.double3(-1), math.double3(0));
        private static WithRespectTo4<double3x3> De => MakeDn(Qe);
        private WithRespectTo4<double3x3> DeHat => MakeDHat(E, De);

        public BendConditionQuantities(double3 x0, double3 x1, double3 x2, double3 x3)
        {
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
        }

        public double C => math.atan2(Sin, Cos);

        public WithRespectTo4<double3> Dc => new()
        {
            Dx0 = GetConditionFirstDerivative(0),
            Dx1 = GetConditionFirstDerivative(1),
            Dx2 = GetConditionFirstDerivative(2),
            Dx3 = GetConditionFirstDerivative(3),
        };

        private double3 GetConditionFirstDerivative(int i)
        {
            // See equation 47 by Pritchard (pg. 5).
            double MakeDCosByElement(int element) =>
                math.dot(DnaHat[i][element], NbHat) + math.dot(NaHat, DnbHat[i][element]);

            var dCos = math.double3(
                MakeDCosByElement(0),   // s = 0 (X)
                MakeDCosByElement(1),   // s = 1 (Y)
                MakeDCosByElement(2)    // s = 2 (Z)
            );

            // See equation 49 by Pritchard (pg. 5).
            double MakeDSinByElement(int element) =>
                math.dot(math.cross(DnaHat[i][element], NbHat) + math.cross(NaHat, DnbHat[i][element]), EHat) +
                math.dot(math.cross(NaHat, NbHat), DeHat[i][element]);

            var dSin = math.double3(
                MakeDSinByElement(0),   // s = 0 (X)
                MakeDSinByElement(1),   // s = 1 (Y)
                MakeDSinByElement(2)    // s = 2 (Z)
            );

            return Cos * dSin - Sin * dCos;
        }

        private static WithRespectTo4<double3x3> MakeDn(Tuple<double3, double3, double3, double3> auxiliary) => new()
        {
            Dx0 = SkewMatrix.MakeFromVector(auxiliary.Item1),
            Dx1 = SkewMatrix.MakeFromVector(auxiliary.Item2),
            Dx2 = SkewMatrix.MakeFromVector(auxiliary.Item3),
            Dx3 = SkewMatrix.MakeFromVector(auxiliary.Item4),
        };

        private static WithRespectTo4<double3x3> MakeDHat(double3 normal, WithRespectTo4<double3x3> dn)
        {
            var inverseLength = 1 / math.length(normal);

            return new WithRespectTo4<double3x3>
            {
                Dx0 = inverseLength * dn.Dx0,
                Dx1 = inverseLength * dn.Dx1,
                Dx2 = inverseLength * dn.Dx2,
                Dx3 = inverseLength * dn.Dx3
            };
        }
    }
}