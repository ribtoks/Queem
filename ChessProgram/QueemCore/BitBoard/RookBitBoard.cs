using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class RookBitBoard : BitBoard
	{			
		public RookBitBoard (AttacksGenerator provider)
			:base(provider)
		{
		}
		
		public RookBitBoard(ulong val, AttacksGenerator provider)
			:base(val, provider)
		{
		}		
	}
}

