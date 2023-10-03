using System;
using System.Collections.Generic;
using Conditions;
using DataStructures;
using LinearAlgebra;
using Unity.Mathematics;

namespace Forces
{
    public class StretchConditionForceCalculator : IConditionForceCalculator
    {
        private readonly IConditionQuantities cq;
        private readonly double k;
        private readonly double kd;

        public StretchConditionForceCalculator(double inK, double inKd, IConditionQuantities conditionQuantities)
        {
            cq = conditionQuantities;
            k = inK;
            kd = inKd;
        }

        /// <summary>
        /// Getting the force from this condition.
        ///
        /// Check equation 7.2 (pg. 60) for general form.
        /// </summary>
        /// <param name="i">Particle to consider</param>
        /// <returns></returns>
        public double3 GetForce(int i) => -k * (cq.Dcu[i] * cq.Cu + cq.Dcv[i] * cq.Cv);

        /// <summary>
        /// Gets the damping force for this condition;
        ///
        /// Check equation 7.5 (pg. 61) for the general form.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double3 GetDampingForce(int i) => -kd * (cq.Dcu[i] * cq.CuDot + cq.Dcv[i] * cq.CvDot);

        public WithRespectTo<double3x3> GetForceFirstPartialDerivative(int i)
        {
            var dfidx0 = -k * (
                Double3.OuterProduct(cq.Dcu[i], cq.Dcu.dx0) + cq.D2Cu[i].dx0 * cq.Cu +
                Double3.OuterProduct(cq.Dcv[i], cq.Dcv.dx0) + cq.D2Cv[i].dx0 * cq.Cv
            );
            var dfidx1 = -k * (
                Double3.OuterProduct(cq.Dcu[i], cq.Dcu.dx1) + cq.D2Cu[i].dx1 * cq.Cu +
                Double3.OuterProduct(cq.Dcv[i], cq.Dcv.dx1) + cq.D2Cv[i].dx1 * cq.Cv
            );
            var dfidx2 = -k * (
                Double3.OuterProduct(cq.Dcu[i], cq.Dcu.dx2) + cq.D2Cu[i].dx2 * cq.Cu +
                Double3.OuterProduct(cq.Dcv[i], cq.Dcv.dx2) + cq.D2Cv[i].dx2 * cq.Cv
            );

            return new WithRespectTo<double3x3>()
            {
                dx0 = dfidx0,
                dx1 = dfidx1,
                dx2 = dfidx2
            };
        }

        /// <summary>
        /// Gets the damping force partial derivative with respect to the positions of each of the particles.
        ///
        /// Check out equation 7.8 (pg. 62) for the general form.
        ///
        /// The second bit, \partial C(x) / \partial x_j ^T vj is the same as C dot. 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WithRespectTo<double3x3> GetDampingForcePartialDerivativeWrtPosition(int i)
        {
            var ddp = new WithRespectTo<double3x3>()
            {
                dx0 = -kd * (cq.D2Cu[i].dx0 * cq.CuDot + cq.D2Cv[i].dx0 * cq.CvDot),
                dx1 = -kd * (cq.D2Cu[i].dx1 * cq.CuDot + cq.D2Cv[i].dx1 * cq.CvDot),
                dx2 = -kd * (cq.D2Cu[i].dx2 * cq.CuDot + cq.D2Cv[i].dx2 * cq.CvDot)
            };

            return ddp;
        }

        /// <summary>
        /// Gets the damping force partial derivative with respect to the velocities of the particles.
        ///
        /// Check out equation 7.11 (pg. 62) for the general form.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WithRespectToV<double3x3> GetDampingForcePartialDerivativeWrtVelocity(int i)
        {
            var ddv = new WithRespectToV<double3x3>()
            {
                dv0 = -kd * (Double3.OuterProduct(cq.Dcu[i], cq.Dcu.dx0) + Double3.OuterProduct(cq.Dcv[i], cq.Dcv.dx0)),
                dv1 = -kd * (Double3.OuterProduct(cq.Dcu[i], cq.Dcu.dx1) + Double3.OuterProduct(cq.Dcv[i], cq.Dcv.dx1)),
                dv2 = -kd * (Double3.OuterProduct(cq.Dcu[i], cq.Dcu.dx2) + Double3.OuterProduct(cq.Dcv[i], cq.Dcv.dx2)),
            };

            return ddv;
        }
    }
}