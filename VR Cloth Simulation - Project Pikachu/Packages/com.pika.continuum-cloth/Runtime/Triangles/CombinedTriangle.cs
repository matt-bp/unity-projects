using Conditions;
using JetBrains.Annotations;
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
        private double? d;
        private double D
        {
            get
            {
                return d ??= restSpaceTriangle.D();
            }
        }
        
        private double? a;
        public double A
        {
            get
            {
                return a ??= restSpaceTriangle.Area();
            }
        }

        public double3 Wu => GetDeformationMapDerivativeU();

        public double3 Wv => GetDeformationMapDerivativeV();

        [CanBeNull] private WithRespectTo<double3x3> dwu;

        public WithRespectTo<double3x3> Dwu
        {
            get
            {
                return dwu ??= restSpaceTriangle.Dwu();
            }   
        }
        
        [CanBeNull] private WithRespectTo<double3x3> dwv;

        public WithRespectTo<double3x3> Dwv
        {
            get
            {
                return dwv ??= restSpaceTriangle.Dwv();
            }   
        }

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
        private double3 GetDeformationMapDerivativeU()
        {
            var dv1 = restSpaceTriangle.Dv1;
            var dv2 = restSpaceTriangle.Dv2;
            var dx1 = worldSpaceTriangle.Dx1;
            var dx2 = worldSpaceTriangle.Dx2;

            var w = 1 / D * (dx1 * dv2 - dx2 * dv1);
            
            return w;
        }
        
        /// <summary>
        /// <para>My own derivation for the V map derivative. It's not the same as U! I followed how the one for U was derived, and applied that to V, and they're different!</para>
        /// <para>For the derivation, look at equation 7.16, we have to work out the inverse of th matrix (I think that's right?) to see what values we multiply to get $W_v$.</para>
        /// </summary>
        /// <returns></returns>
        private double3 GetDeformationMapDerivativeV()
        {
            var du1 = restSpaceTriangle.Du1;
            var du2 = restSpaceTriangle.Du2;
            var dx1 = worldSpaceTriangle.Dx1;
            var dx2 = worldSpaceTriangle.Dx2;

            var w = 1 / D * (-dx1 * du2 + dx2 * du1);
            
            return w;
        }
    }
}