using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QueemSpeedBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            string testsDirectoryPath = args[0];
            string maxDepthStr = args[1];

            if (string.IsNullOrEmpty(testsDirectoryPath))
                return;

            if (!Directory.Exists(testsDirectoryPath))
                return;

            int maxdepth = 0;
            int.TryParse(maxDepthStr, out maxdepth);

            BenchmarkProvider bp = new BenchmarkProvider(testsDirectoryPath);

            Console.WriteLine("Started testing...");
            bp.RunBenchmarks(maxdepth);

            string resultsFilePath = string.Format("test_results_{0}_{1}",
                Guid.NewGuid().ToString().Substring(0, 8),
                DateTime.Now.Millisecond);                

            bp.SaveResultsToFile(resultsFilePath);
        }
    }
}
