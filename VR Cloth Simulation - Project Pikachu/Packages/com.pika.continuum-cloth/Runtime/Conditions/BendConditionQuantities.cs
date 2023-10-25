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

        /// <summary>
        /// <para>Check equation 7.7 (pg. 61).</para>
        /// </summary>
        public double CDot { get; }

        /// <summary>
        /// <para>Calculates the second partial derivative of the condition function with respect to all points in the triangle pair.</para>
        /// <para>Check equation 57 of Pritchard (pg. 6).</para>
        /// </summary>
        public WithRespectTo4<WithRespectTo4<double3x3>> D2C { get; }
    }

    public class BendConditionQuantities : IBendConditionQuantities
    {
        private double3 X0 { get; }
        private double3 X1 { get; }
        private double3 X2 { get; }
        private double3 X3 { get; }
        private List<double3> Velocities { get; }
        private double3 Na => math.cross(X2 - X0, X1 - X0);
        private double3 NaHat => math.normalize(Na);
        private double3 Nb => math.cross(X1 - X3, X2 - X3);
        private double3 NbHat => math.normalize(Nb);
        private double3 E => X1 - X2;
        private double3 EHat => math.normalize(E);
        private double Cos => math.dot(NaHat, NbHat);
        private double Sin => math.dot(math.cross(NaHat, NbHat), EHat);

        #region Na Derivatives

        private Tuple<double3, double3, double3, double3> Qa =>
            Tuple.Create(X2 - X1, X0 - X2, X1 - X0, math.double3(0));

        private WithRespectTo4<double3x3> Dna => MakeDn(Qa);
        private WithRespectTo4<double3x3> DnaHat => MakeDHat(Na, Dna);

        #endregion

        #region Nb Derivatives

        private Tuple<double3, double3, double3, double3> Qb =>
            Tuple.Create(math.double3(0), X2 - X3, X3 - X1, X1 - X2);

        private WithRespectTo4<double3x3> Dnb => MakeDn(Qb);
        private WithRespectTo4<double3x3> DnbHat => MakeDHat(Nb, Dnb);

        #endregion

        #region E Derivatives

        private static Tuple<double3, double3, double3, double3> Qe =>
            Tuple.Create(math.double3(0), math.double3(1), math.double3(-1), math.double3(0));

        private static WithRespectTo4<double3x3> De => MakeDn(Qe);
        private WithRespectTo4<double3x3> DeHat => MakeDHat(E, De);

        #endregion

        public BendConditionQuantities(double3 x0, double3 x1, double3 x2, double3 x3, List<double3> v)
        {
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
            Velocities = v;
        }

        public double C => math.atan2(Sin, Cos);

        public WithRespectTo4<double3> Dc => new()
        {
            Dx0 = GetConditionFirstDerivative(0),
            Dx1 = GetConditionFirstDerivative(1),
            Dx2 = GetConditionFirstDerivative(2),
            Dx3 = GetConditionFirstDerivative(3),
        };

        public double CDot => math.dot(Dc.Dx0, Velocities[0]) +
                              math.dot(Dc.Dx1, Velocities[1]) +
                              math.dot(Dc.Dx2, Velocities[2]) +
                              math.dot(Dc.Dx3, Velocities[3]);

        /// <summary>
        /// <para>See equation 47 by Pritchard (pg. 5).</para> 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private double MakeDCosByElement(int i, Element element) =>
            math.dot(DnaHat[i][(int)element], NbHat) + math.dot(NaHat, DnbHat[i][(int)element]);

        /// <summary>
        /// <para>See equation 49 by Pritchard (pg. 5).</para>
        /// </summary>
        /// <param name="i"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private double MakeDSinByElement(int i, Element element) =>
            math.dot(math.cross(DnaHat[i][(int)element], NbHat) + math.cross(NaHat, DnbHat[i][(int)element]), EHat) +
            math.dot(math.cross(NaHat, NbHat), DeHat[i][(int)element]);

        private double3 GetConditionFirstDerivative(int i)
        {
            var dCos = math.double3(
                MakeDCosByElement(i, Element.X),
                MakeDCosByElement(i, Element.Y),
                MakeDCosByElement(i, Element.Z)
            );

            var dSin = math.double3(
                MakeDSinByElement(i, Element.X),
                MakeDSinByElement(i, Element.Y),
                MakeDSinByElement(i, Element.Z)
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

        public WithRespectTo4<WithRespectTo4<double3x3>> D2C => new()
        {
            Dx0 = GetConditionSecondDerivative(0),
            Dx1 = GetConditionSecondDerivative(1),
            Dx2 = GetConditionSecondDerivative(2),
            Dx3 = GetConditionSecondDerivative(3),
        };

        private WithRespectTo4<double3x3> GetConditionSecondDerivative(int i) => new()
        {
            Dx0 = GetJacobian(i, 0),
            Dx1 = GetJacobian(i, 1),
            Dx2 = GetJacobian(i, 2),
            Dx3 = GetJacobian(i, 3),
        };

        private enum Element
        {
            X = 0,
            Y = 1,
            Z = 2
        }

        /// <summary>
        /// <para>Check equation 5.16 in Stuyck (pg. 37).</para>
        /// </summary>
        /// <param name="i">First element to check</param>
        /// <param name="j">Second element to check, can be equal to i.</param>
        /// <returns>Jacobian of the derivative of the condition function.</returns>
        private double3x3 GetJacobian(int i, int j)
        {
            double GetEntry(Element iElement, Element jElement) =>
                GetJacobianEntry(i, j, iElement, jElement);

            var c0 = math.double3(
                GetEntry(Element.X, Element.X),
                GetEntry(Element.Y, Element.X),
                GetEntry(Element.Z, Element.X)
            );

            var c1 = math.double3(
                GetEntry(Element.X, Element.Y),
                GetEntry(Element.Y, Element.Y),
                GetEntry(Element.Z, Element.Y)
            );

            var c2 = math.double3(
                GetEntry(Element.X, Element.Z),
                GetEntry(Element.Y, Element.Z),
                GetEntry(Element.Z, Element.Z)
            );

            return math.double3x3(c0, c1, c2);
        }

        /// <summary>
        /// <para>This lookup table contains the second partial derivative of the auxiliary variables A.</para>
        /// <para>When we use this, we index by m and n, and the result is the X, Y, and Z values for that second derivative pairing.</para>
        /// </summary>
        private static double4x4 ASkewSecondPartialLookUp => math.double4x4(
            math.double4(0, -1, 1, 0),
            math.double4(1, 0, -1, 0),
            math.double4(-1, 1, 0, 0),
            math.double4(0, 0, 0, 0)
        );

        /// <summary>
        /// <para>This lookup table contains the second partial derivative of the auxiliary variables B.</para>
        /// <para>When we use this, we index by m and n, and the result is the X, Y, and Z values for that second derivative pairing.</para>
        /// </summary>
        private static double4x4 BSkewSecondPartialLookUp => math.double4x4(
            math.double4(0, 0, 0, 0),
            math.double4(0, 0, 1, -1),
            math.double4(0, -1, 0, 1),
            math.double4(0, 1, -1, 0)
        );

        // private double MakeD2CosByElement(int i, int j, Element iElement, Element jElement)
        // {
        //     
        // }

        /// <summary>
        /// <para>Check equation 57 of Pritchard (pg. 6).</para>
        /// </summary>
        /// <param name="i">First position</param>
        /// <param name="j">Second position, can be equal to the first.</param>
        /// <param name="iElement">Element of the vector to use for i.</param>
        /// <param name="jElement">Element of the vector to use for j.</param>
        /// <returns>Evaluation of equation 57 for one entry in the Jacobian matrix.</returns>
        private double GetJacobianEntry(int i, int j, Element iElement, Element jElement)
        {
            // All the X, Y, and Z components will be the same from this lookup
            var d2Na = math.double3(ASkewSecondPartialLookUp[i][j]);
            var d2Nb = math.double3(BSkewSecondPartialLookUp[i][j]);

            var d2NaHat = 1 / math.length(Na) * d2Na;
            var d2NbHat = 1 / math.length(Nb) * d2Nb;
            
            // Equation 50 of Pritchard (pg. 5).
            var d2Cos = math.dot(d2NaHat, NbHat) +
                        math.dot(DnbHat[j][(int)jElement], DnaHat[i][(int)iElement]) +
                        math.dot(DnaHat[j][(int)jElement], DnbHat[i][(int)iElement]) +
                        math.dot(NaHat, d2NbHat);
            // Equation 51 of Pritchard (pg. 5). There is one more addition at the end of this, but it goes to zero, so no worries.
            var d2Sin = math.dot(math.cross(d2NaHat, NbHat) +
                                 math.cross(DnaHat[i][(int)iElement], DnbHat[j][(int)jElement]) +
                                 math.cross(DnaHat[j][(int)jElement], DnbHat[i][(int)iElement]) +
                                 math.cross(NaHat, d2Nb),
                            EHat) +
                        math.dot(
                            math.cross(DnaHat[i][(int)iElement], NbHat) + math.cross(NaHat, DnbHat[i][(int)iElement]),
                            DeHat[j][(int)jElement]) +
                        math.dot(
                            math.cross(DnaHat[j][(int)jElement], NbHat) + math.cross(NaHat, DnbHat[j][(int)jElement]),
                            DeHat[i][(int)iElement]);

            var dSinI = MakeDSinByElement(i, iElement);
            var dCosI = MakeDCosByElement(i, iElement);
            var dSinJ = MakeDSinByElement(j, jElement);
            var dCosJ = MakeDCosByElement(j, jElement);

            return Cos * d2Sin - Sin * d2Cos +
                   (Sin * Sin - Cos * Cos) * (dSinI * dCosJ + dCosI * dSinJ) +
                   2 * Sin * Cos * (dCosI * dCosJ - dSinI * dSinJ);
        }
    }
}