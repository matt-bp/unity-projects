using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SimulationHelpers.Visualization
{
    public class RunStatisticWriter
    {
        public static void Write(List<IRunStatistic> stats, string filename, string extension)
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

            Debug.Assert(stats.Count > 0);
            
            using var fs = File.Create(availableFilename);
            using var sw = new StreamWriter(fs);
            
            sw.WriteLine(stats[0].GetCsvHeader());
            
            foreach (var stat in stats)
            {
                sw.WriteLine(stat.GetCsvLine());
            }
        }
    }
}