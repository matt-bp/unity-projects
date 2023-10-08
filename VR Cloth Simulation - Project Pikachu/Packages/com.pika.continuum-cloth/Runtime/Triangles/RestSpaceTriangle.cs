using System;
using Conditions;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

namespace Triangles
{
    public interface IRestSpaceTriangle
    {
        public double Area();
        public double D();
        double Du1 { get; }
        double Du2 { get; }
        double Dv1 { get; }
        double Dv2 { get; }
        /// <summary>
        /// <para>Derivative of Wu with respect to each particle of the triangle moving.</para>
        /// <para>Since all of our positions data is linear, the only things left after taking the partial derivative will be quantities from the rest triangle.</para>
        /// <para>Used in the condition's partial derivative with respect to particle's positions.</para>
        /// <para>Defined by equation 7.23 (pg. 68).</para>
        /// </summary>
        /// <returns></returns>
        public WithRespectTo<double> Dwu();
        /// <summary>
        /// <para>Derivative of Wv with respect to each particle of the triangle moving.</para>
        /// <para>Since all of our positions data is linear, the only things left after taking the partial derivative will be quantities from the rest triangle.</para>
        /// <para>Used in the condition's partial derivative with respect to particle's positions.</para>
        /// <para>Defined by equation 7.25 on page 69.</para>
        /// </summary>
        /// <returns></returns>
        public WithRespectTo<double> Dwv();
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
        
        public WithRespectTo<double> Dwu() => new()
        {
            dx0 = (Dv1 - Dv2) / D(),
            dx1 = Dv2 / D(),
            dx2 = -Dv1 / D()
        };

        public WithRespectTo<double> Dwv() => new()
        {
            dx0 = (Du2 - Du1) / D(),
            dx1 = -Du2 / D(),
            dx2 = Du1 / D()
        };
    }
}