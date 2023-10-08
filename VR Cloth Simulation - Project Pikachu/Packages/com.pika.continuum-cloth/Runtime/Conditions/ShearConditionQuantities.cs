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
        /// <para>Check out equation 7.28 (pg. 70).</para>
        /// </summary>
        public double C => A * math.dot(combinedTriangle.Wu, combinedTriangle.Wv);
        public virtual WithRespectTo<double3> Dc => new()
        {
            dx0 = GetConditionFirstDerivative(0),
            dx1 = GetConditionFirstDerivative(1),
            dx2 = GetConditionFirstDerivative(2)
        };
        
        /// <summary>
        /// <para>Calculates the time derivative of the condition function.</para>
        ///
        /// <para>This is essentially a summation of the dot product of each conditions
        /// function's first derivative with respect to a
        /// particle with that particles current velocity.</para>
        /// <para>Check equation 7.7 (pg. 61).</para>
        /// </summary>
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

        /// <summary>
        /// <para>Gets the partial derivative of the condition with respect to a particle i.</para>
        /// <para>Check out equation 7.31 (pg. 71).</para>
        /// </summary>
        /// <param name="i">ith particle to perform the partial derivative with respect to.</param>
        /// <returns></returns>
        private double3 GetConditionFirstDerivative(int i)
        {
            var dwuxdxix = combinedTriangle.Dwu[i];
            var dwvxdxix = combinedTriangle.Dwv[i];
            return A * (dwuxdxix * combinedTriangle.Wv + combinedTriangle.Wu * dwvxdxix);
        }

        /// <summary>
        /// Should be the identity matrix multiplied by a scalar.
        ///
        /// Check out equation 7.32 (pg. 71).
        /// </summary>
        /// <param name="i">ith particle to perform the partial derivative.</param>
        /// <returns>Second derivative of the condition function with respect to i and all particles j.</returns>
        private WithRespectTo<double3x3> GetConditionSecondDerivative(int i)
        {
            var dwuxdxix = combinedTriangle.Dwu[i];
            var dwvxdxix = combinedTriangle.Dwv[i];

            var dwuxdx0x = combinedTriangle.Dwu.dx0;
            var dwuxdx1x = combinedTriangle.Dwu.dx1;
            var dwuxdx2x = combinedTriangle.Dwu.dx2;
            
            var dwvxdx0x = combinedTriangle.Dwv.dx0;
            var dwvxdx1x = combinedTriangle.Dwv.dx1;
            var dwvxdx2x = combinedTriangle.Dwv.dx2;

            var d2cdxix0 = dwuxdxix * dwvxdx0x + dwuxdx0x * dwvxdxix;
            var d2cdxix1 = dwuxdxix * dwvxdx1x + dwuxdx1x * dwvxdxix;
            var d2cdxix2 = dwuxdxix * dwvxdx2x + dwuxdx2x * dwvxdxix;
            
            return new WithRespectTo<double3x3>()
            {
                dx0 = d2cdxix0 * double3x3.identity,
                dx1 = d2cdxix1 * double3x3.identity,
                dx2 = d2cdxix2 * double3x3.identity
            };
        }
    }
}