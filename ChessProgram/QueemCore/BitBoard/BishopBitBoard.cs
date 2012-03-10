using System;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoard
{
	public class BishopBitBoard : BitBoard
	{
		public BishopBitBoard (AttacksGenerator generator)
			:base(generator)
		{
		}
		
		public BishopBitBoard(ulong val, AttacksGenerator generator)
			:base(val, generator)
		{					
		}
	}
}

