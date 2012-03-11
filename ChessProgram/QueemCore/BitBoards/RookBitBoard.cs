using System;
using QueemCore.BitBoards.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public class RookBitBoard : BitBoard
	{
		public RookBitBoard()
			:base()
		{
		}
		
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

