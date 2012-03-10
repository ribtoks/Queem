using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoard
{
	public class KnightBitBoard : BitBoard
	{
		public KnightBitBoard (AttacksGenerator generator)
			:base(generator)
		{
		}
		
		public KnightBitBoard(ulong val, AttacksGenerator generator)
			:base(val, generator)
		{
		}		
	}
}

