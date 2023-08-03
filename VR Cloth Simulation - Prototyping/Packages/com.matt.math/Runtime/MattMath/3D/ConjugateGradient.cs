using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<Unity.Mathematics.double3x3>>;
using GridVector = System.Collections.Generic.List<Unity.Mathematics.double3>;


namespace MattMath._3D
{
    /// <summary>
    /// Class used to compute the conjugate gradient using grid matrices and vectors.
    ///
    /// All vectors are used as column vectors, unless specified otherwise.
    /// </summary>
    public static class ConjugateGradient
    {
        public static GridVector Add(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            return first.Zip(second, (x, y) => x + y).ToList();
        }

        public static GridVector Sub(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            return first.Zip(second, (x, y) => x - y).ToList();
        }

        /// <summary>
        /// Multiplies a vector by a constant.
        /// </summary>
        /// <param name="vector">The list of vectors we will treat as one vector</param>
        /// <param name="constant">The results from a dot product usually.</param>
        /// <returns>A vector with each element multiplied by this constant</returns>
        public static GridVector Mult(GridVector vector, double constant)
        {
            return vector.Select(x => x * constant).ToList();
        }

        public static GridVector Mult(GridMatrix matrix, GridVector vector)
        {
            Debug.Assert(vector.Count == matrix.Count);
            var result = Enumerable
                .Range(0, vector.Count)
                .Select(_ => double3.zero)
                .ToList();

            foreach (var (columnOfMatrices, columnIndex) in matrix.Select((v, i) => (v, i)))
            {
                Debug.Assert(columnOfMatrices.Count == matrix.Count);

                foreach (var (cellMatrix, rowIndex) in columnOfMatrices.Select((cv, ci) => (cv, ci)))
                {
                    result[rowIndex] += math.mul(cellMatrix, vector[columnIndex]);
                }
            }

            return result;
        }

        /// <summary>
        /// This returns the dot product from the given vectors.
        ///
        /// Even though they're lists of vectors, I need to take the dot product of each one individually, and add them up.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>A scalar</returns>
        public static double Dot(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            return first.Zip(second, math.dot).Sum();
        }
        
        public static GridVector Solve(GridMatrix a, GridVector b, int iMax, double e)
        {
            var dv = Enumerable
                .Range(0, b.Count)
                .Select(_ => double3.zero).ToList();

            var i = 0;
            var r = Sub(b, Mult(a, dv));
            var d = r.Select(m => m).ToList();
            var deltaNew = Dot(r, r);
            var delta0 = deltaNew;

            while (i < iMax && deltaNew > e * e * delta0)
            {
                var q = Mult(a, d);
                var alpha = deltaNew / Dot(d, q);
                dv = Add(dv, Mult(d, alpha));
                r = Sub(r, Mult(q, alpha));

                var deltaOld = deltaNew;
                deltaNew = Dot(r, r);
                var beta = deltaNew / deltaOld;
                d = Add(r, Mult(d, beta));

                i++;
            }

            return dv;
        }
    }
}