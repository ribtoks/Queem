using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;

namespace QueemCore.BitBoard
{
	public class RookBitBoard : BitBoard
	{
		public RookBitBoard ()
			:base()
		{
		}
		
		public RookBitBoard(ulong val)
			:base(val)
		{
		}
		
		public override IEnumerable<ulong> GetAttacks()
		{
			yield return 0;
		}
	}
}

