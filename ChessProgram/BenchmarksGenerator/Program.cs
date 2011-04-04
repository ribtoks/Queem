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
            string testsCountStr = args[0];
            string depthStr = args[1];
            string path = args[2];

            int depth = 0;
            int.TryParse(depthStr, out depth);

            if (!Directory.Exists(path))
            {
                // create specified path with subdirs
                Directory.CreateDirectory(path);
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

                filePath += string.Format("test{0}.cbi", testNumber);

                File.WriteAllText(filePath, sb.ToString());

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
