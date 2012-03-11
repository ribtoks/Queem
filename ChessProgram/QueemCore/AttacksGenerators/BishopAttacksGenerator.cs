using System;
using QueemCore.BitBoards.Helpers;

namespace QueemCore.AttacksGenerators
{
	public class BishopAttacksGenerator : AttacksGenerator
	{
		public ulong DiagonalAttacks(Square sq, ulong otherFigures)
		{	
			int rank = (int)sq >> 3;
			int file = (int)sq & 7;
			
			ulong figurePos = 1UL << (int)sq;
			ulong reversedFigurePos = 1UL << ((int)sq ^ 63);
			
			ulong diagonalMask = BishopBitBoardHelper.DiagonalsMasks[7 + file - rank];

			ulong forward = otherFigures & diagonalMask;
			ulong reverse = Int64Helper.GetReversedUlong(forward);

			forward -= figurePos;
			reverse -= reversedFigurePos;

			forward ^= Int64Helper.GetReversedUlong(reverse);
			forward &= diagonalMask;
			return forward;
		}
		
		public ulong AntiDiagonalAttacks(Square sq, ulong otherFigures)
		{
			int rank = (int)sq >> 3;
			int file = (int)sq & 7;
			
			ulong figurePos = 1UL << (int)sq;
			ulong reversedFigurePos = 1UL << ((int)sq ^ 63);
			
			ulong antiDiagonalMask = BishopBitBoardHelper.AntiDiagonalsMasks[file + rank];

			ulong forward = otherFigures & antiDiagonalMask;
			ulong reverse = Int64Helper.GetReversedUlong(forward);

			forward -= figurePos;
			reverse -= reversedFigurePos;

			forward ^= Int64Helper.GetReversedUlong(reverse);
			forward &= antiDiagonalMask;
			return forward;
		}
		
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return this.DiagonalAttacks(figureSquare, otherFigures) | 
				this.AntiDiagonalAttacks(figureSquare, otherFigures);
		}
	}
}

