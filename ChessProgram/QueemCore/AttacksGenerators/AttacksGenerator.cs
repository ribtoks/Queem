using System;

namespace Queem.Core.AttacksGenerators
{
	public abstract class AttacksGenerator
	{
		public abstract ulong GetAttacks(Square figures, ulong otherFigures);
	}
}

