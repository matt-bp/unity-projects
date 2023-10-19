using Unity.Mathematics;

namespace TensorAlgebra
{
    public static class Double3
    {
        /// <summary>
        /// <para>Performs the tensor dot product with a matrix by dotting a column in the matrix with our inputted vector.</para>
        /// <para></para>
        /// <para>See https://math.stackexchange.com/a/4225861 for reasong and equations.</para>
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double3 Dot(this double3 vector, double3x3 matrix)
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