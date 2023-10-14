using Unity.Mathematics;

namespace Conditions
{
    public interface IBendConditionQuantities
    {
        public double C { get; }
        public WithRespectTo4<double3> Dc { get; }
    }
    
    public class BendConditionQuantities : IBendConditionQuantities
    {
        private double3 X0 { get; }
        private double3 X1 { get; }
        private double3 X2 { get; }
        private double3 X3 { get; }

        private double3 Na => math.cross(X2 - X0, X1 - X0);
        private double3 NaNorm => math.normalize(Na);
        private double3 Nb => math.cross(X1 - X3, X2 - X3);
        private double3 NbNorm => math.normalize(Nb);
        private double3 E => X1 - X2;

        // Add in other partial derivatives as well
        private WithRespectTo4<double3x3> DeDxi { get; }

        public double C
        {
            get
            {
                var cosTheta = math.dot(NaNorm, NbNorm);
                var sinTheta = math.dot(math.cross(NaNorm, NbNorm), E);
                return math.atan2(sinTheta, cosTheta);
            }
        }

        public WithRespectTo4<double3> Dc =>
            new()
            {
                Dx0 = GetConditionFirstDerivative(0),
                Dx1 = GetConditionFirstDerivative(1),
                Dx2 = GetConditionFirstDerivative(2),
                Dx3 = GetConditionFirstDerivative(3),
            };

        public BendConditionQuantities(double3 x0, double3 x1, double3 x2, double3 x3)
        {
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
        }

        private double3 GetConditionFirstDerivative(int i)
        {
            // sin partial wrt i * cos + sin * cos partial wrt i
            // Start figuring out what this should be
            

            return double3.zero;
        }
    }
}