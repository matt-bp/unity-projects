using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using GridMatrix = System.Collections.Generic.List<System.Collections.Generic.List<Unity.Mathematics.double2x2>>;
using GridVector = System.Collections.Generic.List<Unity.Mathematics.double2>;

namespace MattMath._2D
{
    public static class ConjugateGradient
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
    }
}