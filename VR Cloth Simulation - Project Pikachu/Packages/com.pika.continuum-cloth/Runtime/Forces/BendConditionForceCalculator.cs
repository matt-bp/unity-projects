using Conditions;
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

        public double3 GetDampingForce(int i)
        {
            throw new System.NotImplementedException();
        }

        public WithRespectTo<double3x3> GetForceFirstPartialDerivative(int i)
        {
            throw new System.NotImplementedException();
        }

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