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
        public List<Vector3> Positions { get; set; }
        public List<Vector3> Velocities { get; set; }

        public string GetCsvLine()
        {
            return DeltaTime + "," + Elapsed + "," + GetPositionLine() + "," + GetVelocityLine();
        }
        
        private string GetPositionLine() => string.Join(',', Positions.Select((v, _) => $"{v.x},{v.y},{v.z}"));
        private string GetVelocityLine() => string.Join(',', Velocities.Select((v, _) => $"{v.x},{v.y},{v.z}"));

        
        public string GetCsvHeader() => 
            "DeltaTime,Elapsed," + GetPositionHeader() + "," + GetVelocityHeader();
        
        private string GetPositionHeader() => string.Join(',', Positions.Select((_, i) => $"{i} Position X, {i} Position Y, {i} Position Z"));
        private string GetVelocityHeader() => string.Join(',', Velocities.Select((_, i) => $"{i} Velocity X, {i} Velocity Y, {i} Velocity Z"));

    }
}