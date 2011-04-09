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
            string help = @"
Usage: QueemSpeedBenchmark [-h|--help] [-v|--verbose] tests_dir_path max_depth [results_filepath]

       -h, --help : display this help
    -v, --verbose : be verbose
   tests_dir_path : path to directory with tests
        max_depth : depth
 results_filepath : path to files with results
                    (if not specified, will save results in current folder)
";

            // will fit both -h and --help
            if (args.Any((s) => s.Contains("-h")) || 
                (args.Length == 0))
            {
                Console.WriteLine(help);
                return;
            }

            int index = 0;
            bool verbose = false;

            if (args[0].Contains("-v"))
            {
                verbose = true;
                index += 1;
            }

            string testsDirectoryPath = args[index];
            string maxDepthStr = args[index + 1];

            if (string.IsNullOrEmpty(testsDirectoryPath))
            {
                Console.WriteLine("Please, specify directory with benchmarks.");
                return;
            }

            if (!Directory.Exists(testsDirectoryPath))
            {
                Console.WriteLine("Cannot find directory with benchmarks.");
                return;
            }

            int maxdepth = 0;
            int.TryParse(maxDepthStr, out maxdepth);
            
            if (maxdepth <= 0)
            {
                Console.WriteLine("<max_depth> needs to be a positive integer.");
                return;
            }

            BenchmarkProvider bp = new BenchmarkProvider(testsDirectoryPath);

            if (verbose)
                Console.WriteLine("Started testing...");

            bp.RunBenchmarks(maxdepth, verbose);

            string resultsFilePath = string.Empty;

            // then user specified output name
            if (args.Length == (3 + index))
            {
                resultsFilePath = args[3];
            }
            else
            {
                resultsFilePath = string.Format("test_results_{0}_{1}_{2}",
                    maxdepth,
                    Guid.NewGuid().ToString().Substring(0, 8),
                    DateTime.Now.Millisecond);
            }

            bp.SaveResultsToFile(resultsFilePath);

            if (verbose)
                Console.WriteLine("Testing finished. Results are saved in " + resultsFilePath);
        }
    }
}
