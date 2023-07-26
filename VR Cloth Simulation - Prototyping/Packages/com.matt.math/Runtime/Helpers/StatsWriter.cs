using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Helpers
{
    public class StatsWriter
    {
        public static void WriteRunStatistics(List<RunStatistics1D> stats, string filename, string extension)
        {
            var availableFilename = filename + "." + extension;
            var attempt = 1;
            const int maxAttempts = 1000;
            
            while (File.Exists(availableFilename) && attempt < maxAttempts)
            {
                availableFilename = $"{filename}-{attempt}.{extension}";
                attempt++;
            }
            
            Debug.Assert(attempt < maxAttempts);
            
            using var fs = File.Create(availableFilename);
            using var sr = new StreamWriter(fs);
            
            sr.WriteLine(RunStatistics1D.GetCSVHeader());
            
            foreach (var stat in stats)
            {
                sr.WriteLine(stat.GetCSVLine());
            }
        }
    }
}