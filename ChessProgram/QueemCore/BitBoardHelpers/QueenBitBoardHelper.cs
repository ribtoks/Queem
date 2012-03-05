using System;
using QueemCore.BitBoard.Helpers;

namespace QueemCore
{
	public static class QueenBitBoardHelper
	{
		public static ulong GetAttacks(ulong otherFigures, Square sq)
		{
			return BishopBitBoardHelper.GetBishopAttacks(otherFigures, sq) | 
				RookBitBoardHelper.GetRookAttacks(otherFigures, sq);
		}
	}
}

