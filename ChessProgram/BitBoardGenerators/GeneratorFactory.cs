using System;

namespace MovesGenerators
{
	public static class GeneratorFactory
	{
		public static Generator CreateGenerator(string name)
		{
			switch (name.ToLower())
			{
			case "king":
				return new KingMovesGenerator();
			case "knight":
				return new KnightMovesGenerator();
			case "rook":
				return new RookMovesGenerator();
			case "files":
				return new FileMasksGenerator();
			case "ranks":
				return new RankMasksGenerator();
			case "bishop":
				return new BishopMovesGenerator();
			default:
				return new DummyGenerator();
			}
		}
	}
}

