using TensorAlgebra;
using Unity.Mathematics;

namespace Conditions
{
    public interface IBendConditionQuantities
    {
        /// <summary>
        /// <para>Check out equation 7.34 Stuyck pg. 72.</para>
        /// <returns>The angle between triangle normals.</returns>
        /// </summary>
        public double C { get; }

        /// <summary>
        /// <para>Check out equation 7.35 Stuyck pg. 72.</para>
        /// <returns>The first derivative of the bend condition function with respect to the two triangle's 4 vertices.</returns>
        /// </summary>
        public WithRespectTo4<double3> Dc { get; }
    }

    public class BendConditionQuantities : IBendConditionQuantities
    {
        private double3 X0 { get; }
        private double3 X1 { get; }
        private double3 X2 { get; }
        private double3 X3 { get; }
        private double3 Na => math.cross(X2 - X0, X1 - X0);
        private double3 NaHat => math.normalize(Na);
        private double3 Nb => math.cross(X1 - X3, X2 - X3);
        private double3 NbHat => math.normalize(Nb);
        private double3 E => X1 - X2;
        private double Cos => math.dot(NaHat, NbHat);
        private double Sin => math.dot(math.cross(NaHat, NbHat), E);

        private WithRespectTo4<double3x3> DnaHat => new()
        {
            Dx0 = 1 / math.length(Na) * Dna.Dx0
        };

        private WithRespectTo4<double3x3> Dna => new()
        {
            Dx0 = double3x3.zero
        };

        public BendConditionQuantities(double3 x0, double3 x1, double3 x2, double3 x3)
        {
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
        }

        public double C => math.atan2(Sin, Cos);

        public WithRespectTo4<double3> Dc => new()
        {
            Dx0 = GetConditionFirstDerivative(0),
        };

        private double3 GetConditionFirstDerivative(int i)
        {
            var dCos = DnaHat[i].Dot(NbHat) + NaHat.Dot(DnaHat[i]);

            return Cos * dCos;
        }
    }
}