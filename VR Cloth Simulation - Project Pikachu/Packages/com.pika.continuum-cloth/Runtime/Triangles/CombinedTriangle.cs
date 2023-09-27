using Conditions;
using Unity.Mathematics;

namespace Triangles
{
    public interface ICombinedTriangle
    {
        public double A { get; }
        public double3 Wu { get; }
        public double3 Wv { get; }
        public WithRespectTo<double3x3> Dwu { get; }
        public WithRespectTo<double3x3> Dwv { get; }
    }

    public class CombinedTriangle : ICombinedTriangle
    {
        public double A => restSpaceTriangle.Area();

        public double3 Wu => GetDeformationMapDerivative(
            restSpaceTriangle.Dv1,
            restSpaceTriangle.Dv2);

        public double3 Wv => GetDeformationMapDerivative(
            restSpaceTriangle.Du1,
            restSpaceTriangle.Du2);

        public WithRespectTo<double3x3> Dwu => restSpaceTriangle.Dwu();
        public WithRespectTo<double3x3> Dwv => restSpaceTriangle.Dwv();

        private readonly IRestSpaceTriangle restSpaceTriangle;
        private readonly IWorldSpaceTriangle worldSpaceTriangle;

        public CombinedTriangle(IRestSpaceTriangle rest, IWorldSpaceTriangle world)
        {
            restSpaceTriangle = rest;
            worldSpaceTriangle = world;
        }

        /// <summary>
        /// Defined in equation 7.20 for u on page 67.
        /// </summary>
        /// <param name="d1">First rest space delta (Example: Dv_1)</param>
        /// <param name="d2">Second rest space delta (Example: Dv_2)</param>
        /// <returns></returns>
        private double3 GetDeformationMapDerivative(double d1, double d2)
        {
            var dx1 = worldSpaceTriangle.Dx1;
            var dx2 = worldSpaceTriangle.Dx2;
            var d = restSpaceTriangle.D();

            var w = 1 / d * (dx1 * d2 - dx2 * d1);
            
            return w;
        }
    }
}