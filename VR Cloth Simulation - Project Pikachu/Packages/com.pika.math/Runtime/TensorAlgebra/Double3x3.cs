using Unity.Mathematics;

namespace TensorAlgebra
{
    public static class Double3x3
    {
        /// <summary>
        /// <para>Performs the tensor dot product with a matrix and a vector based on columns.</para>
        /// <para>See https://math.stackexchange.com/a/4225861 for reasong and equations.</para>
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double3 Dot(this double3x3 matrix, double3 vector)
        {
            var dot = double3.zero;
            
            dot[0] = math.dot(matrix.c0, vector);
            dot[1] = math.dot(matrix.c1, vector);
            dot[2] = math.dot(matrix.c2, vector);
            ;
            return dot;
        }
    }
}