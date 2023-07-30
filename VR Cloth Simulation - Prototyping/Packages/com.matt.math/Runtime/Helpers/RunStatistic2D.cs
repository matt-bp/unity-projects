using System.Linq;
using Unity.Mathematics;

namespace Helpers
{
    public class RunStatistic2D : IRunStatistic
    {
        public double DeltaTime { get; set; }
        public double Elapsed { get; set; }
        public double2 Position { get; set; }
        
        public string GetCsvLine()
        {
            return DeltaTime + "," + Elapsed + "," + Position.y;
        }

        public string GetCsvHeader() => 
            "DeltaTime, Elapsed, Y Position";
    }
}