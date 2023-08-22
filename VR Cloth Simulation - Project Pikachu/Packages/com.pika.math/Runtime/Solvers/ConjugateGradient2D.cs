using System.Collections.Generic;
using System.Linq;
using DataStructures;
using Unity.Mathematics;
using UnityEngine;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<Unity.Mathematics.double2x2>>;
using GridVector = System.Collections.Generic.List<Unity.Mathematics.double2>;


namespace Solvers
{
    public class ConjugateGradient2D
    {
        public static GridVector Add(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            var result = first.Select(x => x.xy).ToList();

            for (var row = 0; row < first.Count; row++)
            {
                result[row] = first[row] + second[row];
            }

            return result;
        }
        
        public static GridVector Sub(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            var result = first.Select(x => x.xy).ToList();

            for (var row = 0; row < first.Count; row++)
            {
                result[row] = first[row] - second[row];
            }

            return result;
        }
        
        /// <summary>
        /// Multiplies a vector by a constant.
        /// </summary>
        /// <param name="vector">The list of vectors we will treat as one vector</param>
        /// <param name="constant">The results from a dot product usually.</param>
        /// <returns>A vector with each element multiplied by this constant</returns>
        public static GridVector Mult(GridVector vector, double constant)
        {
            var result = vector.Select(x => x).ToList();

            for (var row = 0; row < vector.Count; row++)
            {
                result[row] *= constant;
            }

            return result;
        }
        
        public static GridVector Mult(GridMatrix matrix, GridVector vector)
        {
            Debug.Assert(vector.Count == matrix.Count);
            var result = Enumerable
                .Range(0, vector.Count)
                .Select(_ => double2.zero)
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
                .Select(_ => double2.zero).ToList();

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

        public static GridVector ConstrainedSolve(GridMatrix a, GridVector b, int iMax, double e,
            List<int> constrainedIndices)
        {
            GridVector Filter(GridVector list) => FilterList(list, constrainedIndices);

            var dv = Grid<double2>.MakeVector(b.Count, 0.0);
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