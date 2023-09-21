using Triangles;
using Unity.Mathematics;

namespace Conditions
{
    public class StretchCondition
    {
        public static (double3 wu, double3 wv) GetDeformationMapDerivatives(RestSpaceTriangle restSpaceTriangle,
            WorldSpaceTriangle worldSpaceTriangle)
        {
            var (du1, du2, dv1, dv2) = restSpaceTriangle.GetDifferences();
            var (dx1, dx2) = worldSpaceTriangle.GetDifferences();

            var wu = math.double3(
                dx1.x * dv2 - dx2.x * dv1,
                dx1.y * dv2 - dx2.y * dv1,
                dx1.z * dv2 - dx2.z * dv1
            );
            wu /= restSpaceTriangle.D();

            var wv = math.double3(
                dx1.x * du2 - dx2.x * du1,
                dx1.y * du2 - dx2.y * du1,
                dx1.z * du2 - dx2.z * du1
            );
            wv /= restSpaceTriangle.D();

            return (wu, wv);
        }

        public static (double cu, double cv) GetCondition(RestSpaceTriangle restSpaceTriangle,
            WorldSpaceTriangle worldSpaceTriangle, double2 b)
        {
            var (wu, wv) = GetDeformationMapDerivatives(restSpaceTriangle, worldSpaceTriangle);

            var cu = (math.length(wu) - b.x) * restSpaceTriangle.Area();
            var cv = (math.length(wv) - b.y) * restSpaceTriangle.Area();

            return (cu, cv);
        }
        
        public static (WithRespectTo<double3> dcu, WithRespectTo<double3> dcv) GetConditionFirstDerivative(
            RestSpaceTriangle restSpaceTriangle, WorldSpaceTriangle worldSpaceTriangle)
        {
            var (wu, wv) = GetDeformationMapDerivatives(restSpaceTriangle, worldSpaceTriangle);

            WithRespectTo<double3> Combine(double3 w, WithRespectTo<double3x3> dw)
            {
                return new WithRespectTo<double3>()
                {
                    dx0 = math.mul(dw.dx0, math.normalize(w)) * restSpaceTriangle.Area(),
                    dx1 = math.mul(dw.dx1, math.normalize(w)) * restSpaceTriangle.Area(),
                    dx2 = math.mul(dw.dx2, math.normalize(w)) * restSpaceTriangle.Area(),
                };
            }

            return (dcu: Combine(wu, restSpaceTriangle.Dwu()), dcv: Combine(wv, restSpaceTriangle.Dwv()));
        }

        public static void GetConditionSecondDerivative(RestSpaceTriangle restSpaceTriangle,
            WorldSpaceTriangle worldSpaceTriangle)
        {
            
        }
    }
}