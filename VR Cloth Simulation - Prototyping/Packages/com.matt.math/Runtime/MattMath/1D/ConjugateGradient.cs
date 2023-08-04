using System.Linq;
using UnityEngine;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;
using GridVector = System.Collections.Generic.List<double>;

namespace MattMath._1D
{
    public static class ConjugateGradient
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
            var dv = Enumerable
                .Range(0, b.Count)
                .Select(_ => 0.0).ToList();

            var i = 0;
            var r = CgSub(b, CgMult(a, dv));
            var d = r.Select(m => m).ToList();
            var deltaNew = CgDot(r, r);
            var delta0 = deltaNew;

            while (i < iMax && deltaNew > e * e * delta0)
            {
                var q = CgMult(a, d);
                var alpha = deltaNew / CgDot(d, q);
                dv = CgAdd(dv, CgMult(d, alpha));
                r = CgSub(r, CgMult(q, alpha));

                var deltaOld = deltaNew;
                deltaNew = CgDot(r, r);
                var beta = deltaNew / deltaOld;
                d = CgAdd(r, CgMult(d, beta));

                i++;
            }

            return dv;
        }
    }
}