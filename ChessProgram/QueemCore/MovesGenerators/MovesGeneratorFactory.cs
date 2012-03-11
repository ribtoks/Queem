using System;
using QueemCore;
using QueemCore.BitBoards;
using QueemCore.AttacksGenerators;

namespace QueemCore.MovesGenerators
{
	public static class MovesGeneratorFactory
	{
		public static MovesGenerator CreateGenerator(Figure figureType, BitBoard bb, AttacksGenerator ag)
		{
			switch (figureType)
			{
			case Figure.Pawn:
				return null;
			case Figure.Knight:
				return new MovesGenerator(bb, ag);
			case Figure.Bishop:
				return new MovesGenerator(bb, ag);
			case Figure.Rook:
				return null;
			case Figure.Queen:
				return new MovesGenerator(bb, ag);
			case Figure.King:
				return null;
			default:
				return null;
			}
		}
	}
}

