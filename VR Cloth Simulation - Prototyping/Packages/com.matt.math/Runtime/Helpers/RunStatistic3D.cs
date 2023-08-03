using Unity.Mathematics;

namespace Helpers
{
    public class RunStatistic3D : IRunStatistic
    {
        public double DeltaTime { get; set; }
        public double Elapsed { get; set; }
        public double3 Position { get; set; }
        
        public string GetCsvLine()
        {
            return DeltaTime + "," + Elapsed + "," + Position.x + "," + Position.y + "," + Position.z;
        }

        public string GetCsvHeader() => 
            "DeltaTime, Elapsed, X Position, Y Position, Z Position";
    }
}