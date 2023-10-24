using System;
using Conditions;
using LinearAlgebra;
using Unity.Mathematics;

namespace Forces
{
    public class ShearConditionForceCalculator : IConditionForceCalculator
    {
        private readonly IShearConditionQuantities cq;
        private readonly double k;
        private readonly double kd;

        public ShearConditionForceCalculator(double inK, double inKd, IShearConditionQuantities conditionQuantities)
        {
            cq = conditionQuantities;
            k = inK;
            kd = inKd;
        }

        public double3 GetForce(int i) => -k * cq.C * cq.Dc[i];

        public double3 GetDampingForce(int i) => -kd * cq.Dc[i] * cq.CDot;

        public WithRespectTo<double3x3> GetForceFirstPartialDerivative(int i)
        {
            return new WithRespectTo<double3x3>
            {
                dx0 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.dx0) + cq.D2C[i].dx0 * cq.C),
                dx1 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.dx1) + cq.D2C[i].dx1 * cq.C),
                dx2 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.dx2) + cq.D2C[i].dx2 * cq.C)
            };
        }

        public WithRespectTo<double3x3> GetDampingForcePartialDerivativeWrtPosition(int i)
        {
            var ddp = new WithRespectTo<double3x3>()
            {
                dx0 = -kd * cq.D2C[i].dx0 * cq.CDot,
                dx1 = -kd * cq.D2C[i].dx1 * cq.CDot,
                dx2 = -kd * cq.D2C[i].dx2 * cq.CDot
            };

            return ddp;
        }

        /// <summary>
        /// Check out equation 7.11 of Stuyck (pg. 63).
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WithRespectToV<double3x3> GetDampingForcePartialDerivativeWrtVelocity(int i)
        {
            var ddv = new WithRespectToV<double3x3>()
            {
                dv0 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.dx0),
                dv1 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.dx1),
                dv2 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.dx2)
            };

            return ddv;
        }
    }
}