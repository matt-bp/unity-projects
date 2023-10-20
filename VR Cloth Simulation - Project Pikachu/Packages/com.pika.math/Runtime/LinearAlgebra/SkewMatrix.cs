using Unity.Mathematics;

namespace LinearAlgebra
{
    public static class SkewMatrix
    {
        /// <summary>
        /// <para>Returns the skew-symmetric matrix form of a vector cross product.</para>
        /// <para>Check out first page of Pritchard for reference.</para>
        /// <para>Also, see this Stack Overflow answer for more math background https://math.stackexchange.com/a/3937074</para>
        /// </summary>
        /// <param name="v"></param>
        /// <returns>A transposed skew matrix, so the columns are actually rows in the original skew matrix.</returns>
        public static double3x3 MakeFromVector(double3 v)
        {
            return new double3x3(
                math.double3(0, -v.z, v.y),
                math.double3(v.z, 0, -v.x),
                math.double3(-v.y, v.x, 0));
        }
    }
}