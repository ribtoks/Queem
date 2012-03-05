using System;
using QueemCore.BitBoard.Helpers;

namespace QueemCore.MovesProviders
{
	public class KingMovesProvider : MovesProvider
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			ulong king = 1UL << (int)figureSquare;
			ulong attacks = BitBoardHelper.ShiftEastOne(king) | BitBoardHelper.ShiftWestOne(king);
			ulong temp = king | attacks;
			attacks |= BitBoardHelper.ShiftNorthOne(temp) | BitBoardHelper.ShiftSouthOne(temp);
			return attacks;
		}
	}
}

