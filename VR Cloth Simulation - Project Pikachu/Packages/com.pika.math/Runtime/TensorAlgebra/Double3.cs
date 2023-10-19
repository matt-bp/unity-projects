using Unity.Mathematics;

namespace TensorAlgebra
{
    public static class Double3
    {
        /// <summary>
        /// <para>Performs the tensor dot product with a matrix and a vector based on rows.</para>
        /// <para>See https://math.stackexchange.com/a/4225861 for reasong and equations.</para>
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double3 Dot(this double3 vector, double3x3 matrix)
        {
            var rowsNowCols = math.transpose(matrix);
            
            var dot = double3.zero;

            dot[0] = math.dot(rowsNowCols.c0, vector);
            dot[1] = math.dot(rowsNowCols.c1, vector);
            dot[2] = math.dot(rowsNowCols.c2, vector);
            ;
            return dot;
        }
    }
}