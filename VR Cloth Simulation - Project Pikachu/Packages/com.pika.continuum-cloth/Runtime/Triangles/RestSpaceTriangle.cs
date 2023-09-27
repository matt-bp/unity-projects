using System;
using Conditions;
using Unity.Mathematics;
using UnityEngine;

namespace Triangles
{
    public interface IRestSpaceTriangle
    {
        public double Area();
        public (double Du1, double Du2, double Dv1, double Dv2) GetDifferences();
        public double D();
        double Du1 { get; }
        double Du2 { get; }
        double Dv1 { get; }
        double Dv2 { get; }
        public WithRespectTo<double3x3> Dwu();
        public WithRespectTo<double3x3> Dwv();
    }
    
    [Serializable]
    public class RestSpaceTriangle : IRestSpaceTriangle
    {
        [SerializeField] private double2 uv0;
        [SerializeField] private double2 uv1;
        [SerializeField] private double2 uv2;
        public double Du1 { get; }
        public double Du2 { get; }
        public double Dv1 { get; }
        public double Dv2 { get; }

        public RestSpaceTriangle(double2 uv0, double2 uv1, double2 uv2)
        {
            this.uv0 = uv0;
            this.uv1 = uv1;
            this.uv2 = uv2;

            Du1 = this.uv1.x - this.uv0.x;
            Du2 = this.uv2.x - this.uv0.x;
            Dv1 = this.uv1.y - this.uv0.y;
            Dv2 = this.uv2.y - this.uv0.y;
        }

        public double Area()
        {
            var vec1 = math.double3(Du1, Dv1, 0);
            var vec2 = math.double3(Du2, Dv2, 0);

            return 0.5 * math.length(math.cross(vec2, vec1));
        }

        public double D() => Du1 * Dv2 - Du2 * Dv1;

        public (double Du1, double Du2, double Dv1, double Dv2) GetDifferences()
        {
            return (Du1, Du2, Dv1, Dv2);
        }

        public WithRespectTo<double3x3> Dwu() => new()
        {
            dx0 = double3x3.identity * (Dv1 - Dv2) / D(),
            dx1 = double3x3.identity * Dv2 / D(),
            dx2 = double3x3.identity * -Dv1 / D()
        };

        public WithRespectTo<double3x3> Dwv() => new()
        {
            dx0 = double3x3.identity * (Du2 - Du1) / D(),
            dx1 = double3x3.identity * -Du2 / D(),
            dx2 = double3x3.identity * Du1 / D()
        };
    }
}