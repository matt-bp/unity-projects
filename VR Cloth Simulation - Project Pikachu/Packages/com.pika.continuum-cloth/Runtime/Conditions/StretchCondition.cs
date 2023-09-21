using LinearAlgebra;
using Triangles;
using Unity.Mathematics;

namespace Conditions
{
    public static class StretchCondition
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

        public static (double cu, double cv) GetCondition(double3 wu, double3 wv, double a, double2 b)
        {
            var cu = (math.length(wu) - b.x) * a;
            var cv = (math.length(wv) - b.y) * a;

            return (cu, cv);
        }

        public static (WithRespectTo<double3> dcu, WithRespectTo<double3> dcv) GetConditionFirstDerivative(
            double3 wu, double3 wv, WithRespectTo<double3x3> dwu, WithRespectTo<double3x3> dwv, double a)
        {
            WithRespectTo<double3> Combine(double3 w, WithRespectTo<double3x3> dw)
            {
                return new WithRespectTo<double3>()
                {
                    dx0 = math.mul(dw.dx0, math.normalize(w)) * a,
                    dx1 = math.mul(dw.dx1, math.normalize(w)) * a,
                    dx2 = math.mul(dw.dx2, math.normalize(w)) * a,
                };
            }

            return (dcu: Combine(wu, dwu), dcv: Combine(wv, dwv));
        }

        // public static void GetConditionSecondDerivative(RestSpaceTriangle restSpaceTriangle,
        //     WorldSpaceTriangle worldSpaceTriangle)
        // {
        //     var (wu, wv) = GetDeformationMapDerivatives(restSpaceTriangle, worldSpaceTriangle);
        //     var wu_hat = math.normalize(wu);
        //     var a_over_wu = restSpaceTriangle.Area() / math.length(wu);
        //     var i_minus_wuwu = double3x3.identity - Double3.OuterProduct(wu_hat, wu_hat);
        //
        //     var dwu = restSpaceTriangle.Dwu();
        //
        //     var d2cu_dx0dx0 = a_over_wu * dwu.dx0 * dwu.dx0 * i_minus_wuwu;
        //     var d2cu_dx0dx1 = a_over_wu * dwu.dx0 * dwu.dx1 * i_minus_wuwu;
        //     var d2cu_dx0dx2 = a_over_wu * dwu.dx0 * dwu.dx2 * i_minus_wuwu;
        //
        //     var d2cu_dx1dx0 = a_over_wu * dwu.dx1 * dwu.dx0 * i_minus_wuwu;
        //     var d2cu_dx1dx1 = a_over_wu * dwu.dx1 * dwu.dx1 * i_minus_wuwu;
        //     var d2cu_dx1dx2 = a_over_wu * dwu.dx1 * dwu.dx2 * i_minus_wuwu;
        //
        //     var d2cu_dx2dx0 = a_over_wu * dwu.dx2 * dwu.dx0 * i_minus_wuwu;
        //     var d2cu_dx2dx1 = a_over_wu * dwu.dx2 * dwu.dx1 * i_minus_wuwu;
        //     var d2cu_dx2dx2 = a_over_wu * dwu.dx2 * dwu.dx2 * i_minus_wuwu;
        // }
    }

    public class StretchConditionQuantities
    {
        /// <summary>
        /// Area of the passed in triangle in rest space.
        /// </summary>
        public double A { get; }

        public double D { get; }
        public double3 Wu { get; }
        public double3 WuNorm => math.normalize(Wu);
        public WithRespectTo<double3x3> Dwu { get; }
        public double3 Wv { get; }
        public double3 WvNorm => math.normalize(Wv);
        public WithRespectTo<double3x3> Dwv { get; }
        public double Cu { get; set; }
        public double Cv { get; set; }
        public WithRespectTo<double3> Dcu { get; set; }
        public WithRespectTo<double3> Dcv { get; set; }

        public StretchConditionQuantities(RestSpaceTriangle restSpaceTriangle, WorldSpaceTriangle worldSpaceTriangle,
            double2 b)
        {
            A = restSpaceTriangle.Area();
            D = restSpaceTriangle.D();

            var ws = StretchCondition.GetDeformationMapDerivatives(restSpaceTriangle, worldSpaceTriangle);
            Wu = ws.wu;
            Wv = ws.wv;

            var c = StretchCondition.GetCondition(Wu, Wv, A, b);
            Cu = c.cu;
            Cv = c.cv;

            Dwu = restSpaceTriangle.Dwu();
            Dwv = restSpaceTriangle.Dwv();

            var conditionFirstDerivative = StretchCondition.GetConditionFirstDerivative(Wu, Wv, Dwu, Dwv, A);
            Dcu = conditionFirstDerivative.dcu;
            Dcv = conditionFirstDerivative.dcv;

        }
    }
}