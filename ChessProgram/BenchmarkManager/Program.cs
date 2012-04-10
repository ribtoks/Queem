using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BenchmarkManager
{
    class Program
    {
        static string[] possible_commands = { "-r", "--run", "-c", "--cmp" };
        static string[] possible_verbose_commands = { "-v", "--verbose" };
                
        static void Main(string[] args)
        {
            string help = @"
Usage: 
    BenchmarkManager [-h|--help] -r|--run [-v|--verbose] 
                     gen_exe test_exe 
                     tests_count test_depth 
                     max_depth 
                     [results_path]

    BenchmarkManager [-h|--help] -c|--cmp file1 file2 results_file


        -h, --help : display this help

Run options:
         -r, --run : create tests and than run them
     -v, --verbose : be verbose
           gen_exe : path to program, that can generate tests
          test_exe : path to program, that can run tests
       tests_count : number of tests to generate
       tests_depth : number of moves, generated in every test
         max_depth : maximum depth for AI when solving tests
      results_path : filename with test results

Compare options:
         -c, --cmp : compare two files with test results
             file1 : path to first file with tests
             file2 : path to second file with tests
      results_file : file with prepared for graphic comparison data
";

            if ((args.Length == 0) ||
                (args[0].ToLower().Contains("-h")) ||
                (args.Length < 4))
            {
                Console.WriteLine(help);
                return;
            }

            string command = args[0].ToLower();
            

            if (!possible_commands.Contains(command))
            {
                Console.WriteLine(help);
                return;
            }

            switch (command)
            {
                case "-r":
                case "--run":

                    try
                    {
                        RunTests(args, help);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "-c":
                case "--cmp":
                    string file1 = args[1];
                    if (!File.Exists(file1))
                    {
                        Console.WriteLine("Cannot find first file");
                        return;
                    }

                    string file2 = args[2];
                    if (!File.Exists(file2))
                    {
                        Console.WriteLine("Cannot find second file");
                        return;
                    }

                    string results_file = args[3];

                    var data1 = ReadTestsFile(file1);
                    var data2 = ReadTestsFile(file2);

                    if (data1.Count != data2.Count)
                    {
                        Console.WriteLine("Different number of tests in files");
                        return;
                    }

                    try
                    {
                        data1 = Transpose(data1);
                        data2 = Transpose(data2);
                    }
                    catch
                    {
                        Console.WriteLine("Data has different columns");
                        return;
                    }

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < data1.Count; ++i)
                    {
                        sb.AppendLine(string.Join(" ", data1[i]));
                        sb.AppendLine(string.Join(" ", data2[i]));
                        sb.AppendLine();
                    }

                    File.WriteAllText(results_file, sb.ToString());

                    break;
            }
        }

        static void RunTests(string[] args, string help)
        {
            if (args.Length < 6)
            {
                Console.WriteLine(help);
                return;
            }

            bool verbose = false;
            int index = 1;
            if (possible_verbose_commands.Contains(args[index].ToLower()))
            {
                verbose = true;
                index += 1;
            }

            string generatorPath = args[index];
            if (!File.Exists(generatorPath))
            {
                Console.WriteLine("Benchmarks generator cannot be found");
                return;
            }

            string testurerPath = args[index + 1];
            if (!File.Exists(testurerPath))
            {
                Console.WriteLine("Benchmark runner cannot be found");
                return;
            }

            int tests_count = 0;
            int.TryParse(args[index + 2], out tests_count);
            if (tests_count <= 0)
            {
                Console.WriteLine("<tests_count> must be a positive integer");
                return;
            }

            int tests_depth = 0;
            int.TryParse(args[index + 3], out tests_depth);
            if (tests_depth <= 0)
            {
                Console.WriteLine("<tests_depth> must be a positive integer");
                return;
            }

            int max_depth = 0;
            int.TryParse(args[index + 4], out max_depth);
            if (max_depth <= 0)
            {
                Console.WriteLine("<max_depth> must be a positive integer");
                return;
            }

            string resultsFilePath = string.Empty;
            if (args.Length == (6 + index))
            {
                resultsFilePath = " " + args[5 + index];
            }

            // path to current result folder
            string testsFolder = "./tests_bundle_" +
                Guid.NewGuid().ToString().Substring(0, 8);

            // now run processes with correct parameters
            ProcessStartInfo gen_psi = new ProcessStartInfo();
            gen_psi.UseShellExecute = false;
            gen_psi.FileName = generatorPath;

            string verboseStr = string.Empty;
            if (verbose)
                verboseStr = "--verbose ";

            gen_psi.Arguments = string.Format(
                "{0}{1} {2} {3}",
                verboseStr,
                tests_count,
                tests_depth,
                testsFolder
                );
            

            Process generateTests = Process.Start(gen_psi);
            generateTests.WaitForExit();

            if (verbose)
                Console.WriteLine("Tests're generated. Starting solving...\r\n");

            // now start solving process

            ProcessStartInfo solve_psi = new ProcessStartInfo();
            solve_psi.UseShellExecute = false;
            solve_psi.FileName = testurerPath;

            solve_psi.Arguments = string.Format(
                "{0}{1} {2}{3}",
                verboseStr,
                testsFolder,
                max_depth,
                resultsFilePath
                );

            Process solveTests = Process.Start(solve_psi);
            solveTests.WaitForExit();

            if (verbose)
                Console.WriteLine("\r\nTests're solved.\r\n");
        }

        static List<List<string>> ReadTestsFile(string filePath)
        {
            var data = new List<List<string>>();

            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] values = line.Split(' ');

                data.Add(new List<string>(values));
            }

            return data;
        }

        static List<List<T>> Transpose<T>(List<List<T>> matr)
        {
            var list = new List<List<T>>();

            for (int j = 0; j < matr[0].Count; ++j)
                list.Add(new List<T>());


            for (int j = 0; j < matr[0].Count; ++j)
            {
                list[j] = new List<T>();

                for (int i = 0; i < matr.Count; ++i)
                {
                    list[j].Add(matr[i][j]);
                }
            }

            return list;
        }
    }
}
