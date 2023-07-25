using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Helpers
{
    public class StatsWriter
    {
        public static void WriteRunStatistics(List<RunStatistics1D> stats)
        {
            Debug.Log("Writing file to " + System.IO.Directory.GetCurrentDirectory());
            
            using var fs = System.IO.File.Create("./Stats/stats.csv");
            using var sr = new StreamWriter(fs);
            
            sr.WriteLine("Elapsed, Position");

            foreach (var stat in stats)
            {
                sr.WriteLine(stat.Elapsed + "," + stat.Position);
            }
        }
    }
}