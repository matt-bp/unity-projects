using System.Runtime.CompilerServices;
using Conditions;
using LinearAlgebra;
using Unity.Mathematics;

namespace Forces
{
    public class BendConditionForceCalculator : IConditionForceCalculator4
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

        public WithRespectTo4<double3x3> GetForceFirstPartialDerivative(int i) => new()
        {
            Dx0 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx0) + cq.D2C[i].Dx0 * cq.C),
            Dx1 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx1) + cq.D2C[i].Dx1 * cq.C),
            Dx2 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx2) + cq.D2C[i].Dx2 * cq.C),
            Dx3 = -k * (Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx3) + cq.D2C[i].Dx3 * cq.C)
        };
        
        public WithRespectTo4<double3x3> GetDampingForcePartialDerivativeWrtPosition(int i) => new()
        {
            Dx0 = -kd * cq.D2C[i].Dx0 * cq.CDot,
            Dx1 = -kd * cq.D2C[i].Dx1 * cq.CDot,
            Dx2 = -kd * cq.D2C[i].Dx2 * cq.CDot,
            Dx3 = -kd * cq.D2C[i].Dx3 * cq.CDot
        };

        public WithRespectToV4<double3x3> GetDampingForcePartialDerivativeWrtVelocity(int i) => new()
        {
            Dv0 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx0),
            Dv1 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx1),
            Dv2 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx2),
            Dv3 = -kd * Double3.OuterProduct(cq.Dc[i], cq.Dc.Dx3)
        };
    }
}