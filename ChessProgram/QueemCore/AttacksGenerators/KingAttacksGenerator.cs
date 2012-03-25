using System;
using Queem.Core.BitBoards.Helpers;

namespace Queem.Core.AttacksGenerators
{
	public class KingAttacksGenerator : AttacksGenerator
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return KingBitBoardHelper.KingMoves[(int)figureSquare];
		}
	}
}

