using System;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoard
{	
	public class QueenBitBoard : BitBoard
	{
		public QueenBitBoard()
			:base()
		{
		}
	
		public QueenBitBoard (AttacksGenerator generator)
			:base(generator)
		{
		}
		
		public QueenBitBoard (ulong val, AttacksGenerator generator)
			:base(val, generator)
		{
		}
	}
}

