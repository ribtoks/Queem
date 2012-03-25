using System;
using System.Collections.Generic;
using Queem.Core.AttacksGenerators;

namespace Queem.Core.BitBoards
{	
	public class QueenBitBoard : BitBoard
	{
		public QueenBitBoard()
			:base()
		{
		}
		
		public QueenBitBoard (ulong val)
			:base(val)
		{
		}
	}
}

