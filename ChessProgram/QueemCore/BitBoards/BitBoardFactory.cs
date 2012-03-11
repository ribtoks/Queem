using System;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public static class BitBoardFactory
	{
		public static BitBoard CreateBitBoard(Figure figureType)
		{
			var generator = AttacksGeneratorFactory.CreateGenerator(figureType);
			
			switch (figureType)
			{
			case Figure.Pawn:
				return new PawnBitBoard(generator);
			case Figure.Knight:
				return new KnightBitBoard(generator);
			case Figure.Bishop:
				return new BishopBitBoard(generator);
			case Figure.Rook:
				return new RookBitBoard(generator);
			case Figure.Queen:
				return new QueenBitBoard(generator);
			case Figure.King:
				return new KingBitBoard(generator);
			default:
				return null;
			}
		}
	}
}

