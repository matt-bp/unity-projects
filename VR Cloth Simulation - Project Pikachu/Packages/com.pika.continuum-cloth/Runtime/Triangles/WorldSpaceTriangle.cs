using System;
using LinearAlgebra;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Triangles
{
    public interface IWorldSpaceTriangle
    {
        public (double3 dx1, double3 dx2) GetDifferences();
    }
    
    [Serializable]
    public class WorldSpaceTriangle : IWorldSpaceTriangle
    {
        [SerializeField] private Double3 x0;
        [SerializeField] private Double3 x1;
        [SerializeField] private Double3 x2;

        public WorldSpaceTriangle(Double3 x0, Double3 x1, Double3 x2)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.x2 = x2;
        }

        public (double3 dx1, double3 dx2) GetDifferences() => (x1.Value - x0.Value, x2.Value - x0.Value);
    }
}