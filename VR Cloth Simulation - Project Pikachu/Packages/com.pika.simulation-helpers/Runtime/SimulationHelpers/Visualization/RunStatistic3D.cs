using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace SimulationHelpers.Visualization
{
    public class RunStatistic3D : IRunStatistic
    {
        public double DeltaTime { get; set; }
        public double Elapsed { get; set; }
        public List<double3> Positions { get; set; }

        public string GetCsvLine()
        {
            return DeltaTime + "," + Elapsed + "," + GetPositionLine();
        }
        
        private string GetPositionLine() => string.Join(',', Positions.Select((v, _) => $"{v.x},{v.y},{v.z}"));


        public string GetCsvHeader() => 
            "DeltaTime,Elapsed," + GetPositionHeader();
        
        private string GetPositionHeader() => string.Join(',', Positions.Select((_, i) => $"{i} Position X, {i} Position Y, {i} Position Z"));

    }
}