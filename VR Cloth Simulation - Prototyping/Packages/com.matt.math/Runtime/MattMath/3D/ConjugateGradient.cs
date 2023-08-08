using System.Collections.Generic;
using System.Linq;
using Helpers;
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

            Debug.Assert(i != iMax, "Hit the max limit of iterations in the 3D solve.");

            return dv;
        }
        
        /// <summary>
        /// Solves the system of equations for x or ax = b. Throughout the solve, it will remove values for all constrained indices.
        /// </summary>
        /// <param name="a">Left side matrix</param>
        /// <param name="b">Right side vector</param>
        /// <param name="iMax">Max iteration count</param>
        /// <param name="e">Desired difference between solutions</param>
        /// <param name="constrainedIndices">List of indices to set as zero. Will remove all values in all directions.</param>
        /// <returns></returns>
        public static GridVector ConstrainedSolve(GridMatrix a, GridVector b, int iMax, double e,
            List<int> constrainedIndices)
        {
            GridVector Filter(GridVector list) => FilterList(list, constrainedIndices);

            var dv = Grid<double3>.MakeVector(b.Count, 0.0);
            var fb = Filter(b);
            var delta0 = Dot(fb, fb);
            var r = Filter(Sub(b, Mult(a, dv)));
            var c = Filter(r);
            var deltaNew = Dot(r, c);

            var i = 0;
            while (i < iMax && deltaNew > e * e * delta0)
            {
                var q = Filter(Mult(a, c));
                var alpha = deltaNew / Dot(c, q);
                dv = Add(dv, Mult(c, alpha));
                r = Sub(r, Mult(q, alpha));
                
                var deltaOld = deltaNew;
                deltaNew = Dot(r, r);
                c = Filter(Add(r, Mult(c, deltaNew / deltaOld)));
                
                i++;
            }

            return dv;
        }
        
        /// <summary>
        /// Creates a new vector, copying the contents of the passed in vector, and zeros out the indices passed in constrainedIndices.
        /// </summary>
        /// <param name="vector">Vector to filter.</param>
        /// <param name="constrainedIndices">Indices to target.</param>
        /// <returns>The filtered vector.</returns>
        private static GridVector FilterList(GridVector vector, IEnumerable<int> constrainedIndices)
        {
            var result = vector.Select(m => m).ToList();
            
            foreach (var index in constrainedIndices)
            {
                result[index] = 0;
            }
            
            return result;
        }
    }
}