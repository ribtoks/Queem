using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoard
{
	public class RookBitBoard : BitBoard
	{			
		public RookBitBoard (AttacksGenerator generator)
			:base(generator)
		{
		}
		
		public RookBitBoard(ulong val, AttacksGenerator generator)
			:base(val, generator)
		{
		}		
	}
}

