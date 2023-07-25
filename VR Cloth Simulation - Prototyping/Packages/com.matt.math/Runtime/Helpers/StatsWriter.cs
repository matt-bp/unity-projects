using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Helpers
{
    public class StatsWriter
    {
        public static void WriteRunStatistics(List<RunStatistics1D> stats, string filename)
        {
            using var fs = File.Create(filename);
            using var sr = new StreamWriter(fs);
            
            sr.WriteLine("Elapsed, Position");

            foreach (var stat in stats)
            {
                sr.WriteLine(stat.Elapsed + "," + stat.Position);
            }
        }
    }
}