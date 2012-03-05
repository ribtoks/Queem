using System;
using System.Collections.Generic;
using QueemCore.BitBoard.Helpers;

namespace QueemCore.MovesProviders
{
	public class KnightMovesProvider : MovesProvider
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return KnightsBoardHelper.KnightMoves[(int)figureSquare];
		}
	}
}

