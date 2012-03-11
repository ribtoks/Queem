using System;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public class BishopBitBoard : BitBoard
	{
		public BishopBitBoard()
			:base()
		{
		}
	
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

