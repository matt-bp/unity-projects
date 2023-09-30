using Unity.Mathematics;

namespace Conditions
{
    public interface IConditionQuantities
    {
        public double Cu { get; }
        public double Cv { get; }
        public WithRespectTo<double3> Dcu { get; }
        public WithRespectTo<double3> Dcv { get; }
        public double CuDot { get; }
        public double CvDot { get; }
        public WithRespectTo<double3x3> D2CuDx0 { get; }
        public WithRespectTo<double3x3> D2CuDx1 { get; }
        public WithRespectTo<double3x3> D2CuDx2 { get; }
        public WithRespectTo<double3x3> D2CvDx0 { get; }
        public WithRespectTo<double3x3> D2CvDx1 { get; }
        public WithRespectTo<double3x3> D2CvDx2 { get; }
    }
}