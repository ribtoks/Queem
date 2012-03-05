using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class RookBitBoard : TwoPiecesBitBoard
	{			
		public RookBitBoard (MovesProvider provider)
			:base(provider)
		{
		}
		
		public RookBitBoard(ulong val, MovesProvider provider)
			:base(val, provider)
		{
		}		
	}
}

