using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BenchmarksGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string help = @"
Usage: BenchmarkGenerator [-h|--help] [-v|--verbose] test_count depth results_path

     -h, --help : display this help
  -v, --verbose : be verbose
    tests_count : number of tests to generate
          depth : number of half-moves of each player 
                  to generate in each test
   results_path : path to folder where to save benchmarks
                  (if folder does not exist, 
                   it will be created)
";

            // will fit both -h and --help
            if (args.Any((s) => s.Contains("-h")) || 
                (args.Length == 0))
            {
                Console.WriteLine(help);
                return;
            }

            bool verbose = false;

            int index = 0;

            if (args[0].Contains("-v"))
            {
                verbose = true;
                index += 1;
            }

            string testsCountStr = args[index];
            string depthStr = args[index + 1];
            string path = args[index + 2];

            int depth = 0;
            int.TryParse(depthStr, out depth);

            if (depth <= 0)
            {
                Console.WriteLine("<depth> needs to be a positive integer.");
                return;
            }

            if (!Directory.Exists(path))
            {
                // create specified path with subdirs
                Directory.CreateDirectory(path);
                Console.WriteLine("Tests directory created");
            }

            int testsCount = 0;
            int.TryParse(testsCountStr, out testsCount);
            int testNumber = 1;
            while (testNumber <= testsCount)
            {
                var moves = BenchmarkGenerator.GenerateSituation(depth);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < moves.Count; ++i)
                {
                    sb.AppendLine(moves[i].ToString());
                }

                string filePath = path;
                if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    filePath += Path.DirectorySeparatorChar;

                filePath += string.Format("test{0}_{1}.cbi",
                    testNumber,
                    DateTime.Now.Millisecond//ToString("dd_MM_yyyy")
                    );

                File.WriteAllText(filePath, sb.ToString());

                if (verbose)
                    Console.WriteLine(
                            string.Format("Test {0} of {1}...",
                                testNumber,
                                testsCount)
                            );

                testNumber += 1;
            }
        }
    }
}
