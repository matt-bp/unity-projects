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
        
        // private double3 WuNorm => math.normalize(Wu);
        // private WithRespectTo<double3x3> Dwu { get; }
        // private double3 Wv { get; }
        // private double3 WvNorm => math.normalize(Wv);
        // private WithRespectTo<double3x3> Dwv { get; }
        //
        // public double Cv { get; }
        // public WithRespectTo<double3> Dcu { get; }
        // public WithRespectTo<double3> Dcv { get; }
        //
        // /// <summary>
        // /// Time derivative of the condition in the U direction.
        // /// </summary>
        // public double CuDot { get; set; }
        //
        // /// <summary>
        // /// Time derivative of the condition in the V direction.
        // /// </summary>
        // public double CvDot { get; set; }
        //
        // public WithRespectTo<double3x3> D2CuDx0 { get; }
        // public WithRespectTo<double3x3> D2CuDx1 { get; }
        // public WithRespectTo<double3x3> D2CuDx2 { get; }
        // public WithRespectTo<double3x3> D2CvDx0 { get; }
        // public WithRespectTo<double3x3> D2CvDx1 { get; }
        // public WithRespectTo<double3x3> D2CvDx2 { get; }

        private readonly ICombinedTriangle combinedTriangle;

        public StretchConditionQuantities(ICombinedTriangle combined,
            double2 b, Tuple<double3, double3, double3> velocities)
        {
            combinedTriangle = combined;
            B = b;

            // D = restSpaceTriangle.D();
            //
            // var ws = GetDeformationMapDerivatives(restSpaceTriangle, worldSpaceTriangle);
            // Wu = ws.wu;
            // Wv = ws.wv;
            //
            // var c = GetCondition(Wu, Wv, A, b);
            // Cu = c.cu;
            // Cv = c.cv;
            //
            // Dwu = restSpaceTriangle.Dwu();
            // Dwv = restSpaceTriangle.Dwv();

            // var conditionFirstDerivative = StretchCondition.GetConditionFirstDerivative(Wu, Wv, Dwu, Dwv, A);
            // Dcu = conditionFirstDerivative.dcu;
            // Dcv = conditionFirstDerivative.dcv;
            //
            // CuDot = math.dot(Dcu.dx0, velocities.Item1) + 
            //         math.dot(Dcu.dx1, velocities.Item2) +
            //         math.dot(Dcu.dx2, velocities.Item3);
            //
            // CvDot = math.dot(Dcv.dx0, velocities.Item1) + 
            //         math.dot(Dcv.dx1, velocities.Item2) +
            //         math.dot(Dcv.dx2, velocities.Item3);
            //
            // var awu = A / math.length(Wu);
            // var identityMinusWu = double3x3.identity - Double3.OuterProduct(WuNorm, WuNorm);
            // D2CuDx0 = new WithRespectTo<double3x3>()
            // {
            //     dx0 = awu * Dwu.dx0 * Dwu.dx0 * identityMinusWu,
            //     dx1 = awu * Dwu.dx0 * Dwu.dx1 * identityMinusWu,
            //     dx2 = awu * Dwu.dx0 * Dwu.dx2 * identityMinusWu,
            // };
            //
            // D2CuDx1 = new WithRespectTo<double3x3>()
            // {
            //     dx0 = awu * Dwu.dx1 * Dwu.dx0 * identityMinusWu,
            //     dx1 = awu * Dwu.dx1 * Dwu.dx1 * identityMinusWu,
            //     dx2 = awu * Dwu.dx1 * Dwu.dx2 * identityMinusWu,
            // };
            //
            // D2CuDx2 = new WithRespectTo<double3x3>()
            // {
            //     dx0 = awu * Dwu.dx1 * Dwu.dx0 * identityMinusWu,
            //     dx1 = awu * Dwu.dx1 * Dwu.dx1 * identityMinusWu,
            //     dx2 = awu * Dwu.dx1 * Dwu.dx2 * identityMinusWu,
            // };
            //
            // var awv = A / math.length(Wv);
            // var identityMinusWv = double3x3.identity - Double3.OuterProduct(WvNorm, WvNorm);
            // D2CvDx0 = new WithRespectTo<double3x3>()
            // {
            //     dx0 = awv * Dwv.dx0 * Dwv.dx0 * identityMinusWv,
            //     dx1 = awv * Dwv.dx0 * Dwv.dx1 * identityMinusWv,
            //     dx2 = awv * Dwv.dx0 * Dwv.dx2 * identityMinusWv,
            // };
            //
            // D2CvDx1 = new WithRespectTo<double3x3>()
            // {
            //     dx0 = awv * Dwv.dx1 * Dwv.dx0 * identityMinusWv,
            //     dx1 = awv * Dwv.dx1 * Dwv.dx1 * identityMinusWv,
            //     dx2 = awv * Dwv.dx1 * Dwv.dx2 * identityMinusWv,
            // };
            //
            // D2CvDx2 = new WithRespectTo<double3x3>()
            // {
            //     dx0 = awv * Dwv.dx1 * Dwv.dx0 * identityMinusWv,
            //     dx1 = awv * Dwv.dx1 * Dwv.dx1 * identityMinusWv,
            //     dx2 = awv * Dwv.dx1 * Dwv.dx2 * identityMinusWv,
            // };
        }

        private static (double3 wu, double3 wv) GetDeformationMapDerivatives(IRestSpaceTriangle restSpaceTriangle,
            IWorldSpaceTriangle worldSpaceTriangle)
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

        private static (double cu, double cv) GetCondition(double3 wu, double3 wv, double a, double2 b)
        {
            var cu = (math.length(wu) - b.x) * a;
            var cv = (math.length(wv) - b.y) * a;

            return (cu, cv);
        }

        private static double GetCondition(double3 w, double a, double b)
        {
            return (math.length(w) - b) * a;
        }

        // private static double3 GetDeformationMapDerivative(double3 dx1, double3 dx2, double d1, double d2, double d)
                    // {
                    //     var w = math.double3(
                    //         dx1.x * d2 - dx2.x * d1,
                    //         dx1.y * d2 - dx2.y * d1,
                    //         dx1.z * d2 - dx2.z * d1
                    //     );
                    //     w /= d;
                    //     return w;
                    // }
    }
}