using System;
using Unity.Mathematics;
using UnityEngine;

namespace LinearAlgebra
{
    [Serializable]
    public class Double3
    {
        [SerializeField] private double3 value;
        public double3 Value
        {
            get => value;
            set => this.value = value;
        }

        public Double3(double3 vector)
        {
            value = vector;
        }
        
        public static double3x3 OuterProduct(double3 column, double3 row)
        {
            var result = double3x3.zero;

            result[0][0] = column.x * row.x;
            result[0][1] = column.y * row.x;
            result[0][2] = column.z * row.x;

            result[1][0] = column.x * row.y;
            result[1][1] = column.y * row.y;
            result[1][2] = column.z * row.y;
            
            result[2][0] = column.x * row.z;
            result[2][1] = column.y * row.z;
            result[2][2] = column.z * row.z;
            
            return result;
        }


    }
}