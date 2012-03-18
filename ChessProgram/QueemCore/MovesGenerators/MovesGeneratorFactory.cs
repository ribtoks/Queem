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
				return new PawnMovesGenerator(bb, ag);
			case Figure.Knight:
			case Figure.Bishop:
			case Figure.Rook:
			case Figure.Queen:
				return new MovesGenerator(bb, ag);
			case Figure.King:
				return new KingMovesGenerator(bb, ag);
			default:
				return null;
			}
		}
	}
}

