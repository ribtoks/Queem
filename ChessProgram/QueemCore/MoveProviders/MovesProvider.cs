using System;

namespace QueemCore.MovesProviders
{
	public abstract class MovesProvider
	{
		public abstract ulong GetAttacks(Square figureSquare, ulong otherFigures);
	}
}

