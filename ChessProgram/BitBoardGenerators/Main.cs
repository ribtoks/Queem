using System;

namespace MovesGenerators
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var movesGenerator = GeneratorFactory.CreateGenerator("ranks");
			movesGenerator.Run();
			var results = movesGenerator.GetResults();
			movesGenerator.WriteResults(Console.Out);
		}
	}
}
