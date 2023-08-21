using System.Collections.Generic;
using System.Linq;
using Codice.Client.Commands.Matcher;
using DataStructures;
using Unity.Mathematics;
using UnityEngine;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;
using GridVector = System.Collections.Generic.List<double>;

namespace Solvers
{
    public class ConjugateGradient1D
    {
        public static GridVector CgAdd(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            GridVector result = first.Select(x => 0.0).ToList();

            for (var row = 0; row < first.Count; row++)
            {
                result[row] = first[row] + second[row];
            }

            return result;
        }

        public static GridVector CgSub(GridVector firstMatrix, GridVector secondMatrix)
        {
            Debug.Assert(firstMatrix.Count == secondMatrix.Count);

            var result = firstMatrix.Select(x => 0.0).ToList();

            for (var row = 0; row < firstMatrix.Count; row++)
            {
                result[row] = firstMatrix[row] - secondMatrix[row];
            }

            return result;
        }

        public static GridVector CgMult(GridVector vector, double constant)
        {
            var result = vector.Select(x => x).ToList();

            for (var row = 0; row < vector.Count; row++)
            {
                result[row] *= constant;
            }

            return result;
        }

        public static GridVector CgMult(GridMatrix matrix, GridVector vector)
        {
            Debug.Assert(vector.Count == matrix.Count);
            var result = Enumerable
                .Range(0, vector.Count)
                .Select(_ => 0.0)
                .ToList();

            foreach (var (columnOfMatrices, columnIndex) in matrix.Select((v, i) => (v, i)))
            {
                Debug.Assert(columnOfMatrices.Count == matrix.Count);

                foreach (var (cellMatrix, rowIndex) in columnOfMatrices.Select((cv, ci) => (cv, ci)))
                {
                    result[rowIndex] += cellMatrix * vector[columnIndex];
                }
            }

            return result;
        }

        public static double CgDot(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            return first.Zip(second, (firstValue, secondValue) => firstValue * secondValue).Sum();
        }
        
        public static GridVector Solve(GridMatrix a, GridVector b, int iMax, double e)
        {
            var dv = Grid<double>.MakeVector(b.Count, 0.0);
            var i = 0;
            var r = CgSub(b, CgMult(a, dv));
            var c = r.Select(m => m).ToList();
            var deltaNew = CgDot(r, r);
            var delta0 = deltaNew;

            while (i < iMax && deltaNew > e * e * delta0)
            {
                var q = CgMult(a, c);
                var alpha = deltaNew / CgDot(c, q);
                dv = CgAdd(dv, CgMult(c, alpha));
                r = CgSub(r, CgMult(q, alpha));

                var deltaOld = deltaNew;
                deltaNew = CgDot(r, r);
                var beta = deltaNew / deltaOld;
                c = CgAdd(r, CgMult(c, beta));

                i++;
            }

            return dv;
        }
        
        public static GridVector ConstrainedSolve(GridMatrix a, GridVector b, int iMax, double e, List<int> constrainedIndices)
        {
            GridVector Filter(GridVector list) => FilterList(list, constrainedIndices);

            var dv = Grid<double>.MakeVector(b.Count, 0.0);
            var fb = Filter(b);
            var delta0 = CgDot(fb, fb);
            var r = Filter(CgSub(b, CgMult(a, dv)));
            var c = Filter(r);
            var deltaNew = CgDot(r, c);

            var i = 0;
            while (i < iMax && deltaNew > e * e * delta0)
            {
                var q = Filter(CgMult(a, c));
                var alpha = deltaNew / CgDot(c, q);
                dv = CgAdd(dv, CgMult(c, alpha));
                r = CgSub(r, CgMult(q, alpha));
                
                var deltaOld = deltaNew;
                deltaNew = CgDot(r, r);
                c = Filter(CgAdd(r, CgMult(c, deltaNew / deltaOld)));
                
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