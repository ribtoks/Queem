using System;
using QueemCore.BitBoards.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public class KnightBitBoard : BitBoard
	{
		public KnightBitBoard()
			:base()
		{
		}
	
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

