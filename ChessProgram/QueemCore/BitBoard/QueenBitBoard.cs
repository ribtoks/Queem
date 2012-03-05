using System;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{	
	public class QueenBitBoard : BitBoard
	{
		public QueenBitBoard (MovesProvider provider)
			:base(provider)
		{
		}
		
		public QueenBitBoard (ulong val, MovesProvider provider)
			:base(val, provider)
		{
		}
	}
}

