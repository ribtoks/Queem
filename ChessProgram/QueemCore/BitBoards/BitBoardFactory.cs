using System;
using Queem.Core.AttacksGenerators;

namespace Queem.Core.BitBoards
{
	public static class BitBoardFactory
	{
		public static BitBoard CreateBitBoard(Figure figureType)
		{			
			switch (figureType)
			{
			case Figure.Pawn:
				return new PawnBitBoard();
			case Figure.Knight:
				return new KnightBitBoard();
			case Figure.Bishop:
				return new BishopBitBoard();
			case Figure.Rook:
				return new RookBitBoard();
			case Figure.Queen:
				return new QueenBitBoard();
			case Figure.King:
				return new KingBitBoard();
			default:
				return null;
			}
		}
	}
}

