using System;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class BishopBitBoard : BitBoard
	{
		public BishopBitBoard (MovesProvider provider)
			:base(provider)
		{
		}
		
		public BishopBitBoard(ulong val, MovesProvider provider)
			:base(val, provider)
		{					
		}
	}
}

