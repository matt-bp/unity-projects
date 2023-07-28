using System.Linq;
using UnityEngine;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<Unity.Mathematics.double2x2>>;
using GridVector = System.Collections.Generic.List<Unity.Mathematics.double2>;

namespace MattMath._2D
{
    public class ConjugateGradient
    {
        public static GridVector CgAdd(GridVector first, GridVector second)
        {
            Debug.Assert(first.Count == second.Count);

            var result = first.Select(x => x.xy).ToList();

            for (var row = 0; row < first.Count; row++)
            {
                result[row] = first[row] + second[row];
            }

            return result;
        }
    }
}