using System;
using System.Collections.Generic;
using Codice.CM.Common;
using Triangles;
using Unity.Mathematics;

namespace Conditions
{
    public class ShearConditionQuantities : IConditionQuantities
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

        public WithRespectTo<double3> Dcu => throw new NotImplementedException("Don't need this anymore, combine first derivative into one variable, not between u and v.");
        public WithRespectTo<double3> Dcv => throw new NotImplementedException("Don't need this anymore, combine first derivative into one variable, not between u and v.");
        public double CuDot => throw new NotImplementedException();
        public double CvDot => throw new NotImplementedException();
        public WithRespectTo<double3x3> D2CuDx0 => throw new NotImplementedException("Don't need this anymore, combine second derivative into one variable, not between u and v.");
        public WithRespectTo<double3x3> D2CuDx1 => throw new NotImplementedException("Don't need this anymore, combine second derivative into one variable, not between u and v.");
        public WithRespectTo<double3x3> D2CuDx2 => throw new NotImplementedException("Don't need this anymore, combine second derivative into one variable, not between u and v.");
        public WithRespectTo<double3x3> D2CvDx0 => throw new NotImplementedException("Don't need this anymore, combine second derivative into one variable, not between u and v.");
        public WithRespectTo<double3x3> D2CvDx1 => throw new NotImplementedException("Don't need this anymore, combine second derivative into one variable, not between u and v.");
        public WithRespectTo<double3x3> D2CvDx2 => throw new NotImplementedException("Don't need this anymore, combine second derivative into one variable, not between u and v.");
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