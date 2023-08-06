using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public class RunStatistic1D : IRunStatistic
    {
        public double DeltaTime { get; set; }
        public double Elapsed { get; set; }
        public List<double> Positions { get; set; }
        public List<double> Velocities { get; set; }
        
        public string GetCsvLine()
        {
            return DeltaTime + "," + Elapsed + "," + GetPositionLine() + "," + GetVelocityLive();
        }

        private string GetPositionLine() => string.Join(',', Positions.Select((v, _) => v));

        private string GetVelocityLive() => string.Join(',', Velocities.Select((v, _) => v));

        public string GetCsvHeader()
        {
            return "DeltaTime,Elapsed," + GetPositionHeader() + "," + GetVelocityHeader();
        }

        private string GetPositionHeader() => string.Join(',', Positions.Select((_, i) => i + " Position Y"));
        
        private string GetVelocityHeader() => string.Join(',', Velocities.Select((_, i) => i + " Velocity Y"));
    }
}