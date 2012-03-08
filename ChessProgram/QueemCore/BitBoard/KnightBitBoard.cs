using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class KnightBitBoard : BitBoard
	{
		public KnightBitBoard (AttacksGenerator provider)
			:base(provider)
		{
		}
		
		public KnightBitBoard(ulong val, AttacksGenerator provider)
			:base(val, provider)
		{
		}		
	}
}

