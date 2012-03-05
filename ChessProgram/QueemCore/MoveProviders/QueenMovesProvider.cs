using System;

namespace QueemCore.MovesProviders
{
	public class QueenMovesProvider : MovesProvider
	{
		protected RookMovesProvider rookProvider = new RookMovesProvider();
		protected BishopMovesProvider bishopProvider = new BishopMovesProvider();		
	
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			return this.rookProvider.GetAttacks(figureSquare, otherFigures) | 
				this.bishopProvider.GetAttacks(figureSquare, otherFigures);
		}
	}
}

