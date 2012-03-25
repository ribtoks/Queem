using System;
using Queem.Core.BitBoards.Helpers;
using System.Collections.Generic;
using Queem.Core.AttacksGenerators;

namespace Queem.Core.BitBoards
{
	public class KnightBitBoard : BitBoard
	{
		public KnightBitBoard()
			:base()
		{
		}
		
		public KnightBitBoard(ulong val)
			:base(val)
		{
		}
	}
}

