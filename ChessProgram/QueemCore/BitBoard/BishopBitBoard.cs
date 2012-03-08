using System;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class BishopBitBoard : BitBoard
	{
		public BishopBitBoard (AttacksGenerator provider)
			:base(provider)
		{
		}
		
		public BishopBitBoard(ulong val, AttacksGenerator provider)
			:base(val, provider)
		{					
		}
	}
}

