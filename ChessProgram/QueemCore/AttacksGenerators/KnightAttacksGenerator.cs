using System;
using System.Collections.Generic;
using QueemCore.BitBoard.Helpers;

namespace QueemCore.AttacksGenerators
{
	public class KnightAttacksGenerator : AttacksGenerator
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return KnightsBoardHelper.KnightMoves[(int)figureSquare];
		}
	}
}

