using System;
using System.Collections.Generic;
using Queem.Core.BitBoards.Helpers;

namespace Queem.Core.AttacksGenerators
{
	public class KnightAttacksGenerator : AttacksGenerator
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return KnightBitBoardHelper.KnightMoves[(int)figureSquare];
		}
	}
}

