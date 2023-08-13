using Unity.Mathematics;

namespace Forces
{
    public static class Force
    {
        public static double3 SpringForce(double k, double l, double3 position1, double3 position2)
        {
            var vectorBetween = position1 - position2;
            var distance = math.distance(position1, position2);
            var force = -k * (distance - l) * (vectorBetween / distance);
            return force;
        }
    }
}
