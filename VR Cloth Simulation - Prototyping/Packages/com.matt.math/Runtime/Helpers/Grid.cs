using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class Grid<T>
    {
        public static List<List<T>> MakeMatrix(int count, T value) => Enumerable
            .Range(0, count)
            .Select(_ => Enumerable.Range(0, count)
                .Select(_ => value /* Set value of cells here */)
                .ToList())
            .ToList();

        public static List<T> MakeVector(int count, T value) => Enumerable
            .Range(0, count)
                .Select(_ => value /* Set value of cells here */)
            .ToList();
    }
}