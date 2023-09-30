using System;
using System.Collections.Generic;
using LinearAlgebra;
using Triangles;
using Unity.Mathematics;

namespace Conditions.New
{
    public class StretchConditionQuantities
    {
        private double A => combinedTriangle.A;
        private double2 B { get; }
        public double Cu => GetCondition(combinedTriangle.Wu, B.x);
        public double Cv => GetCondition(combinedTriangle.Wv, B.y);
        public WithRespectTo<double3> Dcu => GetConditionFirstDerivative(combinedTriangle.Wu, combinedTriangle.Dwu);
        public WithRespectTo<double3> Dcv => GetConditionFirstDerivative(combinedTriangle.Wv, combinedTriangle.Dwv);

        /// <summary>
        /// Time derivative of this condition in the U direction. Check equation 7.7 (pg. 61).
        ///
        /// In this case, there are 3 particles participating in the condition function.
        /// </summary>
        public double CuDot => math.dot(Dcu.dx0, velocities[0]) + 
                               math.dot(Dcu.dx1, velocities[1]) + 
                               math.dot(Dcu.dx2, velocities[2]);
        public double CvDot => math.dot(Dcv.dx0, velocities[0]) + 
                               math.dot(Dcv.dx1, velocities[1]) + 
                               math.dot(Dcv.dx2, velocities[2]);
        
        
        #region Second Derivative

        public WithRespectTo<double3x3> D2CuDx0 => GetConditionSecondDerivativeU(0);
        public WithRespectTo<double3x3> D2CuDx1 => GetConditionSecondDerivativeU(1);
        public WithRespectTo<double3x3> D2CuDx2 => GetConditionSecondDerivativeU(2);
        
        public WithRespectTo<double3x3> D2CvDx0 => GetConditionSecondDerivativeV(0);
        public WithRespectTo<double3x3> D2CvDx1 => GetConditionSecondDerivativeV(1);
        public WithRespectTo<double3x3> D2CvDx2 => GetConditionSecondDerivativeV(2);
        
        #endregion
        
        private readonly ICombinedTriangle combinedTriangle;
        private readonly List<double3> velocities;

        public StretchConditionQuantities(ICombinedTriangle combined, double2 b, List<double3> v)
        {
            combinedTriangle = combined;
            B = b;
            velocities = v;
        }

        private double GetCondition(double3 w, double b)
        {
            return (math.length(w) - b) * A;
        }

        private WithRespectTo<double3> GetConditionFirstDerivative(double3 w, WithRespectTo<double3x3> dw)
        {
            return new WithRespectTo<double3>
            {
                dx0 = math.mul(dw.dx0, math.normalize(w)) * A,
                dx1 = math.mul(dw.dx1, math.normalize(w)) * A,
                dx2 = math.mul(dw.dx2, math.normalize(w)) * A,
            };
        }

        private WithRespectTo<double3x3> GetConditionSecondDerivativeU(int i)
        {
            var awu = A / math.length(combinedTriangle.Wu);
            var wuNorm = math.normalize(combinedTriangle.Wu);
            var identityMinusWu = double3x3.identity - Double3.OuterProduct(wuNorm, wuNorm);
            var dwu = combinedTriangle.Dwu;
            return new WithRespectTo<double3x3>()
            {
                dx0 = awu * dwu[i] * dwu.dx0 * identityMinusWu,
                dx1 = awu * dwu[i] * dwu.dx1 * identityMinusWu,
                dx2 = awu * dwu[i] * dwu.dx2 * identityMinusWu,
            };
        }
        
        private WithRespectTo<double3x3> GetConditionSecondDerivativeV(int i)
        {
            var awv = A / math.length(combinedTriangle.Wv);
            var wvNorm = math.normalize(combinedTriangle.Wv);
            var identityMinusWv = double3x3.identity - Double3.OuterProduct(wvNorm, wvNorm);
            var dwv = combinedTriangle.Dwu;
            return new WithRespectTo<double3x3>()
            {
                dx0 = awv * dwv[i] * dwv.dx0 * identityMinusWv,
                dx1 = awv * dwv[i] * dwv.dx1 * identityMinusWv,
                dx2 = awv * dwv[i] * dwv.dx2 * identityMinusWv,
            };
        }
    }
}