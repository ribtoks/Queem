using System;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{	
	public class QueenBitBoard : BitBoard
	{
		public QueenBitBoard (AttacksGenerator provider)
			:base(provider)
		{
		}
		
		public QueenBitBoard (ulong val, AttacksGenerator provider)
			:base(val, provider)
		{
		}
	}
}

