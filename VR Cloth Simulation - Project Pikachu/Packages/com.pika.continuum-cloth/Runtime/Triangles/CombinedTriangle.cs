using Unity.Mathematics;

namespace Triangles
{
    public interface ICombinedTriangle
    {
        public double A { get; }
        public double3 Wu { get; }
        public double3 Wv { get; }
    }
    
    public class CombinedTriangle : ICombinedTriangle
    {
        public double A => restSpaceTriangle.Area();

        public double3 Wu => GetDeformationMapDerivative(
            worldSpaceTriangle.Dx1,
            worldSpaceTriangle.Dx2,
            restSpaceTriangle.Dv1,
            restSpaceTriangle.Dv2,
            restSpaceTriangle.D());

        public double3 Wv => GetDeformationMapDerivative(
            worldSpaceTriangle.Dx1,
            worldSpaceTriangle.Dx2,
            restSpaceTriangle.Du1,
            restSpaceTriangle.Du2,
            restSpaceTriangle.D());
        
        private readonly IRestSpaceTriangle restSpaceTriangle;
        private readonly IWorldSpaceTriangle worldSpaceTriangle;

        public CombinedTriangle(IRestSpaceTriangle rest, IWorldSpaceTriangle world)
        {
            restSpaceTriangle = rest;
            worldSpaceTriangle = world;
        }
        
        private static double3 GetDeformationMapDerivative(double3 dx1, double3 dx2, double d1, double d2, double d)
        {
            var w = math.double3(
                dx1.x * d2 - dx2.x * d1,
                dx1.y * d2 - dx2.y * d1,
                dx1.z * d2 - dx2.z * d1
            );
            w /= d;
            return w;
        }
    }
}