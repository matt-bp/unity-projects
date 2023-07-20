using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;
using GridVector = System.Collections.Generic.List<double>;
using Unity.Mathematics;

namespace MattMath
{
    public class ConjugateGradient1D
    {
        public static GridVector CgAdd(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            GridVector result = Enumerable.Range(0, first.Count).Select(_ => 0.0).ToList();

            for (var row = 0; row < first.Count; row++)
            {
                result[row] = first[row] + second[row];
            }

            return result;
        }
    }
}