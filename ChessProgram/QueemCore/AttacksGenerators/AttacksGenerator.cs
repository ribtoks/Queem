using System;

namespace QueemCore.MovesProviders
{
	public abstract class AttacksGenerator
	{
		public abstract ulong GetAttacks(Square figureSquare, ulong otherFigures);
	}
}

