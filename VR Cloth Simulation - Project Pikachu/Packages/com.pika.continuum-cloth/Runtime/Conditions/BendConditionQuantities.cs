using Unity.Mathematics;

namespace Conditions
{
    public interface IBendConditionQuantities
    {
        public double C { get; }
    }
    
    public class BendConditionQuantities : IBendConditionQuantities
    {
        public double C
        {
            get
            {
                var cosTheta = math.dot(Na, Nb);
                var sinTheta = math.dot(math.cross(Na, Nb), E);
                return math.atan2(sinTheta, cosTheta);
            }
        }

        private double3 Na { get; }
        private double3 Nb { get; }
        private double3 E { get; }

        public BendConditionQuantities(double3 x0, double3 x1, double3 x2, double3 x3)
        {
            Na = math.normalize(math.cross(x2 - x0, x1 - x0));
            Nb = math.normalize(math.cross(x1 - x3, x2 - x3));
            E = x1 - x2;
        }
    }
}