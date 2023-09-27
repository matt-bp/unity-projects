using System;
using LinearAlgebra;
using Triangles;
using Unity.Mathematics;

namespace Conditions.New
{
    public class StretchConditionQuantities
    {
        private double2 B { get; }
        public double Cu => GetCondition(combinedTriangle.Wu, combinedTriangle.A, B.x);
        public double Cv => GetCondition(combinedTriangle.Wv, combinedTriangle.A, B.x);

        private readonly ICombinedTriangle combinedTriangle;

        public StretchConditionQuantities(ICombinedTriangle combined, double2 b)
        {
            combinedTriangle = combined;
            B = b;

            
        }

        private static double GetCondition(double3 w, double a, double b)
        {
            return (math.length(w) - b) * a;
        }
    }
}