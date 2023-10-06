using System;
using System.Collections.Generic;
using Triangles;
using Unity.Mathematics;

namespace Conditions
{
    public class ShearConditionQuantities
    {
        private double A => combinedTriangle.A;
        /// <summary>
        /// <para>This scalar is essentially the dot product of the u axis with the v axis in world space.</para>
        /// <para>If no shear is occurring, the axes are perpendicular and the condition function is zero.</para>
        /// <para>If shear is occurring, the condition function is equivalent to the cosine of the angle between them, weighted by the triangle’s area in u / v space.</para>
        /// </summary>
        public double C => GetCondition();
        public WithRespectTo<double3> Dc => new()
        {
            dx0 = GetConditionFirstDerivative(0),
            dx1 = GetConditionFirstDerivative(1),
            dx2 = GetConditionFirstDerivative(2)
        };
        public double CDot => math.dot(Dc.dx0, velocities[0]) + 
                              math.dot(Dc.dx1, velocities[1]) + 
                              math.dot(Dc.dx2, velocities[2]);

        private WithRespectTo<double3x3> D2CDx0 => GetConditionSecondDerivative(0);
        private WithRespectTo<double3x3> D2CDx1 => GetConditionSecondDerivative(1);
        private WithRespectTo<double3x3> D2CDx2 => GetConditionSecondDerivative(2);

        /// <summary>
        /// The second derivative of the condition function, with respect to each particle.
        /// 
        /// Check out equation 7.32 (pg. 71).
        /// </summary>
        public WithRespectTo<WithRespectTo<double3x3>> D2C => new()
        {
            dx0 = D2CDx0,
            dx1 = D2CDx1,
            dx2 = D2CDx2
        };

        private readonly ICombinedTriangle combinedTriangle;
        private readonly List<double3> velocities;

        public ShearConditionQuantities(ICombinedTriangle combined, List<double3> v)
        {
            combinedTriangle = combined;
            velocities = v;
        }

        private double GetCondition() => A * math.dot(combinedTriangle.Wu, combinedTriangle.Wv);
        private double3 GetConditionFirstDerivative(int i) => A *
                                                              (math.mul(combinedTriangle.Dwu[i], combinedTriangle.Wv) +
                                                               math.mul(combinedTriangle.Wu, combinedTriangle.Dwv[i]));

        private WithRespectTo<double3x3> GetConditionSecondDerivative(int i)
        {
            return new WithRespectTo<double3x3>()
            {
                dx0 = combinedTriangle.Dwu[i] * combinedTriangle.Dwv.dx0 +
                      combinedTriangle.Dwu.dx0 * combinedTriangle.Dwv[i],
                dx1 = combinedTriangle.Dwu[i] * combinedTriangle.Dwv.dx1 +
                      combinedTriangle.Dwu.dx1 * combinedTriangle.Dwv[i],
                dx2 = combinedTriangle.Dwu[i] * combinedTriangle.Dwv.dx2 +
                      combinedTriangle.Dwu.dx2 * combinedTriangle.Dwv[i]
            };
        }
    }
}