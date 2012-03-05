using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class KingBitBoard : BitBoard
	{
		public KingBitBoard (MovesProvider provider)
			:base(provider)
		{
		}
		
		public KingBitBoard(ulong val, MovesProvider provider)
			:base(val, provider)
		{
		}		
	}
}
