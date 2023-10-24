using System.Runtime.CompilerServices;
using Conditions;
using LinearAlgebra;
using Unity.Mathematics;

namespace Forces
{
    public class BendConditionForceCalculator : IConditionForceCalculator
    {
        private readonly IBendConditionQuantities cq;
        private readonly double k;
        private readonly double kd;

        public BendConditionForceCalculator(double inK, double inKd, IBendConditionQuantities conditionQuantities)
        {
            cq = conditionQuantities;
            k = inK;
            kd = inKd;
        }

        public double3 GetForce(int i) => -k * cq.Dc[i] * cq.C;

        public double3 GetDampingForce(int i) => -kd * cq.Dc[i] * cq.CDot;

        public WithRespectTo<double3x3> GetForceFirstPartialDerivative(int i) => new()
        {
            dx0 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx0) + cq.D2C[i].Dx0 * cq.C),
            dx1 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx1) + cq.D2C[i].Dx1 * cq.C),
            dx2 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx2) + cq.D2C[i].Dx2 * cq.C)
        };


        public WithRespectTo<double3x3> GetDampingForcePartialDerivativeWrtPosition(int i)
        {
            throw new System.NotImplementedException();
        }

        public WithRespectToV<double3x3> GetDampingForcePartialDerivativeWrtVelocity(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}