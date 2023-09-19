using Unity.Mathematics;

namespace Triangles
{
    public class RestSpaceTriangle
    {
        private double2 UV0 { get; }
        private double2 UV1 { get; }
        private double2 UV2 { get; }
        public double Du1 { get; }
        public double Du2 { get; }
        public double Dv1 { get; }
        public double Dv2 { get; }

        public RestSpaceTriangle(double2 uv0, double2 uv1, double2 uv2)
        {
            UV0 = uv0;
            UV1 = uv1;
            UV2 = uv2;
            
            Du1 = UV1.x - UV0.x;
            Du2 = UV2.x - UV0.x;
            Dv1 = UV1.y - UV0.y;
            Dv2 = UV2.y - UV0.y;
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
    }
}