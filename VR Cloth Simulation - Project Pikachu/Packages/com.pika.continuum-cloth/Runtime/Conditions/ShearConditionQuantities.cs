using System;
using System.Collections.Generic;
using Codice.CM.Common;
using Triangles;
using Unity.Mathematics;

namespace Conditions
{
    public class ShearConditionQuantities
    {
        private double A => combinedTriangle.A;
        public double C => GetCondition();
        public double Cu => throw new NotImplementedException("Don't need this anymore, combine conditions into one variable");
        public double Cv => throw new NotImplementedException("Don't need this anymore, combine conditions into one variable");

        public WithRespectTo<double3> Dc => new()
        {
            dx0 = GetConditionFirstDerivative(0),
            dx1 = GetConditionFirstDerivative(1),
            dx2 = GetConditionFirstDerivative(2)
        };

        private readonly ICombinedTriangle combinedTriangle;

        public ShearConditionQuantities(ICombinedTriangle combined)
        {
            combinedTriangle = combined;
        }

        private double GetCondition() => A * math.dot(combinedTriangle.Wu, combinedTriangle.Wv);
        private double3 GetConditionFirstDerivative(int i) => A *
                                                              (math.mul(combinedTriangle.Dwu[i], combinedTriangle.Wv) +
                                                               math.mul(combinedTriangle.Wu, combinedTriangle.Dwv[i]));
    }
}