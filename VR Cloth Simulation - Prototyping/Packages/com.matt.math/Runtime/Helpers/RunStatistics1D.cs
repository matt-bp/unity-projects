using System.Linq;

namespace Helpers
{
    public class RunStatistics1D
    {
        public double DeltaTime { get; set; }
        public double Elapsed { get; set; }
        public double Position { get; set; }
        
        public string GetCSVLine()
        {
            return DeltaTime + "," + Elapsed + "," + Position;
        }

        public static string GetCSVHeader() => 
            string.Join(",", 
                from p in typeof(RunStatistics1D).GetProperties()
                where p.CanRead &&
                      p.CanWrite
                orderby p.Name
                select p.Name);
    }
}