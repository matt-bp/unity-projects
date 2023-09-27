using System;
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

        private readonly ICombinedTriangle combinedTriangle;

        public StretchConditionQuantities(ICombinedTriangle combined, double2 b)
        {
            combinedTriangle = combined;
            B = b;
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
    }
}