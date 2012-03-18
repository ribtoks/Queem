using System;
using QueemCore.BitBoards.Helpers;

namespace QueemCore.AttacksGenerators
{
	public class KingAttacksGenerator : AttacksGenerator
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return KingBitBoardHelper.KingMoves[(int)figureSquare];
		}
	}
}

