using System.Linq;

namespace Helpers
{
    public class RunStatistic1D : IRunStatistic
    {
        public double DeltaTime { get; set; }
        public double Elapsed { get; set; }
        public double Position { get; set; }
        
        public string GetCsvLine()
        {
            return DeltaTime + "," + Elapsed + "," + Position;
        }

        public string GetCsvHeader() => 
            string.Join(",", 
                from p in typeof(RunStatistic1D).GetProperties()
                where p.CanRead &&
                      p.CanWrite
                orderby p.Name
                select p.Name);
    }
}