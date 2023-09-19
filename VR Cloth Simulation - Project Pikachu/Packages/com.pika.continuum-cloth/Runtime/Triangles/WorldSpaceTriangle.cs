using System;
using Unity.Mathematics;
using UnityEngine;

namespace Triangles
{
    public class WorldSpaceTriangle
    {
        private double3 X0 { get; }
        private double3 X1 { get; }
        private double3 X2 { get; }

        public WorldSpaceTriangle(double3 x0, double3 x1, double3 x2)
        {
            X0 = x0;
            X1 = x1;
            X2 = x2;
        }

        public (double3 dx1, double3 dx2) GetDifferences() => (X1 - X0, X2 - X0);
    }
}