using System;

namespace Queem.Core.AttacksGenerators
{
	public class QueenAttacksGenerator : AttacksGenerator
	{
		protected RookAttacksGenerator rookProvider = new RookAttacksGenerator();
		protected BishopAttacksGenerator bishopProvider = new BishopAttacksGenerator();		
	
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return this.rookProvider.GetAttacks(figureSquare, otherFigures) | 
				this.bishopProvider.GetAttacks(figureSquare, otherFigures);
		}
	}
}

